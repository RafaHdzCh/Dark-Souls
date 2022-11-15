using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Serializables")]
    [Header("Health")]
    public int healthLevel = 10;
    [HideInInspector] public int maxHealth;
    [HideInInspector] public int currentHealth;

    [Header("Stamina")]
    public int staminaLevel = 10;
    [HideInInspector] public int maxStamina;
    [HideInInspector] public int currentStamina;
}
