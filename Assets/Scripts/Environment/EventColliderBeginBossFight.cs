using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventColliderBeginBossFight : MonoBehaviour
{
    [SerializeField] WorldEventManager worldEventManager;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(DarkSoulsConsts.PLAYER))
        {
            worldEventManager.ActivateBossFight();
        }
    }
}
