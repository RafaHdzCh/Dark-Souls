using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    [HideInInspector] public WeaponItem attackingWeapon;

    [Header("Scripts")]
    WeaponHolderSlot leftHandSlot;
    WeaponHolderSlot rightHandSlot;
    DamageCollider leftHandDamageCollider;
    DamageCollider rightHandDamageCollider;
    QuickSlotsUI quickSlotsUI;
    PlayerStats playerStats;

    private void Awake()
    {
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
        playerStats = GetComponentInParent<PlayerStats>();

        foreach(WeaponHolderSlot weaponSlot in weaponHolderSlots)
        {
            if(weaponSlot.isLeftHandSlot)
            {
                leftHandSlot = weaponSlot;
            }
            else if(weaponSlot.isRightHandSlot)
            {
                rightHandSlot = weaponSlot;
            }
        }
    }

    public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        if(isLeft)
        {
            leftHandSlot.LoadWeaponModel(weaponItem);
            LoadLeftWeaponDamageCollider();
            quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
        }
        else
        {
            rightHandSlot.LoadWeaponModel(weaponItem);
            LoadRightWeaponDamageCollider();
            quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
        }
    }

    #region Handle Weapon's Damage Collider

    private void LoadLeftWeaponDamageCollider()
    {
        leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }
    private void LoadRightWeaponDamageCollider()
    {
        rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }

    public void OpenLeftHandDamageCollider()
    {
        leftHandDamageCollider.EnableDamageCollider();
    }
    public void OpenRightHandDamageCollider()
    {
        rightHandDamageCollider.EnableDamageCollider();
    }

    public void CloseLeftHandDamageCollider()
    {
        
        leftHandDamageCollider.DisableDamageCollider();
    }
    public void CloseRightHandDamageCollider()
    {
        rightHandDamageCollider.DisableDamageCollider();
    }

    #endregion

    #region Handle Weapon's Stamina Drainage
    public void DrainStaminaLightAttack()
    {
        playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
    }
    public void DrainStaminaHeavyAttack()
    {
        playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
    }
    #endregion
}
