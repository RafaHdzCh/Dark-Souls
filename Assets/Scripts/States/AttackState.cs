using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    [SerializeField] CombatStanceState combatStanceState;
    [SerializeField] PursueTargetState pursueTargetState;
    [SerializeField] RotateTowardsTargetState rotateTowardsTargetState;
     
    public EnemyAttackAction[] enemyAttacks;

    [System.NonSerialized] public EnemyAttackAction currentAttack;

    [System.NonSerialized] public bool hasPerformedAttack = false;
    [System.NonSerialized] public bool willDoComboOnNextAttack = false;

    public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        RotateTowardsTargetWhilstAttacking(enemyManager);
        
        if(distanceFromTarget > enemyManager.maximumAggroRadius) 
        {
            return pursueTargetState;
        }

        if(willDoComboOnNextAttack && enemyManager.canDoCombo)
        {
            AttackTargetWithCombo(enemyAnimatorManager, enemyManager);
        }

        if(!hasPerformedAttack)
        {
            AttackTarget(enemyAnimatorManager, enemyManager);
            RollForComboChance(enemyManager);
        }

        if(willDoComboOnNextAttack && hasPerformedAttack)
        {
            return this;
        }

        return rotateTowardsTargetState;
    }
    private void AttackTarget(EnemyAnimatorManager enemyAnimatorManager, EnemyManager enemyManager)
    {
        if(currentAttack.actionAnimation != null)
        {
            enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
        }
        enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
        hasPerformedAttack = true;
        if(currentAttack.actionAnimation == null)
        {
            return;
        }
    }

    private void AttackTargetWithCombo(EnemyAnimatorManager enemyAnimatorManager, EnemyManager enemyManager)
    {
        willDoComboOnNextAttack = false;
        enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
        enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
        currentAttack = null;
    }

    private void RotateTowardsTargetWhilstAttacking(EnemyManager enemyManager)
    {
        //Rotate manually
        if (enemyManager.canRotate && enemyManager.isInteracting)
        {
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
    }

    private void RollForComboChance(EnemyManager enemyManager)
    {
        float comboChance = Random.Range(0, 100);

        if(comboChance > 50) 
        { 
            if(currentAttack.comboAction != null)
            {
                willDoComboOnNextAttack = true;
                currentAttack = currentAttack.comboAction;
            }
            else
            {
                willDoComboOnNextAttack = false;
                currentAttack = null;
            }
        }
    }
}
