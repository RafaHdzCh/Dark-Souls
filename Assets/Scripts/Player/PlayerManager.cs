using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : CharacterManager
{
    InputHandler inputHandler;
    Animator anim;
    CameraHandler cameraHandler;
    PlayerLocomotion playerLocomotion;

    InteractableUI interactableUI;
    public GameObject interactableUIGameObject;
    public GameObject itemInteractableGameObject;

    [Header("PlayerFlags")]
    [HideInInspector] public bool isInteracting;
    [HideInInspector] public bool isSprinting;
    [HideInInspector] public bool isInAir;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool canDoCombo;

    void Start()
    {
        cameraHandler = FindObjectOfType<CameraHandler>();
        inputHandler = GetComponent<InputHandler>(); 
        anim = GetComponentInChildren<Animator>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        interactableUI = FindObjectOfType<InteractableUI>();
    }

    void Update()
    {
        float delta = Time.deltaTime;

        //Se establecen los booleanos segun lo que indique la animacion.
        isInteracting = anim.GetBool(DarkSoulsConsts.ISINTERACTING);
        canDoCombo = anim.GetBool(DarkSoulsConsts.CANDOCOMBO);
        anim.SetBool(DarkSoulsConsts.ISINAIR, isInAir);

        //Detecta los inputs del control del jugador.
        inputHandler.TickInput(delta);
        playerLocomotion.HandleRollingAndSprinting(delta);
        playerLocomotion.HandleJumping();

        //Busca si estas cerca de objetos interactuables como puerats o items.
        CheckForInteractableObjects();
    }

    private void FixedUpdate()
    {
        float delta = Time.deltaTime;

        //Detecta el movimiento y/o caida del personaje.
        playerLocomotion.HandleMovement(delta);
        playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
    }

    private void LateUpdate()
    {
        //Se establece el bool de cada boton como falso al final de cada frame para que pueda volver a usarse en el siguiente.
        inputHandler.rollFlag = false;
        inputHandler.rb_Input = false;
        inputHandler.rt_Input = false;
        inputHandler.d_pad_Up = false;
        inputHandler.d_pad_Down = false;
        inputHandler.d_pad_Left = false;
        inputHandler.d_pad_Right = false;
        inputHandler.a_input = false;
        inputHandler.jump_Input = false;
        inputHandler.start_Input = false;
        inputHandler.lockOnInput = false;

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

    public void CheckForInteractableObjects()
    {
        Vector3 rayOrigin = transform.position;
        rayOrigin.y = rayOrigin.y + 2f;
        RaycastHit hit;

        //Si generamos una esfera en la posicion del personaje...
        if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers)
            || Physics.SphereCast(rayOrigin, 0.5f, Vector3.down, out hit, 2.5f, cameraHandler.ignoreLayers))
        {
            //Si la detecta un collider con la etiqueta "Interactable"...
            if(hit.collider.CompareTag(DarkSoulsConsts.INTERACTABLE))
            {
                //Se obitene su componente "Interactable"
                Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                //Si este script no es nulo...
                if(interactableObject != null)
                {
                    //Se obtiene componente de tipo texto y se muestra por UI.
                    string interactableText = interactableObject.interactableText;
                    interactableUI.interactableText.text = interactableText;
                    interactableUIGameObject.SetActive(true);

                    //Si se presiona el boton de Interaccion...
                    if(inputHandler.a_input)
                    {
                        //Ejecutamos del script interactable la funcion de Interactuar.
                        hit.collider.GetComponent<Interactable>().Interact(this);
                    }
                }
            }
            //Si el collider no tiene la etiqueta "Interactable"...
            else
            {
                if (interactableUIGameObject != null)
                {
                    //Se desactiva el gameobject de UI del objeto interactuable.
                    interactableUIGameObject.SetActive(false);
                }
                if(itemInteractableGameObject != null && inputHandler.a_input)
                {
                    //Se desactiva el gameobject del objeto interactuable.
                    itemInteractableGameObject.SetActive(false);
                }
            }
        }
    }
}
