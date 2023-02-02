using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
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

    [System.NonSerialized] public int pendingCriticalDamage;


}
