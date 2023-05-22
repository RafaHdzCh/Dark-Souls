using UnityEngine;

public class PlayerEffectsManager : CharacterEffectsManager
{
    [System.NonSerialized] public int amountToBeHealed;
    
    [System.NonSerialized] public GameObject currentParticleFX;
    [System.NonSerialized] public GameObject instantiatedFXModel;

    [Header("Scripts")]
    PlayerStatsManager playerStatsManager;
    PlayerWeaponSlotManager playerWeaponSlotManager;
    [SerializeField] PoisonBuildUpBar poisonBuildUpBar;
    [SerializeField] PoisonAmountBar poisonAmountBar;

    protected override void Awake()
    {
        base.Awake();
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

    protected override void HandlePoisonBuildUp()
    {
        if(poisonBuildUp <= 0)
        {
            poisonBuildUpBar.gameObject.SetActive(false);
        }
        else
        {
            poisonBuildUpBar.gameObject.SetActive(true);
        }
        poisonBuildUpBar.SetPoisonBuildUpAmount(Mathf.RoundToInt(poisonBuildUp));
        base.HandlePoisonBuildUp();
    }

    protected override void HandlePoisonedEffect()
    {
        if(!isPoisoned)
        {
            poisonAmountBar.gameObject.SetActive(false);
        }
        else
        {
            poisonAmountBar.gameObject.SetActive(true);
        }
        base.HandlePoisonedEffect();
        poisonAmountBar.SetPoisonAmount(Mathf.RoundToInt(poisonAmount));
    }
}
