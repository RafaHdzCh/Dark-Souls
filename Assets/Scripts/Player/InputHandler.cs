using UnityEngine;

public class InputHandler : MonoBehaviour
{
    #region Directions Input
    [Header("Directions")]
    [System.NonSerialized] public float horizontal;
    [System.NonSerialized] public float vertical;
    [System.NonSerialized] public float moveAmount;
    [System.NonSerialized] public float mouseX;
    [System.NonSerialized] public float mouseY;
    Vector2 movementInput;
    Vector2 cameraInput;
    #endregion

    #region Buttons Input
    [Header("Inputs")]
    [System.NonSerialized] public bool a_input;
    [System.NonSerialized] public bool b_input;
    [System.NonSerialized] public bool x_input;
    [System.NonSerialized] public bool y_input;

    [System.NonSerialized] public bool rb_Input;
    [System.NonSerialized] public bool rt_Input;

    [System.NonSerialized] public bool lb_Input;
    [System.NonSerialized] public bool lt_Input;

    [System.NonSerialized] public bool d_pad_Up;
    [System.NonSerialized] public bool d_pad_Down;
    [System.NonSerialized] public bool d_pad_Left;
    [System.NonSerialized] public bool d_pad_Right;

    [System.NonSerialized] public bool start_Input;
    [System.NonSerialized] public bool select_Input;

    [System.NonSerialized] public bool right_Stick_Press;
    [System.NonSerialized] public bool left_Stick_Press;

    [System.NonSerialized] public bool critical_Attack_Input;
    [System.NonSerialized] public bool switch_To_Right_Target_Input;
    [System.NonSerialized] public bool switch_To_Left_Target_Input;
    #endregion

    #region Flags
    [Header("Flags")]
    [System.NonSerialized] public bool rollFlag;
    [System.NonSerialized] public bool twoHandFlag;
    [System.NonSerialized] public bool sprintFlag;
    [System.NonSerialized] public bool comboFlag;
    [System.NonSerialized] public bool lockOnFlag;
    [System.NonSerialized] public bool inventoryFlag;
    [System.NonSerialized] public float rollInputTimer;
    #endregion

    #region Scripts

    [Header("Scripts")]

    PlayerAnimatorManager playerAnimatorManager;
    PlayerControls inputActions;
    PlayerCombatManager playerCombatManager;
    PlayerInventoryManager playerInventoryManager;
    PlayerManager playerManager;
    PlayerEffectsManager playerEffectsManager;
    UIManager uiManager;
    CameraHandler cameraHandler;
    PlayerWeaponSlotManager playerWeaponSlotManager;
    PlayerStatsManager playerStatsManager;
    BlockingCollider blockingCollider;
    #endregion

    public Transform criticalAttackRayCastStartPoint;


    private void Start()
    {
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerCombatManager = GetComponent<PlayerCombatManager>();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerManager = GetComponent<PlayerManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();

        uiManager = FindObjectOfType<UIManager>();
        cameraHandler = FindObjectOfType<CameraHandler>();
        blockingCollider = GetComponentInChildren<BlockingCollider>();
    }

    public void OnEnable()
    {
        if(inputActions == null)
        {
            //Se genera un nuevo PlayerControls  y su mapeo.
            inputActions = new PlayerControls();

            inputActions.PlayerActions.A.performed += i => a_input = true;
            inputActions.PlayerActions.B.performed += i => b_input = true;
            inputActions.PlayerActions.B.canceled += i => b_input = false;
            inputActions.PlayerActions.X.performed += i => x_input = true;
            inputActions.PlayerActions.Y.performed += i => y_input = true;

            inputActions.PlayerActions.RB.performed += i => rb_Input = true;
            inputActions.PlayerActions.RT.performed += i => rt_Input = true;

            inputActions.PlayerActions.LB.performed += i => lb_Input = true;
            inputActions.PlayerActions.LB.canceled += i => lb_Input = false;
            inputActions.PlayerActions.LT.performed += i => lt_Input = true;

            inputActions.PlayerQuickSlots.DPadUp.performed += i => d_pad_Up = true;
            inputActions.PlayerQuickSlots.DPadDown.performed += i => d_pad_Down = true;
            inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_pad_Left = true;
            inputActions.PlayerQuickSlots.DPadRight.performed += i => d_pad_Right = true;

            inputActions.PlayerActions.Start.performed += i => start_Input = true;
            inputActions.PlayerActions.Select.performed += i => select_Input = true;

            inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            inputActions.PlayerActions.RightSickPress.performed += i => right_Stick_Press = true;
            inputActions.PlayerMovement.LockOnTargetRight.performed += i => switch_To_Right_Target_Input = true;
            inputActions.PlayerMovement.LockOnTargetLeft.performed += i => switch_To_Left_Target_Input = true;
            inputActions.PlayerActions.CriticalAttack.performed += i => critical_Attack_Input = true;
        }
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    //Detecta los inputs por el usuario.
    public void TickInput(float delta)
    {
        HandleMoveInput(delta);
        HandleRollInput(delta);
        HandleCombatInput(delta);
        HandleQuickSlotsInput();
        HandleInventoryInput();
        HandleLockOnInput();
        HandleTwoHandInput();
        HandleCriticalAttackInput();
        HandleUseConsumableInput();
    }

    private void HandleMoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }

