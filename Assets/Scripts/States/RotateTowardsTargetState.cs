using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsTargetState : State
{
    [SerializeField] CombatStanceState combatStanceState;

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        enemyAnimatorManager.anim.SetFloat(DarkSoulsConsts.VERTICAL, 0);
        enemyAnimatorManager.anim.SetFloat(DarkSoulsConsts.HORIZONTAL, 0);

        Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
        float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.transform.forward, Vector3.up);

        if (enemyManager.isInteracting) return this;

        if(viewableAngle >= 100 && viewableAngle <= 180 && !enemyManager.isInteracting)
        {
            enemyAnimatorManager.PlayTargetAnimationWithRootRotation(DarkSoulsConsts.TURNBEHIND, true);
            return combatStanceState;
        }
        else if(viewableAngle <= -101 & viewableAngle >= -180 && !enemyManager.isInteracting)
        {
            enemyAnimatorManager.PlayTargetAnimationWithRootRotation(DarkSoulsConsts.TURNBEHIND, true);
            return combatStanceState;
        }
        else if(viewableAngle <= -45 && viewableAngle >= -100 && !enemyManager.isInteracting)
        {
            enemyAnimatorManager.PlayTargetAnimationWithRootRotation(DarkSoulsConsts.TURNRIGHT, true);
            return combatStanceState;
        }
        else if(viewableAngle >= 45 && viewableAngle <= 100 && !enemyManager.isInteracting)
        {
            enemyAnimatorManager.PlayTargetAnimationWithRootRotation(DarkSoulsConsts.TURNLEFT, true);
            return combatStanceState;
        }
        return combatStanceState;
    }
}
