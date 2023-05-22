using UnityEngine;

[CreateAssetMenu(menuName = "Items/Consumables/Bomb")]

public class BombItem : ConsumableItem
{
    [Header("Velocity")]
    public int upwardVelocity = 50;
    public int forwardVelocity = 50;
    public int mass = 1;

    [Header("Liv Bomb Model")]
    public GameObject liveBombModel;

    [Header("Base Damage")]
    public int baseDamage = 200;
    public int explosiveDamage = 75;

    public override void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, PlayerWeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
    {
        if(currentItemAmount > 0)
        {
            weaponSlotManager.rightHandSlot.UnloadWeapon();
            playerAnimatorManager.PlayTargetAnimation(consumableAnimation, true);
            GameObject bombModel = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform.position, Quaternion.identity, weaponSlotManager.rightHandSlot.transform);
            playerEffectsManager.instantiatedFXModel = bombModel;
        }
        else
        {
            playerAnimatorManager.PlayTargetAnimation(DarkSoulsConsts.SHRUGGING, true);
        }
    }
}
