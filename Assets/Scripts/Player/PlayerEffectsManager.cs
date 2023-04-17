using UnityEngine;

public class PlayerEffectsManager : CharacterEffectsManager
{
    PlayerStatsManager playerStatsManager;
    PlayerWeaponSlotManager playerWeaponSlotManager;
    [System.NonSerialized] public GameObject currentParticleFX;
    [System.NonSerialized] public GameObject instantiatedFXModel;
    [System.NonSerialized] public int amountToBeHealed;

    private void Awake()
    {
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
    }
    public void HealPlayerFromffect()
    {
        GameObject healParticles = Instantiate(currentParticleFX, playerStatsManager.transform);
        float destroyTime = healParticles.GetComponent<ParticleSystem>().main.duration;

        Destroy(instantiatedFXModel.gameObject, destroyTime);
        Destroy(healParticles, destroyTime);
        playerWeaponSlotManager.LoadBothWaponsOnSlots();
    }
}
