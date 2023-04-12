using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStanceState : State
{
    [SerializeField] public AttackState attackState; 
    [SerializeField] PursueTargetState pursueTargetState;

    public EnemyAttackAction[] enemyAttacks;

    protected bool randomDestinationSet = false;
    protected float verticalMovementValue = 0;
    protected float horizontalMovementValue = 0;

    public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        if (enemyStats.currentHealth <= 0) return null;

        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        enemyAnimatorManager.animator.SetFloat(DarkSoulsConsts.VERTICAL, verticalMovementValue, 0.2f, Time.deltaTime);
        enemyAnimatorManager.animator.SetFloat(DarkSoulsConsts.HORIZONTAL, horizontalMovementValue, 0.2f, Time.deltaTime);
        attackState.hasPerformedAttack = false;

        if (enemyManager.isInteracting)
        {
            enemyAnimatorManager.animator.SetFloat(DarkSoulsConsts.VERTICAL, 0);
            enemyAnimatorManager.animator.SetFloat(DarkSoulsConsts.HORIZONTAL, 0);
            return this;
        }

        if (distanceFromTarget > enemyManager.maximumAggroRadius)
        {
            return pursueTargetState;
        }
        if(!randomDestinationSet)
        {
            randomDestinationSet = true;
            DecideCirclingAction(enemyAnimatorManager, enemyStats);
        }

        HandleRotateTowardsTarget(enemyManager);

        if (enemyManager.currentRecoveryTime <= 0 && attackState.currentAttack != null)
        {
            randomDestinationSet = false;
            return attackState;
        }
        else
        {
            GetNewAttack(enemyManager);
        }
        return this;
    }
    protected private void HandleRotateTowardsTarget(EnemyManager enemyManager)
    {
        //Rotate manually
        if (enemyManager.isPerformingAction)
        {
            print("COMBAT: Manual rotation");
            Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
        }
        //Rotate with pathfinding (navmesh)
        else
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
            Vector3 targetVelocity = enemyManager.navMeshAgent.desiredVelocity;

            enemyManager.navMeshAgent.enabled = true;
            enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
            enemyManager.enemyRigidbody.velocity = targetVelocity;
            enemyManager.navMeshAgent.angularSpeed = 180;
            enemyManager.transform.rotation = Quaternion.Slerp
            (transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
        }
    }

    protected private void DecideCirclingAction(EnemyAnimatorManager enemyAnimatorManager, EnemyStatsManager enemyStats)
    {
        WalkAroundTarget(enemyAnimatorManager, enemyStats);
    }

    protected private void WalkAroundTarget(EnemyAnimatorManager enemyAnimatorManager, EnemyStatsManager enemyStats)
    {
        verticalMovementValue = Random.Range(0, 1);
        if (verticalMovementValue <= 1 && verticalMovementValue >= 0.51f)
        {
            verticalMovementValue = enemyStats.runSpeed;
        }
        else if (verticalMovementValue >= 0 && verticalMovementValue < 0.50)
        {
            verticalMovementValue = enemyStats.walkSpeed;
        }

        horizontalMovementValue = Random.Range(0, 1);
        if(horizontalMovementValue <= 1 && horizontalMovementValue >= 0.51f)
        {
            horizontalMovementValue = enemyStats.runSpeed;
        }
        else if (horizontalMovementValue >= 0.5f && horizontalMovementValue < 0)
        {
            horizontalMovementValue = enemyStats.walkSpeed;
        }
    }

    protected virtual private void GetNewAttack(EnemyManager enemyManager)
    {
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);

        int maxScore = 0;
        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            if (distanceFromTarget <= enemyAttackAction.maximumAttackDistanceToAttack
                && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            {
                if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                    && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                {
                    maxScore += enemyAttackAction.attackScore;
                }
            }
        }

        int randomValue = Random.Range(0, maxScore + 1);
        int temporaryScore = 0;

        for (int j = 0; j < enemyAttacks.Length; j++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[j];

            if (distanceFromTarget <= enemyAttackAction.maximumAttackDistanceToAttack
                && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            {
                if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                    && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                {
                    if (attackState.currentAttack != null) return;

                    temporaryScore += enemyAttackAction.attackScore;

                    if (temporaryScore > randomValue)
                    {
                        attackState.currentAttack = enemyAttackAction;
                    }
                }
            }
        }
    }
}
