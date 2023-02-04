using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spell/Projectile Spell")]
public class ProjectileSpell : SpellItem
{
    public float baseDamage;
    public float projectileVelocity;
    Rigidbody rigi;

    public override void AttemptToCastSpell(PlayerAnimatorManager animatorHandler, 
                                            PlayerStats playerStats, 
                                            WeaponSlotManager weaponSlotManager)
    {
        base.AttemptToCastSpell(animatorHandler, playerStats, weaponSlotManager);
        GameObject instantiatedWarmUpSpellVFX = Instantiate(spellWarmUpFX, weaponSlotManager.rightHandSlot.transform);
        instantiatedWarmUpSpellVFX.gameObject.transform.localScale = new Vector3(100, 100, 100);
        animatorHandler.PlayTargetAnimation(spellAnimation, true);
        //Instantiate the spell in the casting hand
        //play animation to cast the spell
    }

    public override void SuccessfullyCastSpell(PlayerAnimatorManager animatorHandler, PlayerStats playerStats, WeaponSlotManager weaponSlotManager)
    {
        base.SuccessfullyCastSpell(animatorHandler, playerStats, weaponSlotManager);
    }
}
