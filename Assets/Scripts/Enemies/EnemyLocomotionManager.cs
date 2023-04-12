using UnityEngine;

public class EnemyLocomotionManager : MonoBehaviour
{
    public CapsuleCollider characterCollider;
    public CapsuleCollider characterCollisionBlocker;

    private void Start()
    {
        Physics.IgnoreCollision(characterCollider, characterCollisionBlocker, true);
    }
}
