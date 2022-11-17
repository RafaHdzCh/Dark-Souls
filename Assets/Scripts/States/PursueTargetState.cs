using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PursueTargetState : State
{
    [SerializeField] CombatStanceState combatStanceState;

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        if (enemyManager.isPerformingAction)
        {
            return this;
        }

        Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);
        float viewableAngle = Vector3.Angle(targetDirection, transform.position);

        if (enemyManager.distanceFromTarget > enemyManager.maximumAttackRange)
        {
            enemyAnimatorManager.anim.SetFloat(DarkSoulsConsts.VERTICAL, 1, 0.1f, Time.deltaTime);
        }
        
        HandleRotateTowardsTarget(enemyManager);
        enemyManager.navMeshAgent.transform.localPosition = Vector3.zero;
        enemyManager.navMeshAgent.transform.localRotation = Quaternion.identity;

        if(enemyManager.distanceFromTarget <= enemyManager.maximumAttackRange)
        {
            return combatStanceState;
        }
        else
        {
            return this;
        }
    }

    private void HandleRotateTowardsTarget(EnemyManager enemyManager)
    {
        //Rotate manually
        if (enemyManager.isPerformingAction)
        {
            print("esta realizando una accion");
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
            print("Rotando por NavMeshAgent");
            Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
            Vector3 targetVelocity = enemyManager.enemyRigidbody.velocity;

            enemyManager.navMeshAgent.enabled = true;
            enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
            enemyManager.enemyRigidbody.velocity = targetVelocity;
            transform.rotation = Quaternion.Slerp
            (transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
        }
        enemyManager.navMeshAgent.angularSpeed = 0;
    }
}
