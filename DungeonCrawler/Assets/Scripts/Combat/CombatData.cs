using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CombatData
{
    [SerializeField]
    private int currentHealth;
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int attackPower;
    [SerializeField]
    private int rawAttackPower;
    [SerializeField]
    private int temporaryHealth;

    public int CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    public int MaxHealth {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

    public int AttackPower
    {
        get { return attackPower; }
        set { attackPower = value; }
    }
    
    public int RawAttackPower
    {
        get { return rawAttackPower; }
    }

    public int TemporaryHealth
    {
        get { return temporaryHealth; }
        set { temporaryHealth = value; }
    }

    public CombatData(int health, int attack)
    {
        currentHealth = maxHealth = health;
        attackPower = rawAttackPower = attack;
        temporaryHealth = 0;
    }


}
