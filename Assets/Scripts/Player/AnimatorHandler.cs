using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    public Animator anim;
    int vertical;
    int horizontal;
    public bool canRotate;
    public InputHandler inputHandler;
    public PlayerLocomotion playerLocomotion;

    public void Initialize()
    {
        anim = GetComponent<Animator>();
        inputHandler = GetComponentInParent<InputHandler>(); 
        playerLocomotion = GetComponentInParent<PlayerLocomotion>();
        vertical = Animator.StringToHash(DarkSoulsConsts.VERTICAL);
        horizontal = Animator.StringToHash(DarkSoulsConsts.HORIZONTAL);
    }

    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement)
    {
        #region Vertical
        float v = 0;

        if(verticalMovement > 0 && verticalMovement < 0.55f)
        {
            v = 0.5f;
        }
        else if (verticalMovement>0.55f)
        {
            v = 1;
        }
        else if(verticalMovement < 0 && verticalMovement > -0.55f)
        {
            v = -0.5f;
        }
        else if(verticalMovement < -0.55f)
        {
            v = -1;
        }
        else
        {
            v = 0;
        }
        #endregion
        #region Horizontal
        float h = 0;

        if(horizontalMovement > 0 && horizontalMovement < 0.55f)
        {
            h = 0.5f;
        }
        else if( horizontalMovement > 0.55f)
        {
            h = 1;
        }
        else if( horizontalMovement < 0 && horizontalMovement > -0.55f)
        {
            h = -0.5f;
        }
        else if(horizontalMovement < -0.55f)
        {
            h = -1;
        }
        else
        {
            h = 0;
        }
        #endregion

        anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        anim.applyRootMotion = isInteracting;
        anim.SetBool(DarkSoulsConsts.ISINTERACTING, isInteracting);
        anim.CrossFade(targetAnim, 0.2f);
    }

    public void CanRotate()
    {
        canRotate = true;
    }

    public void StopRotation()
    {
        canRotate = false;
    }

    public void OnAnimatorMove()
    {
        if (inputHandler.isInteracting == false) return;
        float delta = Time.deltaTime;
        playerLocomotion.rigidbody.drag = 0;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        playerLocomotion.rigidbody.velocity = velocity;
    }
}
