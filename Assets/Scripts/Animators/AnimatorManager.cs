using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    [Header("Components")]
    public Animator anim;
    [System.NonSerialized] public bool canRotate;


    public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false)
    {
        anim.applyRootMotion = isInteracting;
        anim.SetBool(DarkSoulsConsts.CANROTATE, canRotate);
        anim.SetBool(DarkSoulsConsts.ISINTERACTING, isInteracting);
        anim.CrossFade(targetAnim, 0.2f);
    }
    public void PlayTargetAnimationWithRootRotation(string targetAnim, bool isInteracting)
    {
        anim.applyRootMotion = isInteracting;
        anim.SetBool(DarkSoulsConsts.ISROTATINGWITHROOTMOTION, true);
        anim.SetBool(DarkSoulsConsts.ISINTERACTING, isInteracting);
        anim.CrossFade(targetAnim, 0.2f);
    }

    public virtual void TakeCriticalDamageAnimationEvent()
    {

    }
}
