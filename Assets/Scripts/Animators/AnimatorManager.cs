using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    [System.NonSerialized] public bool canRotate;


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

    }
}
