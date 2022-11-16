using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : CharacterManager
{
    [HideInInspector] public bool isPerformingAction;

    [Header("Serializables")]
    [Header("Detection")]
    public float detectionRadius = 20f;
    public float minimumDetectionAngle = -50f;
    public float maximumDetectionAngle = 50f;

    [Header("Detection")]
    EnemyLocomotionManager enemyLocomotionManager;

    private void Start()
    {
        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        HandleCurrentAction();
    }

    private void HandleCurrentAction()
    {
        if(enemyLocomotionManager.currentTarget == null)
        {
            enemyLocomotionManager.HandleDetection();
        }
        else 
        {
            enemyLocomotionManager.HandleMoveToTarget();
        }
    }
}
