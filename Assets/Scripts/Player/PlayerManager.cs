using UnityEngine;

public class PlayerManager : CharacterManager
{
    InputHandler inputHandler;
    Animator anim;
    CameraHandler cameraHandler;
    PlayerLocomotion playerLocomotion;
    PlayerStats playerStats;
    PlayerAnimatorManager playerAnimatorManager;
    [System.NonSerialized] public Interactable interactableObject;

    InteractableUI interactableUI;
    public GameObject interactableUIGameObject;
    public GameObject itemInteractableGameObject;

    [Header("PlayerFlags")]
    [System.NonSerialized] public bool isInteracting;
    [System.NonSerialized] public bool isSprinting;
    [System.NonSerialized] public bool isInAir;
    [System.NonSerialized] public bool isGrounded;
    [System.NonSerialized] public bool canDoCombo;
    [System.NonSerialized] public bool isUsingRightHand;
    [System.NonSerialized] public bool isUsingLeftHand;

    void Start()
    {
        cameraHandler = FindObjectOfType<CameraHandler>();
        backstabCollider = GetComponentInChildren<CriticalDamageCollider>();
        inputHandler = GetComponent<InputHandler>(); 
        anim = GetComponentInChildren<Animator>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        interactableUI = FindObjectOfType<InteractableUI>();
        playerStats = GetComponent<PlayerStats>();
        playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
    }

    void Update()
    {
        float delta = Time.deltaTime;

        //Se establecen los booleanos segun lo que indique la animacion.
        isInteracting = anim.GetBool(DarkSoulsConsts.ISINTERACTING);
        isFiringSpell = anim.GetBool(DarkSoulsConsts.ISFIRINGSPELL);
        canDoCombo = anim.GetBool(DarkSoulsConsts.CANDOCOMBO);
        anim.SetBool(DarkSoulsConsts.ISINAIR, isInAir);
        anim.SetBool(DarkSoulsConsts.ISDEAD, playerStats.isDead);

        anim.SetBool(DarkSoulsConsts.ISBLOCKING, isBlocking);

        isUsingRightHand = anim.GetBool(DarkSoulsConsts.ISUSINGRIGHTHAND);
        isUsingLeftHand = anim.GetBool(DarkSoulsConsts.ISUSINGLEFTHAND);
        isInvulnerable = anim.GetBool(DarkSoulsConsts.ISINVULNERABLE);
        playerAnimatorManager.canRotate = anim.GetBool(DarkSoulsConsts.CANROTATE);

        //Detecta los inputs del control del jugador.
        inputHandler.TickInput(delta);
        playerLocomotion.HandleRollingAndSprinting(delta);
        playerStats.RegenerateStamina();

        if(interactableObject != null && inputHandler.a_input)
        {
            interactableObject.GetComponent<Interactable>().Interact(this);
        }
    }

    private void FixedUpdate()
    {
        float delta = Time.deltaTime;

        //Detecta el movimiento y/o caida del personaje.
        playerLocomotion.HandleMovement(delta);
        playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
        playerLocomotion.HandleRotation(delta);
    }

    private void LateUpdate()
    {
        //Se establece el bool de cada boton como falso al final de cada frame para que pueda volver a usarse en el siguiente.
        inputHandler.rollFlag = false;

        inputHandler.a_input = false;
        inputHandler.b_input = false;
        inputHandler.x_input = false;
        inputHandler.y_input = false;

        inputHandler.rb_Input = false;
        inputHandler.rt_Input = false;

        inputHandler.lt_Input = false;

        inputHandler.d_pad_Up = false;
        inputHandler.d_pad_Down = false;
        inputHandler.d_pad_Left = false;
        inputHandler.d_pad_Right = false;

        inputHandler.start_Input = false;
        inputHandler.select_Input = false;

        inputHandler.right_Stick_Press = false;
        inputHandler.left_Stick_Press = false;

        //Si hay un cameraHandler...
        if (cameraHandler != null)
        {
            //Se sigue al pivote a lo largo del tiempo.
            cameraHandler.FollowTarget(Time.deltaTime);
            //Se envia por parametro el movimiento del mouse en el tiempo.
            cameraHandler.HandleCameraRotation(Time.deltaTime, inputHandler.mouseX, inputHandler.mouseY);
        }

        //Si estas en el aire (estas cayendo)...
        if (isInAir)
        {
            //Se aumenta el temporizador del tiempo que pasaste en el aire.
            playerLocomotion.inAirTimer += Time.deltaTime;
        }
    }

    #region Player Interactions

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(DarkSoulsConsts.INTERACTABLE))
        {
            //Se obitene su componente "Interactable"
            interactableObject = other.GetComponent<Interactable>();

            //Si este script no es nulo...
            if (interactableObject != null)
            {
                //Se obtiene componente de tipo texto y se muestra por UI.
                string interactableText = interactableObject.interactableText;
                interactableUI.interactableText.text = interactableText;
                interactableUIGameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(DarkSoulsConsts.INTERACTABLE))
        {
            interactableObject = null;
            if (interactableUIGameObject != null)
            {
                //Se desactiva el gameobject de UI del objeto interactuable.
                interactableUIGameObject.SetActive(false);
            }
            if (itemInteractableGameObject != null && inputHandler.a_input)
            {
                //Se desactiva el gameobject del objeto interactuable.
                itemInteractableGameObject.SetActive(false);
            }
        }
    }

    public void OpenChestInteraction(Transform playerStandHereWhenOpeningChest)
    {
        playerLocomotion.rigi.velocity = Vector3.zero;
        transform.position = playerStandHereWhenOpeningChest.transform.position;
        playerAnimatorManager.PlayTargetAnimation(DarkSoulsConsts.OPENCHEST, true);

    }

    public void PassThroughFogWallInteraction(Transform fowWallEntrance)
    {
        playerLocomotion.rigi.velocity = Vector3.zero;
        Vector3 rotationDirection = fowWallEntrance.transform.forward;
        Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
        transform.rotation = turnRotation;

        playerAnimatorManager.PlayTargetAnimation(DarkSoulsConsts.PASSTHROUGHFOGWALL, true);
    }

    #endregion
}
