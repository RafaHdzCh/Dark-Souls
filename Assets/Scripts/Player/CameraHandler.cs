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

    //La camara sigue al pivote.
    public void FollowTarget(float delta)
    {
        Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position,
        targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
        myTransform.position = targetPosition;
        HandleCameraCollision(delta);
    }

    //Rotacion de la camara segun si se esta haciendo target a un objetivo o no.
    public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
    {
        //Si no hay objetivo targeteado
        if(inputHandler.lockOnFlag == false && currentLockOnTarget == null)
        {
            //Movimiento de la camara con el mouse.
            lookAngle += (mouseXInput * lookSpeed) * delta;
            pivotAngle -= (mouseYInput * pivotSpeed) * delta;
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
            //Distancia entre la posicion del enemigo targeteado y la posicion del CameraHandler.
            Vector3 dir = currentLockOnTarget.transform.position - transform.position;
            dir.Normalize();
            dir.y = 0;

            //Rota la camara hacia la posicion del enemigo targeteado.
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = targetRotation;

            //La direccion es la distancia normalizada entre la posicion del enemigo y el pivote de la camara.
            dir = currentLockOnTarget.transform.position - cameraPivotTransform.position;
            dir.Normalize();

            targetRotation = Quaternion.LookRotation(dir);
            Vector3 eulerAngle = targetRotation.eulerAngles;
            eulerAngle.y = 0;
            cameraPivotTransform.localEulerAngles = eulerAngle;
        }
    }

    //Una esfera que provoca que la camara colisione con colliders para que no entre en paredes u objetos.
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

        //Tomamos los colliders en un radio de 26 unidades.
        Collider[] colliders = Physics.OverlapSphere(transform.position, 26);

        for(int i= 0; i < colliders.Length; i++)
        {
            CharacterManager character = colliders[i].GetComponent<CharacterManager>();

            //Si es un personaje...
            if(character != null)
            {
                //Se obtienen las distancias entre el personaje y el enemigo, y el angulo de vision.
                Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
                float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
                float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);
                RaycastHit hit;

                //Evita que te hagas lock a ti mismo.
                if(character.transform.root != targetTransform.transform.root && 
                   viewableAngle > -50 && viewableAngle < 50 && distanceFromTarget <= maximumLockOnDistance)
                {
                    //Lanza una linea desde la posicion del jugador hacia el enemigo.
                    if(Physics.Linecast(playerManager.lockOnTransform.position, character.lockOnTransform.position, out hit))
                    {
                        Debug.DrawLine(playerManager.lockOnTransform.position, character.lockOnTransform.position);
                        
                        //Si la linea choca con un objeto en la capa "Environment", no se puede hacer lockon.
                        if (hit.transform.gameObject.layer == environmentLayer)
                        {
                            inputHandler.lockOnFlag = false;
                        }
                        //Si no choca con el entorno, agregamos al enemigo a la lista de objetivos disponibles.
                        else 
                        {
                            availableTargets.Add(character);
                        }
                    }
                }
            }
        }

        //Se obtienen los enemigos mas cercanos a la derecha y a la izquierda del objetivo 
        for(int k= 0; k < availableTargets.Count; k++)
        {
            //Distancia entre el personaje y cada elemento de la lista.
            float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTargets[k].transform.position);
            
            //Si la distancia entre el enemigo y nosotros es menor que la distancia mas cercana.
            if(distanceFromTarget < shortestDistance)
            {
                //Ese es el enemigo mas cercano y el nuevo objetivo.
                shortestDistance = distanceFromTarget;
                nearestLockOnTarget = availableTargets[k];
            }

            //Si estamos lockeando a un objetivo...
            if(inputHandler.lockOnFlag)
            {
                //Calculamos la posicion la relativa entre el siguiente objetivo en la lista y la camara.
                Vector3 relativeEnemyPosition = inputHandler.transform.InverseTransformPoint(availableTargets[k].transform.position);
                var distanceFromLeftTarget = relativeEnemyPosition.x;
                var distanceFromRightTarget = relativeEnemyPosition.x;

                //Si la posicion relativa del siguiente enemigo en la lista en le eje X es menor que 0 y no lo estoy targeteando...
                if (relativeEnemyPosition.x <= 0 && distanceFromLeftTarget > shortestDistanceOfLeftTarget && availableTargets[k] != currentLockOnTarget)
                {
                    //Ese enemigo se convierte en el mas cercano a la izquierda
                    shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                    leftLockTarget = availableTargets[k];
                }
                //Si la posicion relativa del siguiente enemigo en la lista en le eje X es mayor que 0 y no lo estoy targeteando...
                else if (relativeEnemyPosition.x >= 0 && distanceFromRightTarget < shortestDistanceOfRightTarget && availableTargets[k] != currentLockOnTarget)
                {
                    //Ese enemigo se convierte en el mas cercano a la derecha.
                    shortestDistanceOfRightTarget = distanceFromRightTarget;
                    rightLockTarget = availableTargets[k];
                }
            }    
        }
    }

    //Limpia la lista de objetivos disponibles y el enemigo actual.
    public void ClearLockOnTargets()
    {
        availableTargets.Clear();
        nearestLockOnTarget = null;
        currentLockOnTarget = null;
    }

    //Establece la altura de la camara segun si estamos targeteando a un enemigo o no.
    public void SetCameraHeight()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 newLockedPosition = new Vector3(0, lockedPivotPosition);
        Vector3 newUnlockedPosition = new Vector3(0, unlockedPivotPosition);

        //Si estamos fijando a un objetivo, la camara se desplaza a una posicion mas elevada.
        if(currentLockOnTarget != null)
        {
            cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockedPosition, ref velocity, Time.deltaTime);
        }
        //Si no, la camara se mantiene en su altura normal.
        else
        {
            cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnlockedPosition, ref velocity, Time.deltaTime);
        }
    }
}
