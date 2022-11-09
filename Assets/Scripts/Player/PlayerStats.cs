using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Serializables")]
    [Header("Health")]
    public int healthLevel = 10;
    [HideInInspector] public int maxHealth;
    [HideInInspector] public int currentHealth;
    public HealthBar healthBar;

    [Header("Stamina")]
    public int staminaLevel = 10;
    [HideInInspector] public int maxStamina;
    [HideInInspector] public int currentStamina;
    public StaminaBar staminaBar;

    [Header("Scripts")]
    AnimatorHandler animatorHandler;

    private void Awake()
    {
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
    }

    void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        maxStamina = SetMaxStaminaFromStaminaLevel();
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    private int SetMaxStaminaFromStaminaLevel()
    {
        maxStamina = staminaLevel * 10;
        return maxStamina;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;
        healthBar.SetCurrentHealth(currentHealth);
        animatorHandler.PlayTargetAnimation(DarkSoulsConsts.DAMAGE, true);

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            animatorHandler.PlayTargetAnimation(DarkSoulsConsts.DEATH, true);
            //Handle player death
        }
    }

    public void TakeStaminaDamage(int damage)
    {
        currentStamina = currentStamina - damage;
        staminaBar.SetCurrentStamina(currentStamina);
    }
}
