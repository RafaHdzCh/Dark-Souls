using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : CharacterManager
{
    [System.NonSerialized] public bool isPerformingAction;
    [System.NonSerialized] public bool isInteracting;
        public CharacterStats currentTarget;
    [System.NonSerialized] public Rigidbody enemyRigidbody;
    
    [Header("Variables")]
    [System.NonSerialized] public float rotationSpeed = 50f;
    [System.NonSerialized] public float maximumAttackRange = 1.5f;
    [System.NonSerialized] public float currentRecoveryTime = 0;

    [Header("Combat Flags")]

    public bool canDoCombo;

    [Header("Serializables")]
    [Header("Detection")]
    public float detectionRadius = 40f;
    public float minimumDetectionAngle = -50f;
    public float maximumDetectionAngle = 50f;
    public State currentState;

    [Header("Detection")]
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyStats enemyStats;
    [System.NonSerialized] public NavMeshAgent navMeshAgent;

    private void Awake()
    {
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        enemyStats = GetComponent<EnemyStats>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        enemyRigidbody = GetComponent<Rigidbody>();
        navMeshAgent.enabled = false;
    }
    private void Start()
    {
        enemyRigidbody.isKinematic = false;
    }

    private void Update()
    {
        HandleRecoveryTimer();
        HandleStateMachine();

        isRotatingWithRootMotion = enemyAnimatorManager.anim.GetBool(DarkSoulsConsts.ISROTATINGWITHROOTMOTION);
        isInteracting = enemyAnimatorManager.anim.GetBool(DarkSoulsConsts.ISINTERACTING);
        canDoCombo = enemyAnimatorManager.anim.GetBool(DarkSoulsConsts.CANDOCOMBO);
        enemyAnimatorManager.anim.SetBool(DarkSoulsConsts.ISDEAD, enemyStats.isDead);
    }

    private void LateUpdate()
    {
        navMeshAgent.transform.localPosition = Vector3.zero;
        navMeshAgent.transform.localRotation = Quaternion.identity;
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
