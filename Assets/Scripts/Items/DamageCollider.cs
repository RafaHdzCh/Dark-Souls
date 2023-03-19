using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [System.NonSerialized] public CharacterManager characterManager;
    [System.NonSerialized] public bool enableDamagColliderOnStartUp = false;
    Collider damageCollider;

    [Header("Assign Weapon Damage")]
    public float poiseBreak;
    public float offensivePoiseBonus;

    [Header("Assign Weapon Damage")]
    public int currentWeaponDamage;

    private void Awake()
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;
        damageCollider.enabled = enableDamagColliderOnStartUp;
    }

    public void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }

    public void DisableDamageCollider()
    {
        if (damageCollider == null) return;
        damageCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(DarkSoulsConsts.PLAYER))
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();
            CharacterManager enemyCaracterManager = collision.GetComponent<CharacterManager>();
            BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

            if(enemyCaracterManager != null)
            {
                if(enemyCaracterManager.isParrying)
                {
                    characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation(DarkSoulsConsts.PARRIED, true);
                    return;
                }
                else if(shield != null && enemyCaracterManager.isBlocking)
                {
                    float physicalDamageAfterBlock = currentWeaponDamage - (currentWeaponDamage * shield.blockingPhysicalDamageAbsorbtion) / 100;
                    if (playerStats != null)
                    {
                        playerStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), DarkSoulsConsts.BLOCKIMPACT);
                        return;
                    }
                }
            }

            if(playerStats != null)
            {
                playerStats.TakeDamage(currentWeaponDamage, DarkSoulsConsts.DAMAGE);
            }
        }
        if (collision.CompareTag(DarkSoulsConsts.ENEMY))
        {
            EnemyStats enemyStats = collision.GetComponent<EnemyStats>();
            CharacterManager enemyCaracterManager = collision.GetComponent<CharacterManager>();
            BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

            if (enemyCaracterManager != null)
            {
                if (enemyCaracterManager.isParrying)
                {
                    characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation(DarkSoulsConsts.PARRIED, true);
                    return;
                }
            }
            else if (shield != null && enemyCaracterManager.isBlocking)
            {
                float physicalDamageAfterBlock = currentWeaponDamage - (currentWeaponDamage * shield.blockingPhysicalDamageAbsorbtion) / 100;
                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), DarkSoulsConsts.BLOCKIMPACT);
                    return;
                }
            }

            if (enemyStats != null)
            {
                enemyStats.poiseResetTimer = enemyStats.totalPoiseResetTime;
                enemyStats.totalPoiseDefence = enemyStats.armorPoiseBonus - poiseBreak;

                if(enemyStats.totalPoiseDefence > poiseBreak)
                {
                    enemyStats.TakeDamageNoAnimation(currentWeaponDamage);
                }
                else
                {
                    enemyStats.TakeDamage(currentWeaponDamage, DarkSoulsConsts.DAMAGE);
                }
            }
        }
        if (collision.CompareTag(DarkSoulsConsts.ILLUSIONARYWALL))
        {
            IllusionaryWall illusionaryWall = collision.GetComponent<IllusionaryWall>();

            illusionaryWall.wallHasBeenHit= true;
        }
    }
}
