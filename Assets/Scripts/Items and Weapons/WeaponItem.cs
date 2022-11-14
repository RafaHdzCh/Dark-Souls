using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{
    public GameObject modelPrefab;
    public bool isUnarmed;

    [Header("Attack Animations")]

    [Header("Idle Animations")]
    public string Right_Arm_Idle;
    public string Left_Arm_Idle;
    public string TH_Idle;
    [Header("Light Attack")]
    public string OH_Light_Attack_0;
    public string OH_Light_Attack_1;
    [Header("Heavy Attack")]
    public string OH_Heavy_Attack_0;
    public string OH_Heavy_Attack_1;


    [Header("Stamina Cost")]
    public int baseStamina;
    public float lightAttackMultiplier;
    public float heavyAttackMultiplier;
}
