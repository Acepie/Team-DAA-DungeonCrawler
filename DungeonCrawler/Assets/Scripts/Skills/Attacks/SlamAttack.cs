using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlamAttack : AbstractSkill
{

    public int attackDamage;

    // Use this for initialization
    void Start()
    {
        skillName = "Slam!";
        skillCooldown = 2;
        skillCost = 1;
        skillDescription = "Slam your opponent for massive damage! Cooldown of: " + skillCooldown;
    }

    protected override bool performSkill(AbstractCreature target, PlayerData data)
    {
        if (!skillOnCooldown())
        {
            int damage = attackDamage + data.attackpower;
            skillOnUseText = "Slammed for " + damage + " damage!";
            target.UnderAttack(damage);
            turnsUntilOffCD = 2;
            return true;
        }
        else
        {
            return false;
        }
    }
}
