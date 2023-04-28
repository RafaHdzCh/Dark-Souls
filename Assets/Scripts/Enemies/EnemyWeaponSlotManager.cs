using UnityEngine;

public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
{
    public WeaponItem rightHandWeapon;
    public WeaponItem leftHandWeapon;
    EnemyEffectsManager enemyEffectsManager;
    EnemyStatsManager enemyStatsManager;

    protected override void Awake()
    {
        base.Awake();
        enemyEffectsManager = GetComponent<EnemyEffectsManager>();
        enemyStatsManager = GetComponent<EnemyStatsManager>();
    }

    private void Start()
    {
        LoadWeaponsOnBothHands();
    }

    public void LoadWeaponOnSlot(WeaponItem weapon, bool isLeft)
    {
        if (isLeft)
        {
            leftHandSlot.currentWeapon = weapon;
            leftHandSlot.LoadWeaponModel(weapon);
            LoadWeaponsDamageCollider(true);
        }
        else
        {
            rightHandSlot.currentWeapon = weapon;
            rightHandSlot.LoadWeaponModel(weapon);
            LoadWeaponsDamageCollider(false);
        }
    }

    public void LoadWeaponsOnBothHands()
    {
        if (rightHandWeapon != null)
        {
            LoadWeaponOnSlot(rightHandWeapon, false);
        }
        if (leftHandWeapon != null)
        {
            LoadWeaponOnSlot(leftHandWeapon, true);
        }
    }

    public void LoadWeaponsDamageCollider(bool isLeft)
    {
        if (isLeft)
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            leftHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();

            leftHandDamageCollider.physicalDamage = leftHandWeapon.physicalDamage;
            leftHandDamageCollider.fireDamage = leftHandWeapon.fireDamage;

            enemyEffectsManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }
        else
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            rightHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();

            rightHandDamageCollider.physicalDamage = rightHandWeapon.physicalDamage;
            rightHandDamageCollider.fireDamage = rightHandWeapon.fireDamage;

            enemyEffectsManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();   
        }
    }

    public void OpenDamageCollider()
    {
        rightHandDamageCollider.EnableDamageCollider();
    }

    public void CloseDamageCollider()
    {
        if(rightHandDamageCollider != null)
        {
            rightHandDamageCollider.DisableDamageCollider();
        }
        if(leftHandDamageCollider != null)
        {
            leftHandDamageCollider.DisableDamageCollider();
        }
    }

    public void DrainStaminaLightAttack()
    {

    }

    public void DrainStaminaHeavyAttack()
    {

    }

    public void EnableCombo()
    {

    }

    public void DisableCombo()
    {

    }

    public void GrantWeaponAttackingPoiseBonus()
    {
        enemyStatsManager.totalPoiseDefence += enemyStatsManager.offensivePoiseBonus;
    }

    public void ResetWeaponAttackingPoiseBonus()
    {
        enemyStatsManager.totalPoiseDefence = enemyStatsManager.armorPoiseBonus;
    }
}