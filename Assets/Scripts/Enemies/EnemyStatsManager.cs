using UnityEngine;

public class EnemyStatsManager : CharacterStatsManager
{
    [Header("Serializables")]

    public bool isBoss;
    public float walkSpeed = 0.5f;
    public float runSpeed = 1f;

    [Header("Components")]
    [SerializeField] Transform healthBarTransform = null;
    [SerializeField] Transform cameraTransform = null;
    [SerializeField] Rigidbody rigi = null;

    [Header("Scripts")]
    EnemyHealthBar enemyHealthBar;
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyBossManager enemyBossManager;
    EnemyManager enemyManager;

    private void Awake()
    {
        enemyBossManager = GetComponent<EnemyBossManager>();
        if(!isBoss)
        {
            enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();  
        }
        enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
        enemyManager = GetComponent<EnemyManager>();
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
    }

    void Start()
    {
        if (!isBoss)
        {
            enemyHealthBar.SetMaxHealth(maxHealth);
        }
    }

    public override void HandlePoiseResetTimer()
    {
        base.HandlePoiseResetTimer();
    }

    private void Update()
    {
        HandlePoiseResetTimer();
        if (!isBoss)
        {
            healthBarTransform.LookAt(cameraTransform);
        }
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public override void TakeDamage(int damage, string damageAnimation = "Damage")
    {
        if (isDead) return;
        if (enemyManager.isPhaseShifting) return;

        currentHealth = currentHealth - damage;
        if (!isBoss)
        {
            enemyHealthBar.SetHealth(currentHealth);
        }
        else
        {
            enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
        }

        enemyAnimatorManager.PlayTargetAnimation(DarkSoulsConsts.DAMAGE, true);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            HandleDeath();
        }
    }

    public override void TakeDamageNoAnimation(int damage)
    {
        if (isDead) return; 
        if (enemyManager.isPhaseShifting) return;

        currentHealth = currentHealth - damage;
        if (!isBoss)
        {
            enemyHealthBar.SetHealth(currentHealth);
        }
        else if(isBoss && enemyBossManager!= null)
        {
            enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            HandleDeath();
        }
    }

    public override void TakePoisonDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
        enemyHealthBar.SetHealth(currentHealth);
    }

    public void BreakGuard()
    {
        enemyAnimatorManager.PlayTargetAnimation(DarkSoulsConsts.BREAKGUARD, true);
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
