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
    public bool canBeRiposted;
    public bool canBeParried;
    public bool isParrying;
    //damage will be inflicted during an animation event
    //used during backstab
    public int pendingCriticalDamage;


}
