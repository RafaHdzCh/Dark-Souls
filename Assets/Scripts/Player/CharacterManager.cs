using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("Serializable")]
    public Transform lockOnTransform;
    public BoxCollider backStabBoxCollider;
    [HideInInspector] public BackstabCollider backstabCollider;
}
