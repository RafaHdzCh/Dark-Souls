using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEventManager : MonoBehaviour
{
    [SerializeField] UIBossHealthBar bossHealthBar;
    [SerializeField] EnemyBossManager enemyBossManager;

    public bool bossFightIsActive;      //Is currently fighting
    public bool bossHasBeenAwakened;    //Watched the cutscene
    public bool bossHasBeenDefeated;

    public void ActivateBossFight()
    {
        bossFightIsActive = true;
        bossHasBeenAwakened = true;
        bossHealthBar.SetUIHealthBarToActive();
        //Activate fog wall
    }

    public void BossHasBeenDefeated()
    {
        bossFightIsActive = false;
        bossHasBeenDefeated = true;
        //Deactivate fog walls
    }
}
