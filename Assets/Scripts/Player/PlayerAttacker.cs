using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacker : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerEquipmentManager playerEquipmentManager;
    PlayerStats playerStats;
    PlayerInventory playerInventory;
    PlayerAnimatorManager animatorHandler;
    InputHandler inputHandler;
    CameraHandler cameraHandler;
    WeaponSlotManager weaponSlotManager;
    [HideInInspector] public string lastAttack;

    [SerializeField] LayerMask backStabLayer;
    [SerializeField] LayerMask riposteLayer;

    const int attemptCriticalDamageCost = 10;
    const int criticalDamageCost = 15;

    private void Awake()
    {
        playerEquipmentManager= GetComponent<PlayerEquipmentManager>(); 
        playerManager = GetComponentInParent<PlayerManager>();
        playerStats = GetComponentInParent<PlayerStats>();
        playerInventory = GetComponentInParent<PlayerInventory>();
        playerManager = GetComponentInParent<PlayerManager>();
        animatorHandler = GetComponent<PlayerAnimatorManager>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
        inputHandler = GetComponentInParent<InputHandler>();
        cameraHandler = FindObjectOfType<CameraHandler>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (playerStats.currentStamina <= 0) return;
        //Si se activa el combo...
        if (inputHandler.comboFlag)
        {
            //Se desactiva de inmediato el bool que permite hacer un combo y se ejecuta la animacion del segundo ataque segun el tipo de ataque.
            animatorHandler.anim.SetBool(DarkSoulsConsts.CANDOCOMBO, false);
            //Si el ultimo ataque realizado fue el primer ataque ligero...

            if (lastAttack == weapon.OH_Light_Attack_0)
            {
                //Se ejecuta el segundo ataque ligero y se activa el bool isInteracting para evitar realizar otras acciones.
                animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
            }
            //Si el ultimo ataque realizado fue el primer ataque pesado...
            if (lastAttack == weapon.OH_Heavy_Attack_0)
            {
                //Se ejecuta el segundo ataque pesado y se activa el bool isInteracting para evitar realizar otras acciones.
                animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
            }

            if(lastAttack == weapon.TH_Light_Attack_0)
            {
                animatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_1, true);
            }

            if (lastAttack == weapon.TH_Heavy_Attack_0)
            {
                animatorHandler.PlayTargetAnimation(weapon.TH_Heavy_Attack_1, true);
            }
        }
    }

    //Realizar un ataque ligero.
    public void HandleLightAttack(WeaponItem weapon)
    {
        if (playerStats.currentStamina <= 0) return;
        //Establece el arma de ataque del slotManager como item de ataque.
        weaponSlotManager.attackingWeapon = weapon;

        if (inputHandler.twoHandFlag)
        {
            animatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_0, true);
            lastAttack = weapon.TH_Light_Attack_0;
        }
        else
        {
            //Ejecuta su animacion y se activa el bool isInteracting para evitar realizar otras acciones.
            animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_0, true);
            //El ultimo ataque es el ataque realizado.
            lastAttack = weapon.OH_Light_Attack_0;
        }
    }

    //Realizar un ataque pesado.
    public void HandleHeavyAttack(WeaponItem weapon)
    {
        if (playerStats.currentStamina <= 0) return;
        //Establece el arma de ataque del slotManager como item de ataque.
        weaponSlotManager.attackingWeapon = weapon;

        if (inputHandler.twoHandFlag)
        {
            animatorHandler.PlayTargetAnimation(weapon.TH_Heavy_Attack_0, true);
            lastAttack = weapon.TH_Heavy_Attack_0;
        }
        else
        {
            //Ejecuta su animacion y se activa el bool isInteracting para evitar realizar otras acciones.
            animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_0, true);
            //El ultimo ataque es el ataque realizado.
            lastAttack = weapon.OH_Heavy_Attack_0;
        }
    }

    #region Input Actions
    public void HandleRBAction()
    {
        if(playerInventory.rightWeapon.isMeleeWeapon)
        {
            PerformRBMeleeAction();
        }
        else if(playerInventory.leftWeapon.isSpellCaster ||
                playerInventory.rightWeapon.isFaithCaster ||
                playerInventory.rightWeapon.isPyroCaster)
        {
            PerformRBMagicAction(playerInventory.rightWeapon);
        }
    }

    public void HandleLBAction()
    {
        PerformLBBlockingAction();
    }

    public void HandleLTAction()
    {
        if (playerStats.currentStamina <= 0) return;

        if (playerInventory.leftWeapon.isShieldWeapon)
        {
            PerformLTWeaponArt(inputHandler.twoHandFlag);
        }
        else if(playerInventory.leftWeapon.isMeleeWeapon)
        {
            //Do a light attack
        }
    }

    #endregion

    #region Attack Actions
    private void PerformRBMeleeAction()
    {
        if (playerManager.canDoCombo)
        {
            inputHandler.comboFlag = true;
            HandleWeaponCombo(playerInventory.rightWeapon);
            inputHandler.comboFlag = false;
        }
        else
        {
            if (playerManager.isInteracting) return;
            if (playerManager.canDoCombo) return;
            HandleLightAttack(playerInventory.rightWeapon);
        }
    }

    private void PerformRBMagicAction(WeaponItem weapon)
    {
        if (playerManager.isInteracting) return;
        if(weapon.isFaithCaster)
        {
            if (playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpell)
            {
                if (playerStats.currentMana >= playerInventory.currentSpell.manaCost)
                {
                    playerInventory.currentSpell.AttemptToCastSpell(animatorHandler, playerStats, weaponSlotManager);
                }
                else
                {
                    animatorHandler.PlayTargetAnimation(DarkSoulsConsts.SHRUGGING, true);
                }
            }
        }
        else if (weapon.isPyroCaster)
        {
            if (playerInventory.currentSpell != null && playerInventory.currentSpell.isPyroSpell)
            {
                if (playerStats.currentMana >= playerInventory.currentSpell.manaCost)
                {
                    playerInventory.currentSpell.AttemptToCastSpell(animatorHandler, playerStats, weaponSlotManager);
                }
                else
                {
                    animatorHandler.PlayTargetAnimation(DarkSoulsConsts.SHRUGGING, true);
                }
            }
        }
    }

    private void PerformLTWeaponArt(bool isTwoHanding)
    {
        if (playerManager.isInteracting) return;

        if(isTwoHanding)
        {
            //perform right weapon art
        }
        else
        {
            animatorHandler.PlayTargetAnimation(playerInventory.leftWeapon.Weapon_Art, true);
            playerStats.TakeStaminaDamage(attemptCriticalDamageCost);
        }
    }

    private void SuccessfullyCastSpell()
    {
        playerInventory.currentSpell.SuccessfullyCastSpell(animatorHandler, playerStats, weaponSlotManager, cameraHandler);
        animatorHandler.anim.SetBool(DarkSoulsConsts.ISFIRINGSPELL, true);
    }

    #endregion

    private void PerformLBBlockingAction()
    {
        if (playerManager.isInteracting) return;
        if (playerManager.isBlocking) return;

        animatorHandler.PlayTargetAnimation(DarkSoulsConsts.BLOCKSTART, false);
        playerEquipmentManager.OpenBlockingCollider();
        playerManager.isBlocking = true;
    }

    #region Defense Actions

    public void AttemptBackStabOrRiposte()
    {
        if (playerStats.currentStamina <= 0) return;

        RaycastHit hit;
        DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;
        

        if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position,
            transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
        {
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            if (enemyCharacterManager == null) return;

            //Check for team ID (so you cant attack allis)
            playerStats.TakeStaminaDamage(criticalDamageCost);
            playerManager.transform.position = enemyCharacterManager.backstabCollider.criticalDamagerStandPosition.position;

            Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
            rotationDirection = hit.transform.position - playerManager.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();
            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
            playerManager.transform.rotation = targetRotation;

            int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
            enemyCharacterManager.pendingCriticalDamage = criticalDamage;

            animatorHandler.PlayTargetAnimation(DarkSoulsConsts.BACKSTAB, true);
            enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation(DarkSoulsConsts.BACKSTABBED, true);
        }
        else if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position,
                 transform.TransformDirection(Vector3.forward), out hit, 0.5f, riposteLayer))
        {
            //Check for team ID (so you cant attack allis)
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            if (enemyCharacterManager == null) return;
            if (!enemyCharacterManager.canBeRiposted) return;

            playerStats.TakeStaminaDamage(criticalDamageCost);
            playerManager.transform.position = enemyCharacterManager.riposteCollider.criticalDamagerStandPosition.position;

            Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
            rotationDirection = hit.transform.position - playerManager.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();
            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
            playerManager.transform.rotation = targetRotation;

            int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
            enemyCharacterManager.pendingCriticalDamage = criticalDamage;

            animatorHandler.PlayTargetAnimation(DarkSoulsConsts.RIPOSTE, true);
            enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation(DarkSoulsConsts.RIPOSTED, true);
        }
    }

    #endregion
}
