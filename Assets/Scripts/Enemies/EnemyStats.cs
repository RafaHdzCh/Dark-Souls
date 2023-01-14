using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    [Header("Serializables")]
    [Header("Components")]
    [SerializeField] Transform healthBarTransform;
    [SerializeField] Transform cameraTransform;
    [SerializeField] GameObject healthBarGO;


    [Header("Scripts")]
    EnemyAnimatorManager animatorHandler;
    HealthBar _healthBar;

    private void Awake()
    {
        _healthBar = GetComponentInChildren<HealthBar>();
        animatorHandler = GetComponent<EnemyAnimatorManager>();
    }

    void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        healthBarTransform.LookAt(cameraTransform);
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        _healthBar.SetMaxHealth(maxHealth);
        return maxHealth;
    }

    public void TakeDamageNoAnimation(int damage)
    {
        currentHealth = currentHealth - damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Invoke(nameof(HideHealthBarOnDeath), 3f);
            isDead = true;
        }
    }

    public void TakeDamage(int damage, bool playAnimation)
    {
        if (isDead) return;

        currentHealth = currentHealth - damage;

        _healthBar.SetCurrentHealth(currentHealth);
        if(playAnimation)
        {
            animatorHandler.PlayTargetAnimation(DarkSoulsConsts.DAMAGE, true);
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            if(playAnimation)
            {
                animatorHandler.PlayTargetAnimation(DarkSoulsConsts.DEATH, true);
            }
            Invoke(nameof(HideHealthBarOnDeath), 3f);
            isDead = true;
        }
    }

    private void HideHealthBarOnDeath()
    {
        healthBarGO.SetActive(false);
    }
}
