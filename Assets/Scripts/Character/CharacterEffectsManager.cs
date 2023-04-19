using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    [System.NonSerialized] public WeaponFX rightWeaponFX;
    [System.NonSerialized] public WeaponFX leftWeaponFX;
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
