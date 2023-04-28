using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [System.NonSerialized] public CharacterManager characterManager;
    public bool enableDamagColliderOnStartUp = false;
    protected Collider damageCollider;

    [Header("Poise")]
    public float poiseBreak;
    public float offensivePoiseBonus;

    [Header("Assign Weapon Damage")]
    public int physicalDamage;
    public int fireDamage;
    public int magicDamage;
    public int lightningDamage;
    public int darkDamage;

    protected virtual void Awake()
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
        if (collision.CompareTag(DarkSoulsConsts.CHARACTER))
        {
            CharacterStatsManager enemyStats = collision.GetComponent<CharacterStatsManager>();
            CharacterManager enemyManager = collision.GetComponent<CharacterManager>();
            CharacterEffectsManager enemyEffects = collision.GetComponent<CharacterEffectsManager>();
            BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

            if(enemyManager != null)
            {
                if(enemyManager.isParrying)
                {
                    characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation(DarkSoulsConsts.PARRIED, true);
                    return;
                }
                else if(shield != null && enemyManager.isBlocking)
                {
                    float physicalDamageAfterBlock = physicalDamage - (physicalDamage * shield.blockingPhysicalDamageAbsorbtion) / 100;
                    float fireDamageAfterBlock = fireDamage - (fireDamage * shield.blockingFireDamageAbsortion) / 100;

                    if (enemyStats != null)
                    {
                        //enemyStats.TakeDamageNoAnimation(Mathf.RoundToInt(fireDamageAfterBlock));
                        enemyStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), Mathf.RoundToInt(fireDamageAfterBlock), DarkSoulsConsts.BLOCKIMPACT);
                        return;
                    }
                }
            }

            if(enemyStats != null)
            {
                enemyStats.poiseResetTimer = enemyStats.totalPoiseResetTime;
                enemyStats.totalPoiseDefence = enemyStats.totalPoiseDefence - poiseBreak;

                Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                enemyEffects.PlayBloodSplat(contactPoint);

                if (enemyStats.totalPoiseDefence > poiseBreak)
                {
                    enemyStats.TakeDamageNoAnimation(physicalDamage,0);
                }
                else
                {
                    enemyStats.TakeDamage(physicalDamage, 0, DarkSoulsConsts.DAMAGE);
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
