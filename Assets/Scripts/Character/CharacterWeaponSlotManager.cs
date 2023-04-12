using UnityEngine;

public class CharacterWeaponSlotManager : MonoBehaviour
{
    [Header("Unnarmed ScriptableObject")]
    public WeaponItem unarmedWeapon;

    [Header("Attacking Weapon")]
    [System.NonSerialized] public WeaponItem attackingWeapon;

    [Header("Weapon Slots")]
    [System.NonSerialized] public WeaponHolderSlot leftHandSlot;
    [System.NonSerialized] public WeaponHolderSlot rightHandSlot;
    [System.NonSerialized] public WeaponHolderSlot backSlot;

    [Header("Damage Colliders")]
    [System.NonSerialized] public DamageCollider leftHandDamageCollider;
    [System.NonSerialized] public DamageCollider rightHandDamageCollider;

    protected virtual void Awake()
    {
        LoadWeaponHolderSlot();
    }

    private void LoadWeaponHolderSlot()
    {
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
        {
            if (weaponSlot.isLeftHandSlot)
            {
                leftHandSlot = weaponSlot;
            }
            else if (weaponSlot.isRightHandSlot)
            {
                rightHandSlot = weaponSlot;
            }
            else if (weaponSlot.isBackSlot)
            {
                backSlot = weaponSlot;
            }
        }
    }
}
