using UnityEngine;

public class BombDamageCollider : DamageCollider
{
    [Header("Explosive Damage & radius")]
    [System.NonSerialized] public int explosiveRadius = 2;
    [System.NonSerialized] public int explosionDamage = 20;
    [System.NonSerialized] public int explosionSplashDamage = 10;

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
                if(characterStatsManager.teamIDNumber != teamIDNumber)
                {
                    characterStatsManager.TakeDamage(0, explosionDamage);
                }
            }

            Destroy(impactParticles, 2f);
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
                if(characterStatsManager.teamIDNumber != teamIDNumber)
                {
                    characterStatsManager.TakeDamage(0, explosionSplashDamage);
                }
            }
        }
    }
}
