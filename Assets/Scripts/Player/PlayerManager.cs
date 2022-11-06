using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    InputHandler inputHandler;
    Animator anim;
    CameraHandler cameraHandler;
    PlayerLocomotion playerLocomotion;

    [Header("PlayerFlags")]
    public bool isInteracting;
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;
    public bool canDoCombo;

    void Start()
    {
        cameraHandler = FindObjectOfType<CameraHandler>();
        inputHandler = GetComponent<InputHandler>(); 
        anim = GetComponentInChildren<Animator>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    void Update()
    {
        float delta = Time.deltaTime;

        isInteracting = anim.GetBool(DarkSoulsConsts.ISINTERACTING);
        canDoCombo = anim.GetBool(DarkSoulsConsts.CANDOCOMBO);

        inputHandler.TickInput(delta);
        playerLocomotion.HandleMovement(delta);
        playerLocomotion.HandleRollingAndSprinting(delta);
        playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);

        CheckForInteractablObjects();
    }

    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;
        if (cameraHandler != null)
        {
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
        }
    }
    private void LateUpdate()
    {
        inputHandler.rollFlag = false;
        inputHandler.sprintFlag = false;
        inputHandler.rb_Input = false;
        inputHandler.rt_Input = false;
        inputHandler.d_pad_Up = false;
        inputHandler.d_pad_Down = false;
        inputHandler.d_pad_Left = false;
        inputHandler.d_pad_Right = false;
        inputHandler.a_input = false;

        if(isInAir)
        {
            playerLocomotion.inAirTimer += Time.deltaTime;
        }
    }

    public void CheckForInteractablObjects()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
        {
            if(hit.collider.CompareTag(DarkSoulsConsts.INTERACTABLE))
            {
                Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                if(interactableObject != null)
                {
                    string interactableText = interactableObject.interactableText;
                    //Set the UI Txt to the interactable obects txt
                    //Set thee text pop up to true

                    if(inputHandler.a_input)
                    {
                        hit.collider.GetComponent<Interactable>().Interact(this);
                    }
                }
            }
        }
    }
}
