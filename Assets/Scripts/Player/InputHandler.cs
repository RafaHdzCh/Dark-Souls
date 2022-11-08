using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    public bool a_input;
    public bool b_input;
    public bool jump_Input;
    public bool rb_Input;
    public bool rt_Input;
    public bool d_pad_Up;
    public bool d_pad_Down;
    public bool d_pad_Left;
    public bool d_pad_Right;
    public bool start_Input;

    public bool rollFlag;
    public bool sprintFlag;
    public bool comboFlag;
    public bool inventoryFlag;
    public float rollInputTimer;

    
    PlayerControls inputActions;
    PlayerAttacker playerAttacker;
    PlayerInventory playerInventory;
    PlayerManager playerManager;
    UIManager uiManager;
   
    Vector2 movementInput;
    Vector2 cameraInput;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerAttacker = GetComponent<PlayerAttacker>();
        playerInventory = GetComponent<PlayerInventory>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void OnEnable()
    {
        if(inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
        }
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void TickInput(float delta)
    {
        MoveInput(delta);
        HandleRollInput(delta);
        HandleAttackInput(delta);
        HandleQuickSlotsInput();
        HandleInteractableButtonInput();
        HanldeJumpInput();
        HandleInventoryInput();
    }
    private void MoveInput(float delta)
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
        if (b_input)
        {
            rollInputTimer += delta;
            sprintFlag = true;
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
        inputActions.PlayerActions.RB.performed += i => rb_Input = true;
        inputActions.PlayerActions.RT.performed += i => rt_Input = true;

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
        inputActions.PlayerQuickSlots.DPadRight.performed += i => d_pad_Right = true;
        inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_pad_Left = true;

        if (d_pad_Right)
        {
            playerInventory.ChangeRightWeapon();
        }
        else if(d_pad_Left)
        {
            playerInventory.ChangeLeftWeapon();
        }
    }

    private void HandleInteractableButtonInput()
    {
        inputActions.PlayerActions.A.performed += i => a_input = true;
    }

    private void HanldeJumpInput()
    {
        inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
    }

    private void HandleInventoryInput()
    {
        inputActions.PlayerActions.Start.performed += i => start_Input = true;

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
}