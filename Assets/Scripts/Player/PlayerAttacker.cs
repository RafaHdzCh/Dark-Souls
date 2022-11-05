using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacker : MonoBehaviour
{
    AnimatorHandler animatorHandler;
    InputHandler inputHandler;
    public string lastAttack;

    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if(inputHandler.comboFlag)
        {
            animatorHandler.anim.SetBool(DarkSoulsConsts.CANDOCOMBO, false);
            if (lastAttack == weapon.OH_Light_Attack_0)
            {
                animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
            }
            if (lastAttack == weapon.OH_Heavy_Attack_0)
            {
                animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
            }
        }
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_0, true);
        lastAttack = weapon.OH_Light_Attack_0;
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_0, true);
        lastAttack = weapon.OH_Heavy_Attack_0;
    }
}
