using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    [Header("Scripts")]
    PlayerManager playerManager;
    InputHandler inputHandler;
    AnimatorHandler animatorHandler;
    CameraHandler cameraHandler;

    [Header("Components")]
    [HideInInspector] public Transform myTransform;
    [HideInInspector] public Rigidbody rigi;
    [HideInInspector] public GameObject normalCamera;
    Transform cameraObject;

    [Header("Ground & Air Stats")]
    [HideInInspector] public float inAirTimer;
    float groundDetectionRayStartPoint = 0.5f; //Place our raycast origin 0.5f above our player transform. Placing it where the floating begins
    float minimumdistanceToBeginFall = 1f;
    float groundDirectionRayDistance = 0.2f;
    LayerMask ignoreForGroundCheck;

    [Header("Movement Stats")]
    [HideInInspector] public Vector3 moveDirection;
    float movementSpeed = 5;
    float sprintSpeed = 7;
    float rotationSpeed = 2000;
    float fallingSpeed = 500;

    public CapsuleCollider characterCollider;
    public CapsuleCollider characterCollisionBlocker;

    void Start()
    {
        rigi = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        playerManager = GetComponent<PlayerManager>();
        cameraHandler = FindObjectOfType<CameraHandler>();
        cameraObject = Camera.main.transform;
        myTransform = transform;
        animatorHandler.Initialize();

        playerManager.isGrounded = true;
        ignoreForGroundCheck = ~(1 << 8) | (1 << 11);

        Physics.IgnoreCollision(characterCollider, characterCollisionBlocker, true);
    }

    #region Movement
    Vector3 normalvector;
    Vector3 targetPosition;

    private void HandleRotation(float delta)
    {
        if(inputHandler.lockOnFlag)
        {
            if(inputHandler.sprintFlag || inputHandler.rollFlag)
            {
                Vector3 targetDirection = Vector3.zero;
                targetDirection = cameraHandler.cameraTransform.forward * inputHandler.vertical;
                targetDirection += cameraHandler.cameraTransform.right * inputHandler.horizontal;
                targetDirection.Normalize();
                targetDirection.y = 0;

                if (targetDirection == Vector3.zero)
                {
                    targetDirection = transform.forward;
                }

                Quaternion tr = Quaternion.LookRotation(targetDirection);
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);

                transform.rotation = targetRotation;
            }
            else
            {
                Vector3 rotationDirection = moveDirection;
                rotationDirection = cameraHandler.currentLockOnTarget.transform.position - transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                transform.rotation = targetRotation;
            }
        }
        else
        {
            Vector3 targetDir = Vector3.zero;
            float moveOverride = inputHandler.moveAmount;

            targetDir = cameraObject.forward * inputHandler.vertical;
            targetDir += cameraObject.right * inputHandler.horizontal;
            targetDir.Normalize();
            targetDir.y = 0;
            if (targetDir == Vector3.zero)
            {
                targetDir = myTransform.forward;
            }

            float rs = rotationSpeed;
            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

            myTransform.rotation = targetRotation;
        }
    }

    public void HandleMovement(float delta)
    {
        if (inputHandler.rollFlag) return;
        if (playerManager.isInteracting) return;

        moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        float speed = movementSpeed;
        if(inputHandler.sprintFlag && inputHandler.moveAmount > 0.5f)
        {
            speed = sprintSpeed;
            playerManager.isSprinting = true;
            moveDirection *= speed;
        }
        else
        {
            playerManager.isSprinting = false;
            moveDirection *= speed;
        }
        
        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalvector);
        rigi.velocity = projectedVelocity;

        if(inputHandler.lockOnFlag && inputHandler.sprintFlag == false)
        {
            animatorHandler.UpdateAnimatorValues(inputHandler.vertical, inputHandler.horizontal, playerManager.isSprinting);
        }
        else
        {
            animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);
        }

        if (animatorHandler.canRotate)
        {
            HandleRotation(delta);
        }
    }

    public void HandleRollingAndSprinting(float delta)
    {
        if (animatorHandler.anim.GetBool(DarkSoulsConsts.ISINTERACTING)) return;

        if (inputHandler.rollFlag)
        {
            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;

            if (inputHandler.moveAmount > 0)
            {
                animatorHandler.PlayTargetAnimation(DarkSoulsConsts.ROLLING, true);
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = rollRotation;
            }
            else
            {
                animatorHandler.PlayTargetAnimation(DarkSoulsConsts.STEPBACK, true);
            }
        }
    }

    public void HandleFalling(float delta, Vector3 moveDirection)
    {
        playerManager.isGrounded = false;
        RaycastHit hit;
        Vector3 origin = myTransform.position;
        origin.y += groundDetectionRayStartPoint;

        if(Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
        {
            moveDirection = Vector3.zero;
        }

        if(playerManager.isInAir)
        {
            rigi.AddForce(-Vector3.up * fallingSpeed);
            rigi.AddForce(moveDirection * fallingSpeed / 10f);
        }

        Vector3 dir = moveDirection;
        dir.Normalize();
        origin = origin + dir * groundDirectionRayDistance;

        targetPosition = myTransform.position;
        Debug.DrawRay(origin, -Vector3.up * minimumdistanceToBeginFall, Color.red, 0.1f, false);
        if(Physics.Raycast(origin, -Vector3.up, out hit, minimumdistanceToBeginFall, ignoreForGroundCheck))
        {
            normalvector = hit.normal;
            Vector3 tp = hit.point;
            playerManager.isGrounded = true;
            targetPosition.y = tp.y;

            if(playerManager.isInAir)
            {
                if(inAirTimer > 0.5f)
                {
                    Debug.Log("You were in the air for " + inAirTimer);
                    animatorHandler.PlayTargetAnimation(DarkSoulsConsts.LAND, true);
                    inAirTimer = 0f;
                }
                else
                {
                    animatorHandler.PlayTargetAnimation(DarkSoulsConsts.EMPTY, false);
                    inAirTimer = 0f;
                }
                playerManager.isInAir = false;
            }
        }
        else
        {
            if(playerManager.isGrounded)
            {
                playerManager.isGrounded = false;
            }
            if(playerManager.isInAir == false)
            {
                if(playerManager.isInteracting == false)
                {
                    animatorHandler.PlayTargetAnimation(DarkSoulsConsts.FALLING, true);
                }

                Vector3 vel = rigi.velocity;
                vel.Normalize();
                rigi.velocity = vel * (movementSpeed / 2);
                playerManager.isInAir = true;
            }
        }

        //Makes sure that your model is going to the target position
        if(playerManager.isInteracting || inputHandler.moveAmount > 0)
        {
            myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime / 0.1f);
        }
        else
        {
            myTransform.position = targetPosition;
        }
    }

    public void HandleJumping()
    {
        if (playerManager.isInteracting) return;
        if(inputHandler.jump_Input)
        {
            if(inputHandler.moveAmount > 0)
            {
                moveDirection = cameraObject.forward * inputHandler.vertical;
                moveDirection += cameraObject.right * inputHandler.horizontal;
                animatorHandler.PlayTargetAnimation(DarkSoulsConsts.JUMP, true);
                moveDirection.y = 0;
                Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = jumpRotation;
            }
        }
    }
    #endregion
}
