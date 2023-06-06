using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorldEventManager : MonoBehaviour
{
    [SerializeField] List<FogWall> fogWalls;
    [SerializeField] List<Collider> entranceColliders;
    [SerializeField] UIBossHealthBar bossHealthBar;

    [System.NonSerialized] public bool bossFightIsActive;      //Is currently fighting
    [System.NonSerialized] public bool bossHasBeenAwakened;    //Watched the cutscene
    [System.NonSerialized] public bool bossHasBeenDefeated;

    public void ActivateBossFight()
    {
        bossFightIsActive = true;
        bossHasBeenDefeated = false;
        bossHasBeenAwakened = true;
        bossHealthBar.SetUIHealthBarToActive();

        foreach(var fogWall in fogWalls)
        {
            fogWall.ActivateFogWall();
        }

        foreach(Collider entranceTrigger in entranceColliders)
        {
            entranceTrigger.enabled = false;
        }
    }

    public void BossHasBeenDefeated(Collider entranceCollider)
    {
        bossFightIsActive = false;
        bossHasBeenDefeated = true;
        bossHasBeenAwakened = false;
        bossHealthBar.SetUIHealthBarToInactive();
        Destroy(entranceCollider.gameObject);

        foreach (var fogWall in fogWalls)
        {
            if(fogWall != null)
            {
                fogWall.DeactivateFogWall();
            }
        }
        foreach (Collider entranceTrigger in entranceColliders)
        {
            if(entranceTrigger != null)
            {
                entranceTrigger.enabled = true;
            }
        }
    }
}
