using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    Transform cameraObject;
    InputHandler inputHandler;
    Vector3 moveDirection;

    [HideInInspector]
    public Transform myTransform;

    [HideInInspector]
    public AnimatorHandler animatorHandler;
    public Rigidbody rigidbody;
    public GameObject normalCamera;

    [Header("Stats")]
    [SerializeField] float movementSpeed = 5;
    [SerializeField] float rotationSpeed = 10;
    

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        cameraObject = Camera.main.transform;
        myTransform = transform;
        animatorHandler.Initialize();

    }

    public void Update()
    {
        float delta = Time.deltaTime;

        inputHandler.TickInput(delta);
        HandleMovement(delta);
        HandleRollingAndSprinting(delta);
    }

    #region Movement
    Vector3 normalvector;
    Vector3 targetPosition;

    private void HandleRotation(float delta)
    {
        Vector3 targetDir = Vector3.zero;
        float moveOverride = inputHandler.moveAmount;

        targetDir = cameraObject.forward * inputHandler.vertical;
        targetDir += cameraObject.right * inputHandler.horizontal;
        targetDir.Normalize();
        targetDir.y = 0;
        if(targetDir == Vector3.zero)
            targetDir = myTransform.forward;

        float rs = rotationSpeed;
        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

        myTransform.rotation = targetRotation;
    }

    public void HandleMovement(float delta)
    {
        moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        float speed = movementSpeed;
        moveDirection *= speed;

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalvector);
        rigidbody.velocity = projectedVelocity;

        animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0);

        if (animatorHandler.canRotate)
        {
            HandleRotation(delta);
        }
    }

    public void HandleRollingAndSprinting(float delta)
    {
        if (animatorHandler.anim.GetBool(DarkSoulsConsts.ISINTERACTING))
        {
            print("return");
            return;
        }
        if (inputHandler.rollFlag)
        {
            print("rodar");
            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;

            if(inputHandler.moveAmount > 0)
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
    #endregion
}
