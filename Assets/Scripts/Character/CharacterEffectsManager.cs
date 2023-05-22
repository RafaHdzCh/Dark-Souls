using System;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    [System.NonSerialized] public WeaponFX rightWeaponFX;
    [System.NonSerialized] public WeaponFX leftWeaponFX;

    [Header("Damage FX")]
    public GameObject bloodSplatterFx;

    [Header("Poison FX")]
    public int poisonDamage = 1;
    [System.NonSerialized] public bool isPoisoned;
    [System.NonSerialized] public float poisonBuildUp = 0;
    [System.NonSerialized] public float poisonAmount = 100;
    [System.NonSerialized] public float poisonTimer = 2;
    [System.NonSerialized] public float defaultPoisonAmount = 100;
    [System.NonSerialized] public float timer;

    public Transform buildUpTransform;
    public GameObject defaultPoisonParticlesVFX;
    [System.NonSerialized] public GameObject currentPoisonParticlesVFX;

    [Header("Scripts")]
    CharacterStatsManager characterStatsManager;

    protected virtual void Awake()
    {
        characterStatsManager = GetComponent<CharacterStatsManager>();
    }

    public virtual void PlayWeaponFX(bool isLeft)
    {
        if(!isLeft)
        {
            if (rightWeaponFX == null) return;

            rightWeaponFX.PlayWeaponFX();
        }
        else
        {
            if (leftWeaponFX == null) return;

            leftWeaponFX.PlayWeaponFX();
        }
    }

    public void PlayBloodSplat(Vector3 bloodSplatterLocation)
    {
        GameObject blood = Instantiate(bloodSplatterFx, bloodSplatterLocation, Quaternion.identity);
    }

    public virtual void HandleAllBuildUpEffects()
    {
        if (characterStatsManager.isDead) return;

        HandlePoisonBuildUp();
        HandlePoisonedEffect();
    }

    protected virtual void HandlePoisonBuildUp()
    {
        if (isPoisoned) return;
        
        if(poisonBuildUp > 0 && poisonBuildUp < 100)
        {
            poisonBuildUp -= 1 * Time.deltaTime;
        }
        else if(poisonBuildUp >= 100)
        {
            isPoisoned = true;
            poisonBuildUp = 0;

            if(buildUpTransform != null)
            {
                currentPoisonParticlesVFX = Instantiate(defaultPoisonParticlesVFX, buildUpTransform);
            }
            else
            {
                currentPoisonParticlesVFX = Instantiate(defaultPoisonParticlesVFX, characterStatsManager.transform);
            }
        }
    }

    protected virtual void HandlePoisonedEffect()
    {
        if(isPoisoned)
        {
            if(poisonAmount > 0)
            {
                timer += Time.deltaTime;
                if(timer >= poisonTimer)
                {
                    characterStatsManager.TakePoisonDamage(poisonDamage);
                    timer = 0;
                }
                poisonAmount -= 1 * Time.deltaTime;
            }
            else
            {
                isPoisoned = false;
                poisonAmount = defaultPoisonAmount;

                Destroy(currentPoisonParticlesVFX);
            }
        }
    }
}
