using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDamageCollider : DamageCollider
{
    [SerializeField] private GameObject impactParticles = null;
    [SerializeField] private GameObject projectileParticles = null;

    bool hasCollided = false;

    CharacterStats spellTarget;
    Rigidbody rigi;

    private void Awake()
    {
        rigi = GetComponent<Rigidbody>();
    }
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
            spellTarget = collision.transform.GetComponent<CharacterStats>();
            if (spellTarget != null)
            {
                spellTarget.TakeDamage(currentWeaponDamage, true);
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
