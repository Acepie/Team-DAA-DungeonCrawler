using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRanged : AbstractSkill {

    public Sprite skillIcon;

    // Use this for initialization
    void Start()
    {
        skillName = "Basic Ranged Attack";
        skillDescription = "A basic ranged attack that deals damage to the target";
        skillCost = 1;
        skillCooldown = 1;
        skillRadius = 10;
    }

    protected override bool performSkill(List<AbstractCreature> targets, CombatData data)
    {
        if (targets.Count != 1)
        {
            skillOnUseText = "Can only attack 1 target";
            return false;
        }
        int damage = data.AttackPower;
        skillOnUseText = "Hit for " + damage + " damage";
        targets[0].UnderAttack(damage);
        return true;
    }
}
