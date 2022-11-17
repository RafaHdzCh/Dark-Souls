using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : AnimatorManager
{
    EnemyManager enemyManager;
    private void Start()
    {
        enemyManager = GetComponentInParent<EnemyManager>();
    }

    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        enemyManager.enemyRigidbody.drag = 0;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        enemyManager.enemyRigidbody.velocity = velocity;
    }
}
