using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [Header("Directions")]
    [HideInInspector]public float horizontal;
    [HideInInspector] public float vertical;
    [HideInInspector] public float moveAmount;
    [HideInInspector] public float mouseX;
    [HideInInspector] public float mouseY;
    Vector2 movementInput;
    Vector2 cameraInput;

    [Header("Inputs")]
    [HideInInspector] public bool a_input;
    [HideInInspector] public bool b_input;
    [HideInInspector] public bool twoHand_input;
    [HideInInspector] public bool jump_Input;
    [HideInInspector] public bool rb_Input;
    [HideInInspector] public bool rt_Input;
    [HideInInspector] public bool d_pad_Up;
    [HideInInspector] public bool d_pad_Down;
    [HideInInspector] public bool d_pad_Left;
    [HideInInspector] public bool d_pad_Right;
    [HideInInspector] public bool start_Input;
    [HideInInspector] public bool lockOnInput;
    [HideInInspector] public bool switch_To_Right_Target_Input;
    [HideInInspector] public bool switch_To_Left_Target_Input;

    [Header("Flags")]
    [HideInInspector] public bool rollFlag;
    [HideInInspector] public bool twoHandFlag;
    [HideInInspector] public bool sprintFlag;
    [HideInInspector] public bool comboFlag;
    [HideInInspector] public bool lockOnFlag;
    [HideInInspector] public bool inventoryFlag;
    [HideInInspector] public float rollInputTimer;

    [Header("Scripts")]
    PlayerControls inputActions;
    PlayerAttacker playerAttacker;
    PlayerInventory playerInventory;
    PlayerManager playerManager;
    UIManager uiManager;
    CameraHandler cameraHandler;
    WeaponSlotManager weaponSlotManager;
   

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        playerAttacker = GetComponent<PlayerAttacker>();
        playerInventory = GetComponent<PlayerInventory>();
        uiManager = FindObjectOfType<UIManager>();
        cameraHandler = FindObjectOfType<CameraHandler>();
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
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
            inputActions.PlayerQuickSlots.DPadRight.performed += i => d_pad_Right = true;
            inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_pad_Left = true;
            inputActions.PlayerActions.A.performed += i => a_input = true;
            inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
            inputActions.PlayerActions.Start.performed += i => start_Input = true;
            inputActions.PlayerActions.LockOn.performed += i => lockOnInput = true;
            inputActions.PlayerMovement.LockOnTargetRight.performed += i => switch_To_Right_Target_Input = true;
            inputActions.PlayerMovement.LockOnTargetLeft.performed += i => switch_To_Left_Target_Input = true;
            inputActions.PlayerActions.TwoHand.performed += i => twoHand_input = true;
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
        b_input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
        sprintFlag = b_input;

        if (b_input)
        {
            rollInputTimer += delta;
        }
        else
        {
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
            if(playerManager.canDoCombo)
            {
                comboFlag = true;
                playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting) return;
                if (playerManager.canDoCombo) return;
                playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
            }
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
                playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
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
}