using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    int damage = 25;

    //Aplica dano al jugador al entrar en contacto con este Collider Trigger.
    void OnTriggerEnter(Collider other)
    {
        PlayerStats playerStats = other.GetComponent<PlayerStats>();
        if(playerStats != null)
        {
            playerStats.TakeDamage(damage, true);
        }
    }
}
