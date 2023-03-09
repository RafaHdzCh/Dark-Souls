using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossManager : MonoBehaviour
{
    string bossname = "Boss";
    [SerializeField] UIBossHealthBar bossHealthBar;
    EnemyStats enemyStats;

    private void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();
    }

    private void Start()
    {
        bossHealthBar.SetBossName(bossname);
        bossHealthBar.SetBossMaxHealth(enemyStats.maxHealth);
    }
}
