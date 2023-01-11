using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spell/Healing Spell")]
public class HealingSpell : SpellItem
{
    public int healAmount;
    public override void AttemptToCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats)
    {
        GameObject instantiateWarmUpSpellFx = Instantiate(spellWarmUpFX, animatorHandler.transform);
        animatorHandler.PlayTargetAnimation(spellAnimation, true);
        Debug.Log("Casting spell.");
    }
    public override void SuccessfullyCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats)
    {
        GameObject instantiateSpellFX = Instantiate(spellCastFX, animatorHandler.transform);
        Debug.Log(playerStats);
        playerStats.currentHealth += healAmount;
        Debug.Log("Spell casted.");
    }
}
