using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLocomotionManager : MonoBehaviour
{
    public CapsuleCollider characterCollider;
    public CapsuleCollider characterCollisionBlocker;

    private void Start()
    {
        Physics.IgnoreCollision(characterCollider, characterCollisionBlocker, true);
    }
}
