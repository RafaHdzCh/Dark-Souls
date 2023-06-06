using UnityEngine;

public class EnemyBossManager : MonoBehaviour
{
    public string bossname = "";
    [SerializeField] UIBossHealthBar bossHealthBar;
    EnemyAnimatorManager enemyAnimatorManager;
    BossCombatStanceState bossCombatStanceState;
    [SerializeField] WorldEventManager worldEventManager;
    [SerializeField] Collider entranceCollider;

    private void Awake()
    {
        enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
        bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
    }

    public void UpdateBossHealthBar(int currentHealth, int maxHealth)
    {
        bossHealthBar.SetBossCurrentHealth(currentHealth);
        if (currentHealth <= maxHealth / 2 && !bossCombatStanceState.hasPhaseShifted)
        {
            bossCombatStanceState.hasPhaseShifted = true;
            ShiftToSecondPhase();
        }
        if(currentHealth <= 0)
        {
            worldEventManager.BossHasBeenDefeated(entranceCollider);
        }
    }

    public void ShiftToSecondPhase()
    {
        enemyAnimatorManager.animator.SetBool(DarkSoulsConsts.ISINVULNERABLE, true);
        enemyAnimatorManager.animator.SetBool(DarkSoulsConsts.ISPHASESHIFTING, true);
        enemyAnimatorManager.PlayTargetAnimation(DarkSoulsConsts.PHASESHIFT, true);
    }
}
