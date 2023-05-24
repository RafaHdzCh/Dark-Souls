using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("Serializable")]
    public Transform lockOnTransform;
    public CriticalDamageCollider backstabCollider;
    public CriticalDamageCollider riposteCollider;

    [Header("Combat Flags")]
    [System.NonSerialized] public bool canBeRiposted;
    [System.NonSerialized] public bool canBeParried;
    [System.NonSerialized] public bool isParrying;
    [System.NonSerialized] public bool isBlocking;
    [System.NonSerialized] public bool isInvulnerable;
    [System.NonSerialized] public bool canDoCombo;
    [System.NonSerialized] public bool isUsingRightHand;
    [System.NonSerialized] public bool isUsingLeftHand;
    [System.NonSerialized] public bool isTwoHanding;

    [Header("Movement Flags")]
    [System.NonSerialized] public bool isRotatingWithRootMotion;
    [System.NonSerialized] public bool canRotate;
    [System.NonSerialized] public bool isInteracting;
    [System.NonSerialized] public bool isSprinting;
    [System.NonSerialized] public bool isInAir;
    [System.NonSerialized] public bool isGrounded;

    [Header("Spells")]
    [System.NonSerialized] public bool isFiringSpell;

    [System.NonSerialized] public int pendingCriticalDamage;
}
