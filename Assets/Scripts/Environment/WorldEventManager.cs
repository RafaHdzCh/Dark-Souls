using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEventManager : MonoBehaviour
{
    [SerializeField] List<FogWall> fogWalls;
    [SerializeField] List<Collider> entranceColliders;
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

        foreach(Collider entranceTrigger in entranceColliders)
        {
            entranceTrigger.enabled = false;
        }
    }

    public void BossHasBeenDefeated()
    {
        bossFightIsActive = false;
        bossHasBeenDefeated = true;

        foreach (var fogWall in fogWalls)
        {
            fogWall.DeactivateFogWall();
        }
        foreach (Collider entranceTrigger in entranceColliders)
        {
            entranceTrigger.enabled = true;
        }
    }
}
