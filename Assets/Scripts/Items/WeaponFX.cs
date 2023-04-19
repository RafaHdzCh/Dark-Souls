using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFX : MonoBehaviour
{
    [Header("Weapon FX")]
    [SerializeField] ParticleSystem normalWeaponTrail;
    //fireWeaponTrail
    //...
    private void Start()
    {
        StopTrail();
    }
    public void PlayWeaponFX()
    {
        StopTrail();

        if(normalWeaponTrail.isStopped)
        {
            normalWeaponTrail.Play();
        }

        Invoke(nameof(StopTrail), normalWeaponTrail.main.duration);
    }

    private void StopTrail()
    {
        normalWeaponTrail.Stop();
    }
}
