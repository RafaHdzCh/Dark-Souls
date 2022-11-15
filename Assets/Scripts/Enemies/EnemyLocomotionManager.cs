using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLocomotionManager : MonoBehaviour
{
    [Header("Variables")]
    [HideInInspector] public float distanceFromTarget;
    public float stoppingDistance = 1f;
    [HideInInspector] public float rotationSpeed = 15f;

    [Header("Scripts")]
    [HideInInspector] public CharacterStats currentTarget;
    [HideInInspector] public Rigidbody enemyRigidbody;
    EnemyManager enemyManager;
    EnemyAnimatorManager enemyAnimatorManager;
    NavMeshAgent navMeshAgent;

    [Header("Serializables")]
    public LayerMask detectionLayer;
    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>(); 
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        enemyRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        navMeshAgent.enabled = false;
        enemyRigidbody.isKinematic = false;
    }

    //Busca colliders en una capa especifica. Si los encuentra y son del equipo contrario,
    // y estan en su campo de vista, obtiene su CharacterStats y lo vuelve su objetivo.
    public void HandleDetection()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position,enemyManager.detectionRadius, detectionLayer);
    
        for(int i = 0; i < colliders.Length; i++)
        {
            CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

            if(characterStats != null)
            {
                //Check for TEAM ID
                Vector3 targetDirection = characterStats.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if(viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                {
                    currentTarget = characterStats;
                }
            }
        }
    }
    public void HandleMoveToTarget()
    {
        Vector3 targetDirection = currentTarget.transform.position - transform.position;
        distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
        float viewableAngle = Vector3.Angle(targetDirection, transform.position);
        
        //Si esta realizando una accion, detiene el movimiento.
        if(enemyManager.isPerformingAction)
        {
            enemyAnimatorManager.anim.SetFloat(DarkSoulsConsts.VERTICAL, 0, 0.1f, Time.deltaTime);
            navMeshAgent.enabled = false;
        }
        else
        {
            if(distanceFromTarget > stoppingDistance)
            {
                enemyAnimatorManager.anim.SetFloat(DarkSoulsConsts.VERTICAL, 1, 0.1f, Time.deltaTime);
            }
            else if(distanceFromTarget <= stoppingDistance)
            {
                enemyAnimatorManager.anim.SetFloat(DarkSoulsConsts.VERTICAL, 0, 0.1f, Time.deltaTime);
            }
        }

        navMeshAgent.transform.localPosition = Vector3.zero;
        navMeshAgent.transform.localRotation = Quaternion.identity;
    }

    private void HandleRotateTowardsTarget()
    {
        //Rotate manually
        if(enemyManager.isPerformingAction)
        {
            Vector3 direction = currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if(direction == Vector3.zero)
            {
                direction = transform.forward;
            }
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            // *
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        //Rotate with pathfinding (navmesh)
        else
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(navMeshAgent.desiredVelocity);
            Vector3 targetVelocity = enemyRigidbody.velocity;

            navMeshAgent.enabled = true;
            navMeshAgent.SetDestination(currentTarget.transform.position);
            enemyRigidbody.velocity = targetVelocity;
            // *
            transform.rotation = Quaternion.Slerp(transform.rotation, navMeshAgent.transform.rotation, rotationSpeed * Time.deltaTime);
        }

        HandleRotateTowardsTarget();
        navMeshAgent.transform.localPosition = Vector3.zero;
        navMeshAgent.transform.localRotation = Quaternion.identity;
    }
}
