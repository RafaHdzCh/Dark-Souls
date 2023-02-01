using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [Header("Directions")]
    [System.NonSerialized] public float horizontal;
    [System.NonSerialized] public float vertical;
    [System.NonSerialized] public float moveAmount;
    [System.NonSerialized] public float mouseX;
    [System.NonSerialized] public float mouseY;
    Vector2 movementInput;
    Vector2 cameraInput;

    [Header("Inputs")]
    [System.NonSerialized] public bool a_input;
    [System.NonSerialized] public bool b_input;
    [System.NonSerialized] public bool critical_Attack_Input;
    [System.NonSerialized] public bool twoHand_input;
    [System.NonSerialized] public bool jump_Input;
    [System.NonSerialized] public bool rb_Input;
    [System.NonSerialized] public bool rt_Input;
    [System.NonSerialized] public bool lb_Input;
    [System.NonSerialized] public bool lt_Input;
    [System.NonSerialized] public bool d_pad_Up;
    [System.NonSerialized] public bool d_pad_Down;
    [System.NonSerialized] public bool d_pad_Left;
    [System.NonSerialized] public bool d_pad_Right;
    [System.NonSerialized] public bool start_Input;
    [System.NonSerialized] public bool lockOnInput;
    [System.NonSerialized] public bool switch_To_Right_Target_Input;
    [System.NonSerialized] public bool switch_To_Left_Target_Input;

    [Header("Flags")]
    [System.NonSerialized] public bool rollFlag;
    [System.NonSerialized] public bool twoHandFlag;
    [System.NonSerialized] public bool sprintFlag;
    [System.NonSerialized] public bool comboFlag;
    [System.NonSerialized] public bool lockOnFlag;
    [System.NonSerialized] public bool inventoryFlag;
    [System.NonSerialized] public float rollInputTimer;

    public Transform criticalAttackRayCastStartPoint;

    [Header("Scripts")]
    PlayerAnimatorManager animatorHandler;
    PlayerControls inputActions;
    PlayerAttacker playerAttacker;
    PlayerInventory playerInventory;
    PlayerManager playerManager;
    UIManager uiManager;
    CameraHandler cameraHandler;
    WeaponSlotManager weaponSlotManager;
    PlayerStats playerStats;
   

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        playerStats = GetComponent<PlayerStats>();
        playerAttacker = GetComponentInChildren<PlayerAttacker>();
        playerInventory = GetComponent<PlayerInventory>();
        uiManager = FindObjectOfType<UIManager>();
        cameraHandler = FindObjectOfType<CameraHandler>();
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        animatorHandler = GetComponentInChildren<PlayerAnimatorManager>();
    }

    public void OnEnable()
    {
        if(inputActions == null)
        {
            //Se genera un nuevo PlayerControls  y su mapeo.
            inputActions = new PlayerControls();
            inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            inputActions.PlayerActions.RB.performed += i => rb_Input = true;
            inputActions.PlayerActions.RT.performed += i => rt_Input = true;
            inputActions.PlayerActions.LB.performed += i => lb_Input = true;
            inputActions.PlayerActions.LT.performed += i => lt_Input = true;

            inputActions.PlayerQuickSlots.DPadRight.performed += i => d_pad_Right = true;
            inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_pad_Left = true;
            inputActions.PlayerActions.A.performed += i => a_input = true;

            inputActions.PlayerActions.Roll.performed += i => b_input = true;
            inputActions.PlayerActions.Roll.canceled += i => b_input = false;

            inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
            inputActions.PlayerActions.Start.performed += i => start_Input = true;
            inputActions.PlayerActions.LockOn.performed += i => lockOnInput = true;
            inputActions.PlayerMovement.LockOnTargetRight.performed += i => switch_To_Right_Target_Input = true;
            inputActions.PlayerMovement.LockOnTargetLeft.performed += i => switch_To_Left_Target_Input = true;
            inputActions.PlayerActions.TwoHand.performed += i => twoHand_input = true;
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
        HandleAttackInput(delta);
        HandleQuickSlotsInput();
        HandleInventoryInput();
        HandleLockOnInput();
        HandleTwoHandInput();
        HandleCriticalAttackInput();
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

            if(playerStats.currentStamina <= 0)
            {
                b_input = false;
                sprintFlag = false;
            }

            if(moveAmount > 0.5f && playerStats.currentStamina > 0)
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

    private void HandleAttackInput(float delta)
    {
        if(rb_Input)
        {
            animatorHandler.anim.SetBool(DarkSoulsConsts.ISUSINGRIGHTHAND, true);
            playerAttacker.HandleRBAction();
        }
        if(rt_Input)
        {
            if (playerManager.canDoCombo)
            {
                comboFlag = true;
                playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting) return;
                if (playerManager.canDoCombo) return;
                animatorHandler.anim.SetBool(DarkSoulsConsts.ISUSINGRIGHTHAND, true);
                playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
            }
        }
        if(lb_Input)
        {

        }
        if(lt_Input)
        {
            if(twoHandFlag)
            {
                //handle weapon art
            }
            else
            {
                playerAttacker.HandleLTAction();
            }
        }
    }

    private void HandleQuickSlotsInput()
    {
        if (d_pad_Right)
        {
            playerInventory.ChangeRightWeapon();
        }
        else if(d_pad_Left)
        {
            playerInventory.ChangeLeftWeapon();
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
        if(lockOnInput && lockOnFlag == false)
        {
            lockOnInput = false;
            cameraHandler.HandleLockOn();

            if(cameraHandler.nearestLockOnTarget != null)
            {
                cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                lockOnFlag = true;
            }
        }
        else if(lockOnInput && lockOnFlag)
        {
            lockOnFlag = false;
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
        if(twoHand_input)
        {
            twoHand_input = false;
            twoHandFlag = !twoHandFlag;
            if(twoHandFlag)
            {
                weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
            }
            else
            {
                weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);
            }
        }
    }

    private void HandleCriticalAttackInput()
    {
        if(critical_Attack_Input)
        {
            critical_Attack_Input = false;
            playerAttacker.AttemptBackStabOrRiposte();
        }
    }
}