using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEventManager : MonoBehaviour
{
    [SerializeField] List<FogWall> fogWalls;
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

        foreach(var fogWall in fogWalls)
        {
            fogWall.ActivateFogWall();
        }
    }

    public void BossHasBeenDefeated()
    {
        bossFightIsActive = false;
        bossHasBeenDefeated = true;
        //Deactivate fog walls

        foreach (var fogWall in fogWalls)
        {
            fogWall.DeactivateFogWall();
        }
    }
}
