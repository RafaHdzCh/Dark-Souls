using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Health")]
    [HideInInspector] public int maxHealth;
    [HideInInspector] public int currentHealth;

    [Header("Serializables")]

    [Header("Components")]
    [SerializeField] Transform healthBarTransform;
    [SerializeField] Transform cameraTransform;
    [SerializeField] GameObject healthBarGO;

    [Header("Variables")]
    public int healthLevel = 10;

    [Header("Scripts")]
    AnimatorHandler animatorHandler;
    HealthBar _healthBar;

    private void Awake()
    {
        _healthBar = GetComponentInChildren<HealthBar>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
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

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;
        _healthBar.SetCurrentHealth(currentHealth);
        animatorHandler.PlayTargetAnimation(DarkSoulsConsts.DAMAGE, true);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animatorHandler.PlayTargetAnimation(DarkSoulsConsts.DEATH, true);
            Invoke(nameof(HideHealthBarOnDeath), 3f);
            //Handle player death
        }
    }

    private void HideHealthBarOnDeath()
    {
        healthBarGO.SetActive(false);
    }
}
