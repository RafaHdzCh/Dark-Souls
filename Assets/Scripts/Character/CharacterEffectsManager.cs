using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    public WeaponFX rightWeaponFX;
    public WeaponFX leftWeaponFX;
    public virtual void PlayWeaponFX(bool isLeft)
    {
        if(!isLeft)
        {
            if (rightWeaponFX == null) return;

            rightWeaponFX.PlayWeaponFX();
        }
        else
        {
            if (leftWeaponFX == null) return;

            leftWeaponFX.PlayWeaponFX();
        }
    }
}
