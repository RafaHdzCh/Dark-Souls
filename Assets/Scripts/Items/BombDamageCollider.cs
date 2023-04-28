using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDamageCollider : DamageCollider
{
    [Header("Explosive Damage & radius")]
    [System.NonSerialized] public int explosiveRadius = 1;
    [System.NonSerialized] public int explosionDamage;
    [System.NonSerialized] public int explosionSplashDamage;

    [System.NonSerialized] public int fireExplosionDamage;
    bool hasCollided = false;

    [System.NonSerialized] public Rigidbody bombRigidbody;
    [SerializeField] GameObject impactParticles;

    protected override void Awake()
    {
        damageCollider = GetComponent<Collider>();
        bombRigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!hasCollided)
        {
            hasCollided = true;
            impactParticles = Instantiate(impactParticles, transform.position, Quaternion.identity);
            Explode();

            CharacterStatsManager characterStatsManager = collision.transform.GetComponent<CharacterStatsManager>();
            if(characterStatsManager != null )
            {
                //check for friendly fire.
                characterStatsManager.TakeDamage(0, explosionDamage);
            }

            Destroy(impactParticles, 5f);
            Destroy(transform.parent.parent.gameObject);
        }
    }

    void Explode()
    {
        Collider[] characters = Physics.OverlapSphere(transform.position, explosiveRadius);

        foreach(Collider objectInExplosion in characters)
        {
            CharacterStatsManager characterStatsManager = objectInExplosion.GetComponent<CharacterStatsManager>();

            if(characterStatsManager != null )
            {
                characterStatsManager.TakeDamage(0, explosionSplashDamage);
            }
        }
    }
}
