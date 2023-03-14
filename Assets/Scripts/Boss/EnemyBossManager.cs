using UnityEngine;

public class EnemyBossManager : MonoBehaviour
{
    public string bossname = "";
    [SerializeField] UIBossHealthBar bossHealthBar;
    EnemyStats enemyStats;
    EnemyAnimatorManager enemyAnimatorManager;
    BossCombatStanceState bossCombatStanceState;

    private void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();
        enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
        bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
    }

    private void Start()
    {
        bossHealthBar.SetBossName(bossname);
        bossHealthBar.SetBossMaxHealth(enemyStats.maxHealth);
    }

    public void UpdateBossHealthBar(int currentHealth, int maxHealth)
    {
        bossHealthBar.SetBossCurrentHealth(currentHealth);
        if (currentHealth <= maxHealth / 2 && !bossCombatStanceState.hasPhaseShifted)
        {
            bossCombatStanceState.hasPhaseShifted = true;
            ShiftToSecondPhase();
        }
    }

    public void ShiftToSecondPhase()
    {
        enemyAnimatorManager.anim.SetBool(DarkSoulsConsts.ISINVULNERABLE, true);
        enemyAnimatorManager.anim.SetBool(DarkSoulsConsts.ISPHASESHIFTING, true);
        enemyAnimatorManager.PlayTargetAnimation(DarkSoulsConsts.PHASESHIFT, true);
    }
}
