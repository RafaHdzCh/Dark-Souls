using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : Item
{
    [Header("Item Quantity")]
    public int maxItemAmount;
    public int currentItemAmount;

    [Header("Item Model")]
    public GameObject itemModel;

    [Header("Item Animations")]
    public string consumableAnimation;
    public bool isInteracting;

    public virtual void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, WeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
    {
        if(currentItemAmount > 0)
        {
            playerAnimatorManager.PlayTargetAnimation(consumableAnimation, isInteracting);
        }
        else
        {
            playerAnimatorManager.PlayTargetAnimation(DarkSoulsConsts.SHRUGGING, true);
        }
    }
}
