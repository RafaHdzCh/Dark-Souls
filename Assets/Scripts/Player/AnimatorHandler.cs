using UnityEngine;

public class AnimatorHandler : AnimatorManager
{
    [Header("Variables")]
    [HideInInspector] public bool canRotate;
    int vertical;
    int horizontal;

    [Header("Scripts")]
    [HideInInspector] PlayerLocomotion playerLocomotion;
    [HideInInspector] PlayerManager playerManager;

    public void Initialize()
    {
        playerLocomotion = GetComponentInParent<PlayerLocomotion>();
        vertical = Animator.StringToHash(DarkSoulsConsts.VERTICAL);
        horizontal = Animator.StringToHash(DarkSoulsConsts.HORIZONTAL);
        playerManager = GetComponentInParent<PlayerManager>();
    }

    //Establecer la velocidad de movimiento en el Animator.
    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
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

        if(isSprinting)
        {
            v = 2;
            h = horizontalMovement;
        }

        anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
    }

    public void CanRotate()
    {
        canRotate = true;
    }

    public void StopRotation()
    {
        canRotate = false;
    }

    public void EnableCombo()
    {
        anim.SetBool(DarkSoulsConsts.CANDOCOMBO, true);
    }

    public void DisableCombo()
    {
        anim.SetBool(DarkSoulsConsts.CANDOCOMBO, false);
    }

    public void OnAnimatorMove()
    {
        if(this.gameObject.CompareTag("Enemy"))
        {

        }
        else if (playerManager.isInteracting == false) return;
        else
        {
            Move();
        }
    }

    public void Move()
    {
        float delta = Time.deltaTime;
        playerLocomotion.GetComponent<Rigidbody>().drag = 0;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        playerLocomotion.GetComponent<Rigidbody>().velocity = velocity;
    }
}