using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingCollider : MonoBehaviour
{
    public BoxCollider blockingCollider;
    [System.NonSerialized] public float blockingPhysicalDamageAbsorbtion;

    private void Awake()
    {
        blockingCollider= GetComponent<BoxCollider>();
    }

    public void SetColliderDamageAbsortion(WeaponItem weapon)
    {
        if (weapon == null) return;
        blockingPhysicalDamageAbsorbtion = weapon.physicalDamageAbsortion;
    }

    public void EnableBlockingCollider()
    {
        blockingCollider.enabled = true;
    }
    public void DisableBlockingCollider()
    {
        blockingCollider.enabled = false;
    }
}
