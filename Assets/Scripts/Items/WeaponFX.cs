using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFX : MonoBehaviour
{
    [Header("Weapon FX")]
    [SerializeField] ParticleSystem normalWeaponTrail;
    //fireWeaponTrail
    //...

    public void PlayWeaponFX()
    {
        normalWeaponTrail.Stop();

        if(normalWeaponTrail.isStopped)
        {
            normalWeaponTrail.Play();
        }
    }
}
