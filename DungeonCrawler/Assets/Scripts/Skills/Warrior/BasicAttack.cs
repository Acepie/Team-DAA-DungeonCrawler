using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : AbstractSkill
{

    public Sprite skillIcon;

    // Use this for initialization
    void Start()
    {
        skillName = "Basic Attack";
        skillDescription = "A basic attack that deals damage to the target";
        skillCost = 1;
        skillCooldown = 1;
        skillRadius = 2;
    }

    protected override bool performSkill(List<AbstractCreature> targets, CombatData data)
    {
        if (targets.Count != 1) {
            skillOnUseText = "Can only attack 1 target";
            return false;
        }
        int damage = data.attackpower;
        skillOnUseText = "Hit for " + damage + " damage";
        targets[0].UnderAttack(damage);
        return true;
    }
}
