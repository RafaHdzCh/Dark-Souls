using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.TextCore.Text;

public class CameraHandler : MonoBehaviour
{
    public static CameraHandler singleton;

    [Header("Serializables")]
    public Transform cameraTransform;
    public Transform cameraPivotTransform;
    public LayerMask ignoreLayers;
    public LayerMask environmentLayer;

    private Vector3 cameraTransformPosition;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Transform myTransform;

    public float lookSpeed = 0.005f;
    float followSpeed = 0.1f;
    public float pivotSpeed = 0.005f;

    float targetPosition;
    float defaultPosition;
    float lookAngle;
    float pivotAngle;
    float minimumPivot = -35f;
    float maximumPivot = 35f;

    float cameraSphereRadius = 0.2f;
    float cameraCollisionOffset = 0.2f;
    float minimumCollisionOffset = 0.2f;
    float lockedPivotPosition = 2.25f;
    float unlockedPivotPosition = 1.65f;

    float maximumLockOnDistance = 30f;

    List<CharacterManager> availableTargets = new List<CharacterManager>();
    [HideInInspector] public CharacterManager nearestLockOnTarget;
    [HideInInspector] public CharacterManager leftLockTarget;
    [HideInInspector] public CharacterManager rightLockTarget;
    [HideInInspector] public CharacterManager currentLockOnTarget;
    [HideInInspector] public Transform targetTransform;

    InputHandler inputHandler;
    PlayerManager playerManager;

    private void Awake()
    {
        singleton = this;
        myTransform = transform;
        defaultPosition = cameraTransform.localPosition.z;
        ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        inputHandler = FindObjectOfType<InputHandler>();
        playerManager = FindObjectOfType<PlayerManager>();
    }

    private void Start()
    {
        environmentLayer = LayerMask.NameToLayer(DarkSoulsConsts.ENVIRONMENT);
    }

    public void FollowTarget(float delta)
    {
        Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position,
        targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
        myTransform.position = targetPosition;
        HandleCameraCollision(delta);
    }

    public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
    {
        if(inputHandler.lockOnFlag == false && currentLockOnTarget == null)
        {
            lookAngle += (mouseXInput * lookSpeed) / delta;
            pivotAngle -= (mouseYInput * pivotSpeed) / delta;
            pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

            Vector3 rotation = Vector3.zero;
            rotation.y = lookAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            myTransform.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = pivotAngle;

            targetRotation = Quaternion.Euler(rotation);
            cameraPivotTransform.localRotation = targetRotation;
        }
        else
        {
            float velocity = 0;
            Vector3 dir = currentLockOnTarget.transform.position - transform.position;
            dir.Normalize();
            dir.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = targetRotation;

            dir = currentLockOnTarget.transform.position - cameraPivotTransform.position;
            dir.Normalize();

            targetRotation = Quaternion.LookRotation(dir);
            Vector3 eulerAngle = targetRotation.eulerAngles;
            eulerAngle.y = 0;
            cameraPivotTransform.localEulerAngles = eulerAngle;
        }
    }

    //Creates a Sphere that makes the camera collide with certain layers in the scene.
    private void HandleCameraCollision(float delta)
    {
        targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition), ignoreLayers))
        {
            float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetPosition = -(dis - cameraCollisionOffset);
        }

        if(Mathf.Abs(targetPosition) < minimumCollisionOffset)
        {
            targetPosition = -minimumCollisionOffset;
        }

        cameraTransformPosition.z = Mathf.MoveTowards(cameraTransform.localPosition.z, targetPosition, 180f);
        cameraTransform.localPosition = cameraTransformPosition;
    }

    public void HandleLockOn()
    {
        float shortestDistance = Mathf.Infinity;
        float shortestDistanceOfLeftTarget = -Mathf.Infinity;
        float shortestDistanceOfRightTarget = Mathf.Infinity;

        Collider[] colliders = Physics.OverlapSphere(transform.position, 26);

        for(int i= 0; i < colliders.Length; i++)
        {
            CharacterManager character = colliders[i].GetComponent<CharacterManager>();

            if(character != null)
            {
                Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
                float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
                float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);
                RaycastHit hit;
                //Avoid targeting ourselves or far away enemies.
                if(character.transform.root != targetTransform.transform.root && 
                   viewableAngle > -50 && viewableAngle < 50 && distanceFromTarget <= maximumLockOnDistance)
                {
                    if(Physics.Linecast(playerManager.lockOnTransform.position, character.lockOnTransform.position, out hit))
                    {
                        Debug.DrawLine(playerManager.lockOnTransform.position, character.lockOnTransform.position);
                        if(hit.transform.gameObject.layer == environmentLayer)
                        {
                            inputHandler.lockOnFlag = false;
                        }
                        else 
                        {
                            availableTargets.Add(character);
                        }
                    }
                }
            }
        }

        for(int k= 0; k < availableTargets.Count; k++)
        {
            float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTargets[k].transform.position);
            if(distanceFromTarget < shortestDistance)
            {
                shortestDistance = distanceFromTarget;
                nearestLockOnTarget = availableTargets[k];
            }
            if(inputHandler.lockOnFlag)
            {
                //Vector3 relativeEnemyPosition = currentLockOnTarget.transform.InverseTransformPoint(availableTargets[k].transform.position);
                Vector3 relativeEnemyPosition = inputHandler.transform.InverseTransformPoint(availableTargets[k].transform.position);
                var distanceFromLeftTarget = relativeEnemyPosition.x;
                var distanceFromRightTarget = relativeEnemyPosition.x;

                if (relativeEnemyPosition.x <= 0 && distanceFromLeftTarget > shortestDistanceOfLeftTarget && availableTargets[k] != currentLockOnTarget)
                {
                    shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                    leftLockTarget = availableTargets[k];
                }
                else if(relativeEnemyPosition.x >= 0 && distanceFromRightTarget < shortestDistanceOfRightTarget && availableTargets[k] != currentLockOnTarget)
                {
                    shortestDistanceOfRightTarget = distanceFromRightTarget;
                    rightLockTarget = availableTargets[k];
                }
            }    
        }
    }

    public void ClearLockOnTargets()
    {
        availableTargets.Clear();
        nearestLockOnTarget = null;
        currentLockOnTarget = null;
    }

    public void SetCameraHeight()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 newLockedPosition = new Vector3(0, lockedPivotPosition);
        Vector3 newUnlockedPosition = new Vector3(0, unlockedPivotPosition);

        if(currentLockOnTarget != null)
        {
            cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockedPosition, ref velocity, Time.deltaTime);
        }
        else
        {
            cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnlockedPosition, ref velocity, Time.deltaTime);
        }
    }
}
