using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    [HideInInspector] public WeaponItem attackingWeapon;

    [Header("Scripts")]
    WeaponHolderSlot leftHandSlot;
    WeaponHolderSlot rightHandSlot;
    WeaponHolderSlot backSlot;

    DamageCollider leftHandDamageCollider;
    DamageCollider rightHandDamageCollider;

    QuickSlotsUI quickSlotsUI;
    PlayerStats playerStats;
    InputHandler inputHandler;
    Animator animator;
    PlayerManager playerManager;

    private void Awake()
    {
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
    }
    private void LoadRightWeaponDamageCollider()
    {
        rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }

    public void OpenDamageCollider()
    {
        if(playerManager.isUsingRightHand)
        {
            rightHandDamageCollider.EnableDamageCollider();
            print("collider derecho abierto");
        }
        else if(playerManager.isUsingLeftHand)
        {
            leftHandDamageCollider.EnableDamageCollider();
            print("collider izquierdo abierto");
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
