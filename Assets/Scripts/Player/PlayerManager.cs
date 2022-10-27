using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputHandler inputHandler;
    Animator anim;
    void Start()
    {
        inputHandler = GetComponent<InputHandler>(); 
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        inputHandler.isInteracting = anim.GetBool(DarkSoulsConsts.ISINTERACTING);
        inputHandler.rollFlag = false;
    }
}
