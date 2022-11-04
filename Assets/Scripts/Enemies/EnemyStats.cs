using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    AnimatorHandler animatorHandler;

    private void Awake()
    {
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
    }

    void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;
        animatorHandler.PlayTargetAnimation(DarkSoulsConsts.DAMAGE, true);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animatorHandler.PlayTargetAnimation(DarkSoulsConsts.DEATH, true);
            //Handle player death
        }
    }
}
