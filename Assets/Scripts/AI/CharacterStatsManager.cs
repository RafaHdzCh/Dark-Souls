using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    [Header("Serializables")]
    [Header("Health")]
    [System.NonSerialized] public int healthLevel = 10;
    [System.NonSerialized] public int maxHealth;
    [System.NonSerialized] public int currentHealth;
    [System.NonSerialized] public int maxHalth;

    [Header("Stamina")]
    [System.NonSerialized] public int staminaLevel = 10;
    [System.NonSerialized] public float maxStamina;
    [System.NonSerialized] public float currentStamina;

    [Header("Mana")]
    [System.NonSerialized] public float manaLevel = 10;
    [System.NonSerialized] public float maxMana;
    [System.NonSerialized] public float currentMana;

    [Header("Souls")]
    [System.NonSerialized] public int soulCount = 0;
    public int soulsAwardedOnDeath = 50;

    [Header("Poise")]
    public float totalPoiseDefence;
    public float offensivePoiseBonus;
    public float armorPoiseBonus;
    public float totalPoiseResetTime = 15;
    public float poiseResetTimer = 0;

    [Header("Armor Absortions")]
    public float physicalDamageAbsortionHead;
    public float physicalDamageAbsortionBody;
    public float physicalDamageAbsortionLegs;
    public float physicalDamageAbsortionHands;

    [HideInInspector] public bool isDead;

    private void Start()
    {
        totalPoiseDefence = armorPoiseBonus;
    }

    public virtual void TakeDamage(int damage, string damageAnimation = "Damage")
    {

    }

    public virtual void TakeDamageNoAnimation(int damage)
    {
        
    }

    public virtual void HandlePoiseResetTimer()
    {
        if(poiseResetTimer > 0)
        {
            poiseResetTimer -= Time.deltaTime;
        }
        else
        {
            totalPoiseDefence = armorPoiseBonus;
        }
    }
}