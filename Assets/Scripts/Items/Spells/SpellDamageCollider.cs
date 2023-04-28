using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDamageCollider : DamageCollider
{
    [SerializeField] private GameObject impactParticles = null;
    [SerializeField] private GameObject projectileParticles = null;

    bool hasCollided = false;

    private void Start()
    {
        if(projectileParticles != null)
        {
            projectileParticles = Instantiate(projectileParticles, transform.position, transform.rotation);
            projectileParticles.transform.parent = transform;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasCollided)
        {
            print(collision);
            EnemyStatsManager enemyStats = collision.transform.GetComponent<EnemyStatsManager>();
            if (enemyStats != null)
            {
                if(enemyStats.isBoss)
                {
                    enemyStats.TakeDamageNoAnimation(0, fireDamage);
                }
                else
                {
                    enemyStats.TakeDamage(0, fireDamage);
                }
            }
            hasCollided = true;
            impactParticles = Instantiate(impactParticles, transform);
            impactParticles.transform.parent = null;
            impactParticles.transform.rotation = new Quaternion(0, 0, 0, 0);

            Destroy(projectileParticles);
            Destroy(impactParticles, impactParticles.GetComponent<ParticleSystem>().main.duration);
            Destroy(gameObject, 2f);
        }
    }
}
