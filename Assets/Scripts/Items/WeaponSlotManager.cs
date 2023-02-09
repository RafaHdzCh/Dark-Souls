using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    [HideInInspector] public WeaponItem attackingWeapon;

    [Header("Scripts")]
    [System.NonSerialized] public WeaponHolderSlot leftHandSlot;
    [System.NonSerialized] public WeaponHolderSlot rightHandSlot;
    [System.NonSerialized] public WeaponHolderSlot backSlot;

    [System.NonSerialized] public DamageCollider leftHandDamageCollider;
    [System.NonSerialized] public DamageCollider rightHandDamageCollider;

    QuickSlotsUI quickSlotsUI;
    PlayerStats playerStats;
    InputHandler inputHandler;
    Animator animator;
    PlayerManager playerManager;
    PlayerInventory playerInventory;

    private void Awake()
    {
        playerInventory = GetComponentInParent<PlayerInventory>();
        quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
        inputHandler = GetComponentInParent<InputHandler>();
        playerStats = GetComponentInParent<PlayerStats>();
        animator = GetComponent<Animator>();
        playerManager = GetComponentInParent<PlayerManager>();

        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
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
            else if(weaponSlot.isBackSlot)
            {
                backSlot = weaponSlot;
            }
        }
    }

    public void LoadBothWaponsOnSlots()
    {
        LoadWeaponOnSlot(playerInventory.rightWeapon, false);
        LoadWeaponOnSlot(playerInventory.leftWeapon, true);
    }

    public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        if(isLeft)
        {
            leftHandSlot.currentWeapon = weaponItem;
            leftHandSlot.LoadWeaponModel(weaponItem);
            LoadLeftWeaponDamageCollider();
            quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);

            #region Handle Left Weapon Idle Animation
            if (weaponItem != null)
            {
                animator.CrossFade(weaponItem.Left_Arm_Idle, 0.2f);
            }
            else
            {
                animator.CrossFade(DarkSoulsConsts.LEFTARMEMPTY, 0.2f);
            }
            #endregion
        }
        else
        {
            if(inputHandler.twoHandFlag)
            {
                backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                leftHandSlot.UnloadWeaponAndDestroy();
                animator.CrossFade(weaponItem.TH_Idle, 0.2f);
            }
            else
            {
                #region Handle Right Weapon Idle Animation

                animator.CrossFade(DarkSoulsConsts.BOTHARMSEMPTY, 0.2f);

                backSlot.UnloadWeaponAndDestroy();
                if (weaponItem != null)
                {

                    animator.CrossFade(weaponItem.Right_Arm_Idle, 0.2f);
                }
                else
                {
                    animator.CrossFade(DarkSoulsConsts.RIGHTARMEMPTY, 0.2f);
                }

                #endregion
            }
            rightHandSlot.currentWeapon = weaponItem;
            rightHandSlot.LoadWeaponModel(weaponItem);
            LoadRightWeaponDamageCollider();
            quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
        }
    }

    #region Handle Weapon's Damage Collider

    private void LoadLeftWeaponDamageCollider()
    {
        leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            leftHandDamageCollider.currentWeaponDamage = playerInventory.leftWeapon.baseDamage;
    }
    private void LoadRightWeaponDamageCollider()
    {
        rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        rightHandDamageCollider.currentWeaponDamage = playerInventory.rightWeapon.baseDamage;
    }

    public void OpenDamageCollider()
    {
        if(playerManager.isUsingRightHand)
        {
            rightHandDamageCollider.EnableDamageCollider();
        }
        else if(playerManager.isUsingLeftHand)
        {
            leftHandDamageCollider.EnableDamageCollider();
        }
    }

    public void CloseDamageCollider()
    {
        rightHandDamageCollider.DisableDamageCollider();
        leftHandDamageCollider.DisableDamageCollider();
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
