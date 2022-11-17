using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStanceState : State
{
    [SerializeField] AttackState attackState;
    [SerializeField] PursueTargetState pursueTargetState;

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

        if (enemyManager.currentRecoveryTime <= 0 && enemyManager.distanceFromTarget <= enemyManager.maximumAttackRange)
        {
            return attackState;
        }
        else if(enemyManager.distanceFromTarget > enemyManager.maximumAttackRange)
        {
            return pursueTargetState;
        }
        else
        {
            return this;
        }
    }
}