    private void HandleRollInput( float delta)
    {
        if (b_input)
        {
            rollInputTimer += delta;

            if(playerStatsManager.currentStamina <= 0)
            {
                b_input = false;
                sprintFlag = false;
            }

            if(moveAmount > 0.5f && playerStatsManager.currentStamina > 0)
            {
                sprintFlag = true;
            }
        }
        else
        {
            sprintFlag = false;
            if(rollInputTimer > 0 && rollInputTimer < 0.5f)
            {
                sprintFlag = false;
                rollFlag = true;
            }
            rollInputTimer = 0;
        }
    }

    private void HandleCombatInput(float delta)
    {
        if(rb_Input)
        {
            playerAnimatorManager.animator.SetBool(DarkSoulsConsts.ISUSINGRIGHTHAND, true);
            playerCombatManager.HandleRBAction();
        }
        if(rt_Input)
        {
            if (playerManager.canDoCombo)
            {
                comboFlag = true;
                playerCombatManager.HandleWeaponCombo(playerInventoryManager.rightWeapon);
                comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting) return;
                if (playerManager.canDoCombo) return;
                playerAnimatorManager.animator.SetBool(DarkSoulsConsts.ISUSINGRIGHTHAND, true);
                playerCombatManager.HandleHeavyAttack(playerInventoryManager.rightWeapon);
            }
        }
        if(lb_Input)
        {
            playerCombatManager.HandleLBAction();
        }
        else
        {
            playerManager.isBlocking = false;
            if(blockingCollider.blockingCollider.enabled)
            {
                blockingCollider.DisableBlockingCollider();
            }
        }
        if(lt_Input)
        {
            if(twoHandFlag)
            {
                //handle weapon art
            }
            else
            {
                playerCombatManager.HandleLTAction();
            }
        }
    }

    private void HandleQuickSlotsInput()
    {
        if (d_pad_Right)
        {
            playerInventoryManager.ChangeRightWeapon();
        }
        else if(d_pad_Left)
        {
            playerInventoryManager.ChangeLeftWeapon();
        }
    }

    private void HandleInventoryInput()
    {
        if(start_Input)
        {
            inventoryFlag = !inventoryFlag;

            if(inventoryFlag)
            {
                uiManager.OpenSelectWindow();
                uiManager.UpdateUI();
                uiManager.hudWindow.SetActive(false);
            }
            else
            {
                uiManager.CloseSelectWindow();
                uiManager.CloseAllInventoryWindows();
                uiManager.hudWindow.SetActive(true);
            }
        }
    }

    private void HandleLockOnInput()
    {
        if(right_Stick_Press && lockOnFlag == false)
        {
            right_Stick_Press = false;
            cameraHandler.HandleLockOn();

            if(cameraHandler.nearestLockOnTarget != null)
            {
                cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                lockOnFlag = true;
            }
        }
        else if(right_Stick_Press && lockOnFlag)
        {
            lockOnFlag = false;
            cameraHandler.ClearLockOnTargets();
        }

        if(lockOnFlag && switch_To_Left_Target_Input)
        {
            switch_To_Left_Target_Input = false;
            cameraHandler.HandleLockOn();
            if(cameraHandler.leftLockTarget != null)
            {
                cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
            }
        }
        if(lockOnFlag && switch_To_Right_Target_Input)
        {
            switch_To_Right_Target_Input = false;
            cameraHandler.HandleLockOn();
            if(cameraHandler.rightLockTarget != null)
            {
                cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
            }
        }
        cameraHandler.SetCameraHeight();
    }

    private void HandleTwoHandInput()
    {
        if(y_input)
        {
            y_input = false;
            twoHandFlag = !twoHandFlag;
            if(twoHandFlag)
            {
                playerWeaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
            }
            else
            {
                playerWeaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
                playerWeaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.leftWeapon, true);
            }
        }
    }

    private void HandleCriticalAttackInput()
    {
        if(critical_Attack_Input)
        {
            critical_Attack_Input = false;
            playerCombatManager.AttemptBackStabOrRiposte();
        }
    }

    private void HandleUseConsumableInput()
    {
        if(x_input)
        {
            x_input = false;
            playerInventoryManager.currentConsumableItem.AttemptToConsumeItem(playerAnimatorManager, playerWeaponSlotManager, playerEffectsManager);

        }
    }
}