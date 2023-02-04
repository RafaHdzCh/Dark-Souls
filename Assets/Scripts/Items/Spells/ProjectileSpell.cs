using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spell/Projectile Spell")]
public class ProjectileSpell : SpellItem
{
    [Header("Projectile Damage")]
    public float baseDamage;

    [Header("Projectile Physics")]
    public float projectileForwardVelocity;
    public float projectileUpwardVelocity;
    public float projectileMass;
    public bool isEffectedByGravity;
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

    public override void SuccessfullyCastSpell(PlayerAnimatorManager animatorHandler, 
                                                PlayerStats playerStats, 
                                                WeaponSlotManager weaponSlotManager,
                                                CameraHandler cameraHandler)
    {
        base.SuccessfullyCastSpell(animatorHandler, playerStats, weaponSlotManager, cameraHandler);
        GameObject instantiateSpellVFX = Instantiate(spellCastFX, weaponSlotManager.rightHandSlot.transform.position, cameraHandler.cameraPivotTransform.transform.rotation);
        rigi = instantiateSpellVFX.GetComponent<Rigidbody>();
        //SpellDamageCollider sdc = instantiateSpellVFX.GetComponent<SpellDamagCollider>();
        if(cameraHandler.currentLockOnTarget != null)
        {
            instantiateSpellVFX.transform.LookAt(cameraHandler.currentLockOnTarget.transform);
        }
        else
        {
            instantiateSpellVFX.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x, playerStats.transform.eulerAngles.y, 0);
        }

        rigi.AddForce(instantiateSpellVFX.transform.forward * projectileForwardVelocity);
        rigi.AddForce(instantiateSpellVFX.transform.up * projectileUpwardVelocity);
        rigi.useGravity = isEffectedByGravity;
        rigi.mass = projectileMass;
        instantiateSpellVFX.transform.parent = null;
    }
}
