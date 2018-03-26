using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlamAttack : AbstractSkill
{
    [SerializeField]
    private float attackBonus;

    // Use this for initialization
    void Start()
    {
        attackBonus = 0.5f;
        skillName = "Slam";
        skillCooldown = 2;
        skillCost = 1;
        skillDescription = "Slam your opponent for massive damage!\nCooldown: " + skillCooldown;
        skillRadius = 2;
    }

    protected override bool performSkill(List<AbstractCreature> targets, CombatData data)
    {
        if (targets.Count != 1) {
            skillOnUseText = "Can only attack 1 target";
            return false;
        }

        int damage = data.AttackPower + (int)(data.AttackPower * attackBonus);
        skillOnUseText = "Slammed for " + damage + " damage!";
        targets[0].UnderAttack(damage);
        return true;
    }
}
