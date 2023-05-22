using UnityEngine;

public class IdleState : State
{
    [SerializeField] Animator anim;
    [SerializeField] PursueTargetState pursueTargetState;

    [Header("Serializables")]
    public LayerMask detectionLayer;

    public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        if (enemyStats.currentHealth <= 0) return null;

        #region Handle Enemy Target Detection
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStatsManager characterStats = colliders[i].transform.GetComponent<CharacterStatsManager>();

            if (characterStats != null)
            {
                if (characterStats.teamIDNumber != enemyStats.teamIDNumber)
                {
                    Vector3 targetDirection = characterStats.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                    {
                        enemyManager.currentTarget = characterStats;
                    }
                }
            }
        }
        #endregion

        #region Handle Switching To Next State
        if (enemyManager.currentTarget != null)
        {
            return pursueTargetState;
        }
        else
        {
            return this;
        }
        #endregion
    }
}
