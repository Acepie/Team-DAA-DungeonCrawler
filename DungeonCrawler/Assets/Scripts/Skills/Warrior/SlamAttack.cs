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
        skillRadius = 2;
    }

    protected override bool performSkill(List<AbstractCreature> targets, CombatData data)
    {
        if (targets.Count != 1) {
            skillOnUseText = "Can only attack 1 target";
            return false;
        }

        int damage = attackDamage + data.attackpower;
        skillOnUseText = "Slammed for " + damage + " damage!";
        targets[0].UnderAttack(damage);
        return true;
    }
}
