using UnityEngine;

[CreateAssetMenu(menuName = "Items/Consumables/Clump")]
public class ClumpItem : ConsumableItem
{
    [Header("Recovery FX")]
    public GameObject clumpFX;

    [Header("Cure FX")]
    public bool curePoison;
    public bool cureBleeding;
    public bool cureFrost;

    public override void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, PlayerWeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
    {
        base.AttemptToConsumeItem(playerAnimatorManager, weaponSlotManager, playerEffectsManager);
        GameObject clump = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform);
        playerEffectsManager.currentParticleFX = clumpFX;
        playerEffectsManager.instantiatedFXModel = clump;

        if(curePoison)
        {
            playerEffectsManager.poisonBuildUp = 0;
            playerEffectsManager.poisonAmount = playerEffectsManager.defaultPoisonAmount;
            playerEffectsManager.isPoisoned = false;

            if(playerEffectsManager.currentParticleFX != null)
            {
                playerEffectsManager.currentParticleFX.SetActive(false);
            }
        }
        weaponSlotManager.rightHandSlot.UnloadWeapon();
    }
}
