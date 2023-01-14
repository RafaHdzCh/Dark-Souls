using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spell/Healing Spell")]
public class HealingSpell : SpellItem
{
    public int healAmount;
    public override void AttemptToCastSpell(PlayerAnimatorManager animatorHandler, PlayerStats playerStats)
    {
        base.AttemptToCastSpell(animatorHandler, playerStats);
        if(spellWarmUpFX != null)
        {
            GameObject instantiateWarmUpSpellFx = Instantiate(spellWarmUpFX, animatorHandler.transform);
            Destroy(instantiateWarmUpSpellFx, instantiateWarmUpSpellFx.GetComponent<ParticleSystem>().main.duration);
        }
        animatorHandler.PlayTargetAnimation(spellAnimation, true);
        
    }
    public override void SuccessfullyCastSpell(PlayerAnimatorManager animatorHandler, PlayerStats playerStats)
    {
        base.SuccessfullyCastSpell(animatorHandler, playerStats);
        GameObject instantiateSpellFX = Instantiate(spellCastFX, animatorHandler.transform);
        Destroy(instantiateSpellFX, instantiateSpellFX.GetComponent<ParticleSystem>().main.duration);
        playerStats.HealPlayer(healAmount);
    }
}
