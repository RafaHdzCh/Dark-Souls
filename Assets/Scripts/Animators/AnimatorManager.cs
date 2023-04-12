using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    protected CharacterManager characterManager;
    protected CharacterStatsManager characterStatsManager;

    [System.NonSerialized] public bool canRotate;

    protected virtual void Awake()
    {
        characterManager = GetComponent<CharacterManager>();
        characterStatsManager = GetComponent<CharacterStatsManager>();
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false)
    {
        animator.applyRootMotion = isInteracting;
        animator.SetBool(DarkSoulsConsts.CANROTATE, canRotate);
        animator.SetBool(DarkSoulsConsts.ISINTERACTING, isInteracting);
        animator.CrossFade(targetAnim, 0.2f);
    }

    public void PlayTargetAnimationWithRootRotation(string targetAnim, bool isInteracting)
    {
        animator.applyRootMotion = isInteracting;
        animator.SetBool(DarkSoulsConsts.ISROTATINGWITHROOTMOTION, true);
        animator.SetBool(DarkSoulsConsts.ISINTERACTING, isInteracting);
        animator.CrossFade(targetAnim, 0.2f);
    }

    public virtual void TakeCriticalDamageAnimationEvent()
    {
        characterStatsManager.TakeDamageNoAnimation(characterManager.pendingCriticalDamage);
        characterManager.pendingCriticalDamage = 0;
    }

    public virtual void CanRotate()
    {
        animator.SetBool(DarkSoulsConsts.CANROTATE, true);
    }

    public virtual void StopRotation()
    {
        animator.SetBool(DarkSoulsConsts.CANROTATE, false);
    }

    public virtual void EnableCombo()
    {
        animator.SetBool(DarkSoulsConsts.CANDOCOMBO, true);
    }

    public virtual void DisableCombo()
    {
        animator.SetBool(DarkSoulsConsts.CANDOCOMBO, false);
    }

    public virtual void EnableIsInvulnerable()
    {
        animator.SetBool(DarkSoulsConsts.ISINVULNERABLE, true);
    }

    public virtual void DisableIsInvulnerable()
    {
        animator.SetBool(DarkSoulsConsts.ISINVULNERABLE, false);
    }

    public void EnableIsParrying()
    {
        characterManager.isParrying = true;
    }

    public void DisableIsParrying()
    {
        characterManager.isParrying = false;
    }

    public void EnableCanBeRiposted()
    {
        characterManager.canBeRiposted = true;
    }

    public void DisableCanBeRiposted()
    {
        characterManager.canBeRiposted = false;
    }
}
