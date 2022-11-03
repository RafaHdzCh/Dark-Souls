using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    [SerializeField] AnimatorHandler animatorHandler;

    public void HandleLightAttack(WeaponItem weapon)
    {
        print(weapon.OH_Light_Attack_1);
        animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        print(weapon.OH_Heavy_Attack_1);
        animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
    }
}
