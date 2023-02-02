using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "A.I./Enemy Actions/Attack Actions")]
public class EnemyAttackAction : EnemyAction
{
    public bool canCombo;

    public EnemyAttackAction comboAction;

    public int attackScore = 3;
    public float recoveryTime = 2f;

    public float maximumAttackAngle = 35f;
    public float minimumAttackAngle = -35f;

    public float minimumDistanceNeededToAttack = 0;
    public float maximumAttackDistanceToAttack = 3;
}
