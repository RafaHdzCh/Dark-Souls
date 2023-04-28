using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{
    public GameObject modelPrefab;
    public bool isUnarmed;

    [Header("Damage")]
    public int physicalDamage;
    public int fireDamage;
    public int criticalDamageMultiplier = 3;

    [Header("Poise")]
    public float poiseBreak;
    public float offensivePoiseBonus;

    [Header("Absorption")]
    public float physicalDamageAbsortion;

    [Header("Idle Animations")]
    public string Right_Arm_Idle;
    public string Left_Arm_Idle;
    public string TH_Idle;

    [Header("Attack Animations")]

    [Header("One Hand Attacks")]
    [Header("Light Attack")]
    public string OH_Light_Attack_0;
    public string OH_Light_Attack_1;
    [Header("Heavy Attack")]
    public string OH_Heavy_Attack_0;
    public string OH_Heavy_Attack_1;

    [Header("Two Hands Attack")]
    [Header("Light Attack")]
    public string TH_Light_Attack_0;
    public string TH_Light_Attack_1;
    [Header("Heavy Attack")]
    public string TH_Heavy_Attack_0;
    public string TH_Heavy_Attack_1;

    [Header("Stamina Cost")]
    public int baseStamina;
    public float lightAttackMultiplier;
    public float heavyAttackMultiplier;

    [Header("Weapon Type")]
    public bool isSpellCaster;
    public bool isFaithCaster;
    public bool isPyroCaster;
    public bool isMeleeWeapon;
    public bool isShieldWeapon;

    [Header("Weapon Art")]
    public string Weapon_Art;
}
