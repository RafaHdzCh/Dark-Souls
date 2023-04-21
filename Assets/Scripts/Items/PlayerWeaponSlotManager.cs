using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
{
    QuickSlotsUI quickSlotsUI;
    InputHandler inputHandler;
    Animator animator;
    PlayerEffectsManager playerEffectsManager;
    PlayerInventoryManager playerInventoryManager;
    PlayerManager playerManager;
    PlayerStatsManager playerStatsManager;
    [SerializeField] CameraHandler cameraHandler;

    protected override void Awake()
    {
        base.Awake();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerManager = GetComponent<PlayerManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
        inputHandler = GetComponent<InputHandler>();
        animator = GetComponent<Animator>();
    }

    public void LoadBothWaponsOnSlots()
    {
        LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
        LoadWeaponOnSlot(playerInventoryManager.leftWeapon, true);
    }

    public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        if(weaponItem != null)
        {
            if(isLeft)
            {
                leftHandSlot.currentWeapon = weaponItem;
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                animator.CrossFade(weaponItem.Left_Arm_Idle, 0.2f);
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
                    animator.CrossFade(DarkSoulsConsts.BOTHARMSEMPTY, 0.2f);
                    backSlot.UnloadWeaponAndDestroy();
                    animator.CrossFade(weaponItem.Right_Arm_Idle, 0.2f);
                }

                rightHandSlot.currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
            }
        }
        else
        {
            if(isLeft)
            {
                animator.CrossFade(DarkSoulsConsts.LEFTARMEMPTY, 0.2f);
                playerInventoryManager.leftWeapon = unarmedWeapon;
                leftHandSlot.currentWeapon = unarmedWeapon;
                leftHandSlot.LoadWeaponModel(unarmedWeapon);
                LoadLeftWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(true, unarmedWeapon);
            }
            else
            {
                animator.CrossFade(DarkSoulsConsts.RIGHTARMEMPTY, 0.2f);
                playerInventoryManager.rightWeapon = unarmedWeapon;
                rightHandSlot.currentWeapon = unarmedWeapon;
                rightHandSlot.LoadWeaponModel(unarmedWeapon);
                LoadRightWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(false, unarmedWeapon);
            }
        }
    }

    public void SuccesfullyThrowFirebom()
    {
        Destroy(playerEffectsManager.instantiatedFXModel);
        BombItem firebombItem = playerInventoryManager.currentConsumableItem as BombItem;

        GameObject activeModelBomb = Instantiate(firebombItem.liveBombModel, rightHandSlot.transform.position, cameraHandler.cameraPivotTransform.rotation);
        activeModelBomb.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x, playerManager.lockOnTransform.eulerAngles.y, 0);
    }

    #region Handle Weapon's Damage Collider

    private void LoadLeftWeaponDamageCollider()
    {
        leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        leftHandDamageCollider.currentWeaponDamage = playerInventoryManager.leftWeapon.baseDamage;
        leftHandDamageCollider.poiseBreak = playerInventoryManager.leftWeapon.poiseBreak;
        playerEffectsManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
    }

    private void LoadRightWeaponDamageCollider()
    {
        rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        rightHandDamageCollider.currentWeaponDamage = playerInventoryManager.rightWeapon.baseDamage;
        rightHandDamageCollider.poiseBreak = playerInventoryManager.rightWeapon.poiseBreak;
        playerEffectsManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
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
        if(rightHandDamageCollider != null)
        {
            rightHandDamageCollider.DisableDamageCollider();
        }
        if(leftHandDamageCollider != null)
        {
            leftHandDamageCollider.DisableDamageCollider();
        }
    }

    #endregion

    #region Handle Weapon's Stamina Drainage
    public void DrainStaminaLightAttack()
    {
        playerStatsManager.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
    }
    public void DrainStaminaHeavyAttack()
    {
        playerStatsManager.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
    }
    #endregion

    #region Handle Weapon's Poise Bonus

    public void GrantWeaponAttackingPoiseBonus()
    {
        playerStatsManager.totalPoiseDefence += attackingWeapon.offensivePoiseBonus;
    }

    public void ResetWeaponAttackingPoiseBonus()
    {
        playerStatsManager.totalPoiseDefence = playerStatsManager.armorPoiseBonus;
    }

    #endregion
}
