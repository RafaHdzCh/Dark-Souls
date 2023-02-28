using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{
    [System.NonSerialized] public bool isInteractingStatus = false;
    [System.NonSerialized] public bool isFiringSpellStatus = false;
    [System.NonSerialized] public bool canRotateStatus = true;
    [System.NonSerialized] public bool isRotatingWithRootMotionStatus = false;

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(DarkSoulsConsts.ISINTERACTING, isInteractingStatus);
        animator.SetBool(DarkSoulsConsts.ISFIRINGSPELL, isFiringSpellStatus);
        animator.SetBool(DarkSoulsConsts.CANROTATE, canRotateStatus);
        animator.SetBool(DarkSoulsConsts.ISROTATINGWITHROOTMOTION, isRotatingWithRootMotionStatus);
    }

}
