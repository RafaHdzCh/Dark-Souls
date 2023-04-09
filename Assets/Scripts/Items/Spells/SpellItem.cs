using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class SpellItem : Item
{ 
    public GameObject spellWarmUpFX;
    public GameObject spellCastFX;
    public string spellAnimation;

    [Header("Spell Type")]
    public bool isFaithSpell;
    public bool isMagicSpell;
    public bool isPyroSpell;

    [Header("Spell Cost")]
    public int manaCost;

    [Header("Spell Description")]
    [TextArea]
    public string spellDescription;

    public virtual void AttemptToCastSpell(PlayerAnimatorManager animatorHandler, 
                                            PlayerStatsManager playerStats, 
                                            PlayerWeaponSlotManager weaponSlotManager)
    {

    }
    public virtual void SuccessfullyCastSpell(PlayerAnimatorManager animatorHandler,
                                                PlayerStatsManager playerStats,
                                                PlayerWeaponSlotManager weaponSlotManager,
                                                CameraHandler cameraHandler)
    {
        playerStats.DeductMana(manaCost);
    }
}
