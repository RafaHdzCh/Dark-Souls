using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    PlayerAnimatorManager playerAnimatorManager;
    PlayerEffectsManager playerEffectsManager;
    PlayerEquipmentManager playerEquipmentManager;
    PlayerInventoryManager playerInventoryManager;
    PlayerManager playerManager;
    PlayerStatsManager playerStatsManager;
    PlayerWeaponSlotManager playerWeaponSlotManager;
    InputHandler inputHandler;
    CameraHandler cameraHandler;

    [HideInInspector] public string lastAttack;

    [SerializeField] LayerMask backStabLayer;
    [SerializeField] LayerMask riposteLayer;

    const int attemptCriticalDamageCost = 10;
    const int criticalDamageCost = 15;

    private void Awake()
    {
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
        playerEquipmentManager= GetComponent<PlayerEquipmentManager>(); 
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerManager = GetComponent<PlayerManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        inputHandler = GetComponent<InputHandler>();
        cameraHandler = FindObjectOfType<CameraHandler>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (playerStatsManager.currentStamina <= 0) return;
        //Si se activa el combo...
        if (inputHandler.comboFlag)
        {
            //Se desactiva de inmediato el bool que permite hacer un combo y se ejecuta la animacion del segundo ataque segun el tipo de ataque.
            playerAnimatorManager.animator.SetBool(DarkSoulsConsts.CANDOCOMBO, false);
            //Si el ultimo ataque realizado fue el primer ataque ligero...

            if (lastAttack == DarkSoulsConsts.OH_LIGHT_ATTACK_0)
            {
                //Se ejecuta el segundo ataque ligero y se activa el bool isInteracting para evitar realizar otras acciones.
                playerAnimatorManager.PlayTargetAnimation(DarkSoulsConsts.OH_LIGHT_ATTACK_1, true);
            }
            //Si el ultimo ataque realizado fue el primer ataque pesado...
            if (lastAttack == DarkSoulsConsts.OH_HEAVY_ATTACK_0)
            {
                //Se ejecuta el segundo ataque pesado y se activa el bool isInteracting para evitar realizar otras acciones.
                playerAnimatorManager.PlayTargetAnimation(DarkSoulsConsts.OH_HEAVY_ATTACK_1, true);
            }

            if(lastAttack == DarkSoulsConsts.TH_LIGHT_ATTACK_0)
            {
                playerAnimatorManager.PlayTargetAnimation(DarkSoulsConsts.TH_LIGHT_ATTACK_1, true);
            }

            if (lastAttack == DarkSoulsConsts.TH_HEAVY_ATTACK_0)
            {
                playerAnimatorManager.PlayTargetAnimation(DarkSoulsConsts.TH_HEAVY_ATTACK_1, true);
            }
        }
    }

    //Realizar un ataque ligero.
    public void HandleLightAttack(WeaponItem weapon)
    {
        if (playerStatsManager.currentStamina <= 0) return;
        //Establece el arma de ataque del slotManager como item de ataque.
        playerWeaponSlotManager.attackingWeapon = weapon;

        if (inputHandler.twoHandFlag)
        {
            playerAnimatorManager.PlayTargetAnimation(DarkSoulsConsts.TH_LIGHT_ATTACK_0, true);
            lastAttack = DarkSoulsConsts.TH_LIGHT_ATTACK_0;
        }
        else
        {
            //Ejecuta su animacion y se activa el bool isInteracting para evitar realizar otras acciones.
            playerAnimatorManager.PlayTargetAnimation(DarkSoulsConsts.OH_LIGHT_ATTACK_0, true);
            //El ultimo ataque es el ataque realizado.
            lastAttack = DarkSoulsConsts.OH_LIGHT_ATTACK_0;
        }
    }

    //Realizar un ataque pesado.
    public void HandleHeavyAttack(WeaponItem weapon)
    {
        if (playerStatsManager.currentStamina <= 0) return;
        //Establece el arma de ataque del slotManager como item de ataque.
        playerWeaponSlotManager.attackingWeapon = weapon;

        if (inputHandler.twoHandFlag)
        {
            playerAnimatorManager.PlayTargetAnimation(DarkSoulsConsts.TH_HEAVY_ATTACK_0, true);
            lastAttack = DarkSoulsConsts.TH_HEAVY_ATTACK_0;
        }
        else
        {
            //Ejecuta su animacion y se activa el bool isInteracting para evitar realizar otras acciones.
            playerAnimatorManager.PlayTargetAnimation(DarkSoulsConsts.OH_HEAVY_ATTACK_0, true);
            //El ultimo ataque es el ataque realizado.
            lastAttack = DarkSoulsConsts.OH_HEAVY_ATTACK_0;
        }
    }

    #region Input Actions
    public void HandleRBAction()
    {
        if(playerInventoryManager.rightWeapon.weaponType == WeaponType.Melee ||
           playerInventoryManager.rightWeapon.weaponType == WeaponType.Unarmed)
        {
            PerformRBMeleeAction();
        }
        else if(playerInventoryManager.rightWeapon.weaponType == WeaponType.SpellCaster ||
                playerInventoryManager.rightWeapon.weaponType == WeaponType.FaithCaster ||
                playerInventoryManager.rightWeapon.weaponType == WeaponType.PyroCaster)
        {
            PerformRBMagicAction(playerInventoryManager.rightWeapon);
        }
    }

    public void HandleLBAction()
    {
        PerformLBBlockingAction();
    }

    public void HandleLTAction()
    {
        if (playerStatsManager.currentStamina <= 0) return;

        if (playerInventoryManager.leftWeapon.weaponType == WeaponType.Shield)
        {
            PerformLTWeaponArt(inputHandler.twoHandFlag);
        }
        else if (playerInventoryManager.leftWeapon.weaponType == WeaponType.Melee ||
                 playerInventoryManager.rightWeapon.weaponType == WeaponType.Unarmed)
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
            HandleWeaponCombo(playerInventoryManager.rightWeapon);
            inputHandler.comboFlag = false;
        }
        else
        {
            if (playerManager.isInteracting) return;
            if (playerManager.canDoCombo) return;
            HandleLightAttack(playerInventoryManager.rightWeapon);
        }

        playerEffectsManager.PlayWeaponFX(false);
    }

    private void PerformRBMagicAction(WeaponItem weapon)
    {
        if (playerManager.isInteracting) return;
        if(weapon.weaponType == WeaponType.FaithCaster)
        {
            if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isFaithSpell)
            {
                if (playerStatsManager.currentMana >= playerInventoryManager.currentSpell.manaCost)
                {
                    playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager);
                }
                else
                {
                    playerAnimatorManager.PlayTargetAnimation(DarkSoulsConsts.SHRUGGING, true);
                }
            }
        }
        else if (weapon.weaponType == WeaponType.PyroCaster)
        {
            if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isPyroSpell)
            {
                if (playerStatsManager.currentMana >= playerInventoryManager.currentSpell.manaCost)
                {
                    playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager);
                }
                else
                {
                    playerAnimatorManager.PlayTargetAnimation(DarkSoulsConsts.SHRUGGING, true);
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
            playerAnimatorManager.PlayTargetAnimation(playerInventoryManager.leftWeapon.Weapon_Art, true);
            playerStatsManager.TakeStaminaDamage(attemptCriticalDamageCost);
        }
    }

    private void SuccessfullyCastSpell()
    {
        playerInventoryManager.currentSpell.SuccessfullyCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager, cameraHandler);
        playerAnimatorManager.animator.SetBool(DarkSoulsConsts.ISFIRINGSPELL, true);
    }

    #endregion


    #region Defense Actions
    private void PerformLBBlockingAction()
    {
        if (playerManager.isInteracting) return;
        if (playerManager.isBlocking) return;

        playerAnimatorManager.PlayTargetAnimation(DarkSoulsConsts.BLOCKSTART, false, true);
        playerEquipmentManager.OpenBlockingCollider();
        playerManager.isBlocking = true;
    }

    #endregion
    public void AttemptBackStabOrRiposte()
    {
        if (playerStatsManager.currentStamina <= 0) return;

        RaycastHit hit;
        DamageCollider rightWeapon = playerWeaponSlotManager.rightHandDamageCollider;
        
        if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position,
            transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
        {
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            if (enemyCharacterManager == null) return;

            //Check for team ID (so you cant attack allis)
            playerStatsManager.TakeStaminaDamage(criticalDamageCost);
            playerManager.transform.position = enemyCharacterManager.backstabCollider.criticalDamagerStandPosition.position;

            Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
            rotationDirection = hit.transform.position - playerManager.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();
            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
            playerManager.transform.rotation = targetRotation;

            int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
            enemyCharacterManager.pendingCriticalDamage = criticalDamage;

            playerAnimatorManager.PlayTargetAnimation(DarkSoulsConsts.BACKSTAB, true);
            enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation(DarkSoulsConsts.BACKSTABBED, true);
        }
        else if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position,
                 transform.TransformDirection(Vector3.forward), out hit, 0.5f, riposteLayer))
        {
            //Check for team ID (so you cant attack allis)
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            if (enemyCharacterManager == null) return;
            if (!enemyCharacterManager.canBeRiposted) return;

            playerStatsManager.TakeStaminaDamage(criticalDamageCost);
            playerManager.transform.position = enemyCharacterManager.riposteCollider.criticalDamagerStandPosition.position;

            Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
            rotationDirection = hit.transform.position - playerManager.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();
            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
            playerManager.transform.rotation = targetRotation;

            int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
            enemyCharacterManager.pendingCriticalDamage = criticalDamage;

        playerAnimatorManager.PlayTargetAnimation(DarkSoulsConsts.RIPOSTE, true);

            enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation(DarkSoulsConsts.RIPOSTED, true);
                //print("Reproduciendo riposteo");
        }
    }
}
