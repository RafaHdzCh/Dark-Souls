using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : AnimatorManager
{
    EnemyManager enemyManager;
    EnemyStats enemyStats;
    EnemyBossManager enemyBossManager;


    private void Awake()
    {
        enemyStats = GetComponentInParent<EnemyStats>();
        enemyManager = GetComponentInParent<EnemyManager>();
        enemyBossManager = GetComponentInParent<EnemyBossManager>();
    }
    public override void TakeCriticalDamageAnimationEvent()
    {
        enemyStats.TakeDamageNoAnimation(enemyManager.pendingCriticalDamage);
        enemyManager.pendingCriticalDamage = 0;
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
    public void CanRotate()
    {
        animator.SetBool(DarkSoulsConsts.CANROTATE, true);
    }

    public void StopRotation()
    {
        animator.SetBool(DarkSoulsConsts.CANROTATE, false);
    }

    public void EnableCombo()
    {
        animator.SetBool(DarkSoulsConsts.CANDOCOMBO, true);
    }

    public void DisableCombo()
    {
        animator.SetBool(DarkSoulsConsts.CANDOCOMBO, false);
    }

    public void EnableIsInvulnerable()
    {
        animator.SetBool(DarkSoulsConsts.ISINVULNERABLE, true);
    }
    public void DisableIsInvulnerable()
    {
        animator.SetBool(DarkSoulsConsts.ISINVULNERABLE, false);
    }

    public void InstantiateBossParticleFX()
    {
        BossFXTransform bossFXTransform = GetComponentInChildren<BossFXTransform>();

        GameObject phaseFX = bossFXTransform.transform.GetChild(0).gameObject;
        phaseFX.SetActive(true);
    }

}
