using UnityEngine;

public class PlayerAnimatorManager : AnimatorManager
{
    [Header("Variables")]
    int vertical;
    int horizontal;

    PlayerLocomotionManager playerLocomotionManager;
    PlayerManager playerManager;
    PlayerStatsManager playerStatsManager;
    InputHandler inputHandler;

    public void Initialize()
    {
        inputHandler = GetComponent<InputHandler>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        vertical = Animator.StringToHash(DarkSoulsConsts.VERTICAL);
        horizontal = Animator.StringToHash(DarkSoulsConsts.HORIZONTAL);
        playerManager = GetComponent<PlayerManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
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

        animator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
    }

    public void CanRotate()
    {
        animator.SetBool(DarkSoulsConsts.CANROTATE, true);
    }

    public void StopRotation()
    {
        animator.SetBool(DarkSoulsConsts.CANROTATE, false);
    }

    public void EnableCombo()
    {
        animator.SetBool(DarkSoulsConsts.CANDOCOMBO, true);
    }

    public void DisableCombo()
    {
        animator.SetBool(DarkSoulsConsts.CANDOCOMBO, false);
    }

    public void EnableIsInvulnerable()
    {
        animator.SetBool(DarkSoulsConsts.ISINVULNERABLE, true);
    }
    public void DisableIsInvulnerable()
    {
        animator.SetBool(DarkSoulsConsts.ISINVULNERABLE, false);
    }

    public void EnableIsParrying()
    {
        playerManager.isParrying = true;
    }

    public void DisableIsParrying()
    {
        playerManager.isParrying = false;
    }

    public void EnableCanBeRiposted()
    {
        playerManager.canBeRiposted = true;
    }
    public void DisableCanBeRiposted()
    {
        playerManager.canBeRiposted = false;
    }
    public override void TakeCriticalDamageAnimationEvent()
    {
        playerStatsManager.TakeDamageNoAnimation(playerManager.pendingCriticalDamage);
        playerManager.pendingCriticalDamage = 0;
    }

    public void DisableCollision()
    {
        playerLocomotionManager.characterCollider.enabled = false;
        playerLocomotionManager.characterCollisionBlocker.enabled = false;
    }

    public void EnableCollision()
    {
        playerLocomotionManager.characterCollider.enabled = true;
        playerLocomotionManager.characterCollisionBlocker.enabled = true;
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
        playerLocomotionManager.GetComponent<Rigidbody>().drag = 0;
        Vector3 deltaPosition = animator.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        playerLocomotionManager.GetComponent<Rigidbody>().velocity = velocity;
    }
}