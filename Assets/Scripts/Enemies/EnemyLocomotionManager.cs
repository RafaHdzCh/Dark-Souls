using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLocomotionManager : MonoBehaviour
{
    [HideInInspector] public CharacterStats currentTarget;
    EnemyManager enemyManager;
    public LayerMask detectionLayer;
    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();   
    }

    //Busca colliders en una capa especifica. Si los encuentra y son del equipo contrario,
    // y estan en su campo de vista, obtiene su CharacterStats y lo vuelve su objetivo.
    public void HandleDetection()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position,enemyManager.detectionRadius, detectionLayer);
    
        for(int i = 0; i < colliders.Length; i++)
        {
            CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

            if(characterStats != null)
            {
                //Check for TEAM ID
                Vector3 targetDirection = characterStats.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if(viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                {
                    currentTarget = characterStats;
                }
            }
        }
    }
}
