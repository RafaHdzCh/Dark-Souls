using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Consumables/Flask")]
public class FlaskItem : ConsumableItem
{
    [Header("Flask Type")]
    public bool estusFlask;
    public bool ashenFlask;

    [Header("Recovery Amount")]
    public int healthRecoveryAmount;
    public int manaRecoveryAmount;

    [Header("Recovery FX")]
    public GameObject recoveryFX;

    public override void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, WeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
    {
        base.AttemptToConsumeItem(playerAnimatorManager, weaponSlotManager, playerEffectsManager);
        GameObject flask = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform);
        playerEffectsManager.currentParticleFX = recoveryFX;
        playerEffectsManager.amountToBeHealed = healthRecoveryAmount;
        playerEffectsManager.instantiatedFXModel = flask;
        weaponSlotManager.rightHandSlot.UnloadWeapon();

        //play recovery fx if we drink

    }
}
