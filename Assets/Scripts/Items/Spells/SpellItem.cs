using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellItem : MonoBehaviour
{
    public GameObject spellWarmUpFX;
    public GameObject spellCastFX;
    public string spellAnimation;

    [Header("Spell Type")]
    public bool isFaithSpell;
    public bool isMagicSpell;
    public bool isPyroSpell;

    [Header("Spell Description")]
    [TextArea]
    public string spellDescription;

    public virtual void AttemptToCastSpell()
    {
        print("You attempted to cast a spell");
    }
    public virtual void SuccessfullyCastSpell()
    {
        print("You succesfully casted a spell");
    }
}
