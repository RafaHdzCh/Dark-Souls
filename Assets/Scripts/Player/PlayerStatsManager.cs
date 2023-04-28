using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
    public HealthBar healthBar;
    public StaminaBar staminaBar;
    public ManaBar manaBar;

    [Header("Scripts")]
    PlayerAnimatorManager playerAnimatorManager;
    PlayerManager playerManager;

    private float staminaRegenerationAmount = 30f;
    private float staminaRegenTimer = 0;


    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
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

    private void Update()
    {
        HandlePoiseResetTimer();
    }
    public override void HandlePoiseResetTimer()
    {
        if (poiseResetTimer > 0)
        {
            poiseResetTimer -= Time.deltaTime;
        }
        else if(poiseResetTimer <= 0 && !playerManager.isInteracting)
        {
            totalPoiseDefence = armorPoiseBonus;
        }
        else
        {
            return;
        }
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

    public override void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation = "Damage") 
    {
        if (playerManager.isInvulnerable) return;
        if (isDead) return;

        float totalPhysicalDamageAbsorption = 1 -
            (1 - physicalDamageAbsortionHead / 100) *
            (1 - physicalDamageAbsortionBody / 100) *
            (1 - physicalDamageAbsortionLegs / 100) *
            (1 - physicalDamageAbsortionHands / 100);
        physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));

        float totalFireDamageAbsorption = 1 -
            (1 - fireDamageAbsortionHead / 100) *
            (1 - fireDamageAbsortionBody / 100) *
            (1 - fireDamageAbsortionLegs / 100) *
            (1 - fireDamageAbsortionHands / 100);
        fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));

        float finalDamage = physicalDamage + fireDamage;
        currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);
        playerAnimatorManager.PlayTargetAnimation(damageAnimation, true);

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            playerAnimatorManager.PlayTargetAnimation(DarkSoulsConsts.DEATH, true);
            isDead = true;
        }
        healthBar.SetCurrentHealth(currentHealth);
    }

    public override void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
    {
        if (isDead) return;

        float totalPhysicalDamageAbsorption = 1 -
            (1 - physicalDamageAbsortionHead / 100) *
            (1 - physicalDamageAbsortionBody / 100) *
            (1 - physicalDamageAbsortionLegs / 100) *
            (1 - physicalDamageAbsortionHands / 100);
        physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));

        float totalFireDamageAbsorption = 1 -
            (1 - fireDamageAbsortionHead / 100) *
            (1 - fireDamageAbsortionBody / 100) *
            (1 - fireDamageAbsortionLegs / 100) *
            (1 - fireDamageAbsortionHands / 100);
        fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));   

        float finalDamage = physicalDamage + fireDamage;
        currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            playerAnimatorManager.PlayTargetAnimation(DarkSoulsConsts.DEATH, true);
            isDead = true;
        }
        healthBar.SetCurrentHealth(currentHealth);
    }
    public override void TakePoisonDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            playerAnimatorManager.PlayTargetAnimation(DarkSoulsConsts.DEATH, true);
            isDead = true;
        }
        healthBar.SetCurrentHealth(currentHealth);
    }

    public void TakeStaminaDamage(int damage)
    {
        currentStamina = currentStamina - damage;
        staminaBar.SetCurrentStamina(currentStamina);
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
