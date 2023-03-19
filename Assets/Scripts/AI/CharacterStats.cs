using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Serializables")]
    [Header("Health")]
    [System.NonSerialized] public int healthLevel = 10;
    [System.NonSerialized] public int maxHealth;
    [System.NonSerialized] public int currentHealth;

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

    [Header("Poise")]
    public float totalPoiseDefence;
    public float offnsivePoiseBonus;
    public float armorPoiseBonus;
    public float totalPoiseResetTime = 15;
    public float poiseResetTimer = 0;

    [Header("Armor Absortions")]
    public float physicalDamageAbsortionHead;
    public float physicalDamageAbsortionBody;
    public float physicalDamageAbsortionLegs;
    public float physicalDamageAbsortionHands;


    [HideInInspector] public bool isDead;

    protected virtual void Update()
    {
        HandlePoiseResetTimer();
    }

    private void Start()
    {
        totalPoiseDefence = armorPoiseBonus;
    }

    public virtual void TakeDamage(int damage, string damageAnimation = "Damage")
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
