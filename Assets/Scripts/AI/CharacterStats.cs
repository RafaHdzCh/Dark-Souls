using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Serializables")]
    [Header("Health")]
    [HideInInspector] public int healthLevel = 10;
    [HideInInspector] public int maxHealth;
    [HideInInspector] public int currentHealth;

    [Header("Stamina")]
    [HideInInspector] public int staminaLevel = 10;
    [HideInInspector] public float maxStamina;
    [HideInInspector] public float currentStamina;

    [Header("Mana")]
    [HideInInspector] public float manaLevel = 10;
    [HideInInspector] public float maxMana;
    [HideInInspector] public float currentMana;

    [Header("Souls")]
    [HideInInspector] public int soulCount = 0;

    [HideInInspector] public bool isDead;

    public virtual void TakeDamage(int damage, string damageAnimation = "Damage")
    {

    }
}
