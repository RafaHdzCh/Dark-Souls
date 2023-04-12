using UnityEngine;

public class EnemyAnimatorManager : AnimatorManager
{
    EnemyManager enemyManager;
    EnemyStatsManager enemyStats;

    protected override void Awake()
    {
        base.Awake();
        enemyStats = GetComponentInParent<EnemyStatsManager>();
        enemyManager = GetComponentInParent<EnemyManager>();
    }

    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        enemyManager.enemyRigidbody.drag = 0;
        Vector3 deltaPosition = animator.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        enemyManager.enemyRigidbody.velocity = velocity;

        if(enemyManager.isRotatingWithRootMotion)
        {
            enemyManager.transform.rotation *= animator.deltaRotation;
        }
    }

    public void AwardSoulsOnDeath()
    {
        PlayerStatsManager playerStats = FindObjectOfType<PlayerStatsManager>();
        SoulCounter soulCounter = FindObjectOfType<SoulCounter>();
        if (playerStats != null)
        {
            playerStats.AddSouls(characterStatsManager.soulsAwardedOnDeath);

            if(soulCounter != null)
            {
                soulCounter.SetSoulCountText(characterStatsManager.soulsAwardedOnDeath);
            }
        }
    }

    public void InstantiateBossParticleFX()
    {
        BossFXTransform bossFXTransform = GetComponentInChildren<BossFXTransform>();

        GameObject phaseFX = bossFXTransform.transform.GetChild(0).gameObject;
        phaseFX.SetActive(true);
    }
}
