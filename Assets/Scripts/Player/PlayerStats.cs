using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    public HealthBar healthBar;
    public StaminaBar staminaBar;
    public ManaBar manaBar;

    [Header("Scripts")]
    PlayerAnimatorManager animatorHandler;
    PlayerManager playerManager;

    private float staminaRegenerationAmount = 30f;
    private float staminaRegenTimer = 0;


    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        animatorHandler = GetComponentInChildren<PlayerAnimatorManager>();
    }

    void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetCurrentHealth(currentHealth);

        maxStamina = SetMaxStaminaFromStaminaLevel();
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
        staminaBar.SetCurrentStamina(currentStamina);

        maxMana = SetMaxManaFromManaLevel();
        currentMana = maxMana;
        manaBar.SetMaxMana(maxMana);
        manaBar.SetCurrentMana(currentMana);
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    private float SetMaxStaminaFromStaminaLevel()
    {
        maxStamina = staminaLevel * 10;
        return maxStamina;
    }

    private float SetMaxManaFromManaLevel()
    {
        maxMana = manaLevel * 10;
        return maxMana;
    }
    public override void TakeDamage(int damage, bool playAnimation)
    {
        if (playerManager.isInvulnerable) return;
        if (isDead) return;


        currentHealth = currentHealth - damage;
        healthBar.SetCurrentHealth(currentHealth);
        if(playAnimation)
        {
            animatorHandler.PlayTargetAnimation(DarkSoulsConsts.DAMAGE, true);
        }

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            if (playAnimation)
            {
                animatorHandler.PlayTargetAnimation(DarkSoulsConsts.DEATH, true);
            }
            isDead = true;
        }
    }

    public void TakeStaminaDamage(int damage)
    {
        currentStamina = currentStamina - damage;
        staminaBar.SetCurrentStamina(currentStamina);
    }

    public void TakeDamageNoAnimation(int damage)
    {
        currentHealth = currentHealth - damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
    }

    public void RegenerateStamina()
    {
        if(playerManager.isInteracting)
        {
            staminaRegenTimer = 0;
        }
        else
        {
            staminaRegenTimer += Time.deltaTime;
            if (currentStamina < maxStamina && staminaRegenTimer > 1f)
            {
                currentStamina += staminaRegenerationAmount * Time.deltaTime;
                staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
            }
        }
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth = currentHealth + healAmount;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        healthBar.SetCurrentHealth(currentHealth);
    }

    public void DeductMana(int manaAmount)
    {
        currentMana = currentMana - manaAmount;

        if(currentMana < 0)
        {
            currentMana = 0;
        }

        manaBar.SetCurrentMana(currentMana);
    }

    public void AddSouls(int souls)
    {
        soulCount = soulCount + souls;
    }
}
