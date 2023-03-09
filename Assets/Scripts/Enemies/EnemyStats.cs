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
    [SerializeField] Rigidbody rigi;

    [Header("Scripts")]
    EnemyHealthBar enemyHealthBar;
    EnemyAnimatorManager enemyAnimatorManager;


    public int soulsAwardedOnDeath = 50;

    private void Awake()
    {
        enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();  
        enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
    }

    void Start()
    {
        enemyHealthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        healthBarTransform.LookAt(cameraTransform);
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public void TakeDamageNoAnimation(int damage)
    {
        currentHealth = currentHealth - damage;

        enemyHealthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
    }

    public override void TakeDamage(int damage, string damageAnimation = "Damage")
    {
        if (isDead) return;

        currentHealth = currentHealth - damage;
        enemyHealthBar.SetHealth(currentHealth);

        enemyAnimatorManager.PlayTargetAnimation(DarkSoulsConsts.DAMAGE, true);

        if (currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    private void HideHealthBarOnDeath()
    {
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

    private void HandleDeath()
    {
        currentHealth = 0;
        DeactivateCollidersOnDeath();
        
        enemyAnimatorManager.PlayTargetAnimation(DarkSoulsConsts.DEATH, true);
        Invoke(nameof(HideHealthBarOnDeath), 3f);
        isDead = true;
    }
}
