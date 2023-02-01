using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using static TreeEditor.TreeGroup;

public class EnemyStats : CharacterStats
{
    [Header("Serializables")]
    [Header("Components")]
    [SerializeField] Transform healthBarTransform;
    [SerializeField] Transform cameraTransform;
    [SerializeField] GameObject healthBarGO;
    [SerializeField] Rigidbody rigi;

    [Header("Scripts")]
    EnemyAnimatorManager enemyAnimatorManager;
    HealthBar _healthBar;


    public int soulsAwardedOnDeath = 50;

    private void Awake()
    {
        _healthBar = GetComponentInChildren<HealthBar>();
        enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
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
            enemyAnimatorManager.PlayTargetAnimation(DarkSoulsConsts.DAMAGE, true);
        }

        if (currentHealth <= 0)
        {
            HandleDeath(playAnimation);
        }
    }

    private void HideHealthBarOnDeath()
    {
        healthBarGO.SetActive(false);
        FindObjectOfType<InputHandler>().lockOnFlag = false;
        FindObjectOfType<CameraHandler>().ClearLockOnTargets();
    }

    private void DeactivateCollidersOnDeath()
    {
        CapsuleCollider mainCollider = gameObject.GetComponent<CapsuleCollider>();
        Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();

        mainCollider.enabled = false;
        foreach(Collider collider in colliders)
        {
            collider.enabled = false;
        }
        rigi.isKinematic = true;
    }

    private void HandleDeath(bool playAnimation)
    {
        currentHealth = 0;
        DeactivateCollidersOnDeath();
        
        if (playAnimation)
        {
            enemyAnimatorManager.PlayTargetAnimation(DarkSoulsConsts.DEATH, true);
        }
        Invoke(nameof(HideHealthBarOnDeath), 3f);
        isDead = true;
    }
}
