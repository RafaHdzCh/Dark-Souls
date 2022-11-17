using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : CharacterManager
{
    [HideInInspector] public bool isPerformingAction;
    [HideInInspector] public CharacterStats currentTarget;
    [HideInInspector] public Rigidbody enemyRigidbody;

    
    [Header("Variables")]
    [HideInInspector] public float distanceFromTarget;
    [HideInInspector] public float rotationSpeed = 30f;
    [HideInInspector] public float maximumAttackRange = 1.5f;
    [HideInInspector] public float currentRecoveryTime = 0;

    [Header("Serializables")]
    [Header("Detection")]
    public float detectionRadius = 20f;
    public float minimumDetectionAngle = -50f;
    public float maximumDetectionAngle = 50f;
    public State currentState;

    [Header("Detection")]
    EnemyLocomotionManager enemyLocomotionManager;
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyStats enemyStats;
    [HideInInspector] public NavMeshAgent navMeshAgent;

    private void Awake()
    {
        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        enemyStats = GetComponent<EnemyStats>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        enemyRigidbody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        navMeshAgent.enabled = false;
        enemyRigidbody.isKinematic = false;
    }

    private void Update()
    {
        HandleRecoveryTimer();
    }

    private void FixedUpdate()
    {
        HandleStateMachine();
    }

    private void HandleStateMachine()
    {
        if(currentState != null)
        {
            State nextState = currentState.Tick(this, enemyStats, enemyAnimatorManager);
            if(nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }
        /*
        if(enemyLocomotionManager.currentTarget != null)
        {
            enemyLocomotionManager.distanceFromTarget = Vector3.Distance(enemyLocomotionManager.currentTarget.transform.position, transform.position);
        }
        if(enemyLocomotionManager.currentTarget == null)
        {
            enemyLocomotionManager.HandleDetection();
        }
        else if(enemyLocomotionManager.distanceFromTarget > enemyLocomotionManager.stoppingDistance)
        {
            enemyLocomotionManager.HandleMoveToTarget();
        }
        else if(enemyLocomotionManager.distanceFromTarget <= enemyLocomotionManager.stoppingDistance)
        {
            AttackTarget();
        }
        */
    }

    private void SwitchToNextState(State state)
    {
        currentState = state;
    }

    private void HandleRecoveryTimer()
    {
        if(currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }

        if(isPerformingAction)
        {
            if(currentRecoveryTime <= 0)
            {
                isPerformingAction = false;
            }
        }
    }
    
}
