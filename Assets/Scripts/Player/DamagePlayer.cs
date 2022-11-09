using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    int damage = 25;
    void OnTriggerEnter(Collider other)
    {
        PlayerStats playerStats = other.GetComponent<PlayerStats>();
        if(playerStats != null)
        {
            playerStats.TakeDamage(damage);
        }
    }
}
