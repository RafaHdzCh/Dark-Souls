using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [System.NonSerialized] public CharacterManager characterManager;
    [System.NonSerialized] public bool enableDamagColliderOnStartUp = false;
    Collider damageCollider;

    [Header("Poise")]
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
            PlayerStatsManager playerStats = collision.GetComponent<PlayerStatsManager>();
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
                playerStats.poiseResetTimer = playerStats.totalPoiseResetTime;
                playerStats.totalPoiseDefence = playerStats.totalPoiseDefence - poiseBreak;
                print("Player poise is currently: " + playerStats.totalPoiseDefence);

                if (playerStats.totalPoiseDefence > poiseBreak)
                {
                    playerStats.TakeDamageNoAnimation(currentWeaponDamage);
                }
                else
                {
                    playerStats.TakeDamage(currentWeaponDamage, DarkSoulsConsts.DAMAGE);
                }
            }
        }
        if (collision.CompareTag(DarkSoulsConsts.ENEMY))
        {
            EnemyStatsManager enemyStats = collision.GetComponent<EnemyStatsManager>();
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
                enemyStats.totalPoiseDefence = enemyStats.totalPoiseDefence - poiseBreak;
                print("Enemy poise is currently: " + enemyStats.totalPoiseDefence);

                if(enemyStats.isBoss)
                {
                    if (enemyStats.totalPoiseDefence > poiseBreak)
                    {
                        enemyStats.TakeDamageNoAnimation(currentWeaponDamage);
                    }
                    else
                    {
                        enemyStats.TakeDamageNoAnimation(currentWeaponDamage);
                        enemyStats.BreakGuard();
                    }
                }
                else
                {
                    if (enemyStats.totalPoiseDefence > poiseBreak)
                    {
                        enemyStats.TakeDamageNoAnimation(currentWeaponDamage);
                    }
                    else
                    {
                        enemyStats.TakeDamage(currentWeaponDamage, DarkSoulsConsts.DAMAGE);
                    }
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
