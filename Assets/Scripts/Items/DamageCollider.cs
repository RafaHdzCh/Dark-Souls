using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [System.NonSerialized] public CharacterManager characterManager;
    Collider damageCollider;

    [Header("Assign Weapon Damage")]
    public int currentWeaponDamage = 25;

    private void Awake()
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;
        damageCollider.enabled = false;
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

            if(characterManager != null)
            {
                if(characterManager.isParrying)
                {
                    characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation(DarkSoulsConsts.RIPOSTED, true);
                    return;
                }
            }

            if(playerStats != null)
            {
                playerStats.TakeDamage(currentWeaponDamage, true);
            }
        }
        if (collision.CompareTag(DarkSoulsConsts.ENEMY))
        {
            EnemyStats enemyStats = collision.GetComponent<EnemyStats>();
            CharacterManager enemyCaracterManager = collision.GetComponent<CharacterManager>();

            if (characterManager != null)
            {
                if (characterManager.isParrying)
                {
                    characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation(DarkSoulsConsts.RIPOSTED, true);
                    return;
                }
            }

            if (enemyStats != null)
            {
                enemyStats.TakeDamage(currentWeaponDamage, true);
            }
        }
    }
}
