using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmbushState : State
{
    private bool isSleeping = true;
    private float detctionRadius = 2;

    [SerializeField] PursueTargetState pursurTargetState;
    [SerializeField] LayerMask detectionLayer;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        if (enemyStats.currentHealth <= 0) return null;
        if (isSleeping && enemyManager.isInteracting == false)
        {
            enemyAnimatorManager.PlayTargetAnimation(DarkSoulsConsts.SLEEP, true);
        }

        #region Handle Target Detection

        Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, detctionRadius, detectionLayer);
        for(int i = 0; i < colliders.Length; i++)
        {
            CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

            if(characterStats != null)
            {
                Vector3 targetDirection = characterStats.transform.position - enemyManager.transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

                if(viewableAngle >= enemyManager.minimumDetectionAngle &&
                    viewableAngle < enemyManager.maximumDetectionAngle)
                {
                    enemyManager.currentTarget = characterStats;
                    isSleeping = false;
                    enemyAnimatorManager.PlayTargetAnimation(DarkSoulsConsts.WAKE, true);
                }
            }
        }

        #endregion

        #region Handle State Change

        if(enemyManager.currentTarget != null)
        {
            return pursurTargetState;
        }
        else 
        {
            return this;
        }

        #endregion
    }
}