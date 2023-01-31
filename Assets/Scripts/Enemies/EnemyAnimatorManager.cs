using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : AnimatorManager
{
    EnemyManager enemyManager;
    EnemyStats enemyStats;
    private void Awake()
    {
        enemyStats = GetComponentInParent<EnemyStats>();
        enemyManager = GetComponentInParent<EnemyManager>();
    }
    public override void TakeCriticalDamageAnimationEvent()
    {
        enemyStats.TakeDamage(enemyManager.pendingCriticalDamage, false);
        enemyManager.pendingCriticalDamage = 0;
    }
    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        enemyManager.enemyRigidbody.drag = 0;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        enemyManager.enemyRigidbody.velocity = velocity;
    }

    public void AwardSoulsOnDeath()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        SoulCounter soulCounter = FindObjectOfType<SoulCounter>();
        if (playerStats != null)
        {
            playerStats.AddSouls(enemyStats.soulsAwardedOnDeath);

            if(soulCounter != null)
            {
                soulCounter.SetSoulCountText(enemyStats.soulsAwardedOnDeath);
            }
        }
    }

    public void EnableIsParrying()
    {
        enemyManager.isParrying = true;
    }

    public void DisableIsParrying()
    {
        enemyManager.isParrying = false;
    }

    public void EnableCanBeRiposted()
    {
        enemyManager.canBeRiposted = true;
    }
    public void DisableCanBeRiposted()
    {
        enemyManager.canBeRiposted = false;
    }
}
