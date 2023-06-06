using UnityEngine;

public class EventColliderBeginBossFight : MonoBehaviour
{
    [SerializeField] WorldEventManager worldEventManager; 
    [SerializeField] EnemyStatsManager enemyStats;
    [SerializeField] EnemyBossManager enemyBossManager;
    [SerializeField] UIBossHealthBar bossHealthBar;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(DarkSoulsConsts.CHARACTER))
        {
            if(!worldEventManager.bossFightIsActive)
            {
                bossHealthBar.SetBossName(enemyBossManager.bossname);
                bossHealthBar.SetBossMaxHealth(enemyStats.maxHealth);
                worldEventManager.ActivateBossFight();
            }
        }
    }
}
