using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsManager : MonoBehaviour
{
    PlayerStats playerStats;
    WeaponSlotManager weaponSlotManager;
    [System.NonSerialized] public GameObject currentParticleFX;
    [System.NonSerialized] public GameObject instantiatedFXModel;
    [System.NonSerialized] public int amountToBeHealed;

    private void Awake()
    {
        playerStats = GetComponentInParent<PlayerStats>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
    }
    public void HealPlayerFromffect()
    {
        GameObject healParticles = Instantiate(currentParticleFX, playerStats.transform);
        float destroyTime = healParticles.GetComponent<ParticleSystem>().main.duration;

        Destroy(instantiatedFXModel.gameObject, destroyTime);
        Destroy(healParticles, destroyTime);
        weaponSlotManager.LoadBothWaponsOnSlots();
    }
}
