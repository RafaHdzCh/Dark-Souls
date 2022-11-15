using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    [Header("Components")]
    public Animator anim;
    //Ejecuta la animacion que llegue por parametro.
    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        anim.applyRootMotion = isInteracting;
        anim.SetBool(DarkSoulsConsts.ISINTERACTING, isInteracting);
        anim.CrossFade(targetAnim, 0.2f);
    }
}
