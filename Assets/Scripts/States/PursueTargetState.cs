using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PursueTargetState : State
{
    [SerializeField] CombatStanceState combatStanceState;
    [SerializeField] RotateTowardsTargetState rotateTowardsTargetState;

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        print("Pursue Target State");
        if (enemyStats.currentHealth <= 0) return null;

        Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.transform.position, Vector3.up);

        HandleRotateTowardsTarget(enemyManager);

        if(viewableAngle > 45 || viewableAngle < -45)
        {
            return rotateTowardsTargetState;
        }

        if (enemyManager.isInteracting)
        {
            return this;
        }
        if (enemyManager.isPerformingAction)
        {
            print("PURSUE: Esta realizando accion");
            enemyAnimatorManager.anim.SetFloat(DarkSoulsConsts.VERTICAL, .5f, 0.1f, Time.deltaTime);
            return this;
        }

        if (distanceFromTarget > enemyManager.maximumAggroRadius)
        {
            print("PURSUE: Distancia superior al limite para atacar");
            enemyAnimatorManager.anim.SetFloat(DarkSoulsConsts.VERTICAL, 1, 0.1f, Time.deltaTime);
            return this;
        }
        
        else if(distanceFromTarget <= enemyManager.maximumAggroRadius)
        {
            print("PURSUE: Combat Stance");
            return combatStanceState;
        }

        else
        {
            print("PURSUE: De nuevo pursue");
            return this;
        }
    }

    private void HandleRotateTowardsTarget(EnemyManager enemyManager)
    {
        //Rotate manually
        if (enemyManager.isPerformingAction)
        {
            Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
        }
        //Rotate with pathfinding (navmesh)
        else
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
            Vector3 targetVelocity = enemyManager.navMeshAgent.desiredVelocity;

            enemyManager.navMeshAgent.enabled = true;
            enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
            enemyManager.enemyRigidbody.velocity = targetVelocity;
            enemyManager.navMeshAgent.angularSpeed = 180;
            enemyManager.transform.rotation = Quaternion.Slerp
            (transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
        }
    }
}
