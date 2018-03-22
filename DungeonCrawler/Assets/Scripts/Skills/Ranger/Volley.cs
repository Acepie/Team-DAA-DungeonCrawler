using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volley : AbstractSkill
{

    // Use this for initialization
    void Start()
    {
        skillName = "Volley";
        skillDescription = "A basic attack that deals damage to all targets";
        skillCost = 1;
        skillCooldown = 2;
        skillRadius = 10;
    }

    protected override bool performSkill(List<AbstractCreature> targets, CombatData data)
    {
        int damage = data.attackpower;
        skillOnUseText = "Hit all targets for " + damage + " damage";
        targets.ForEach((t) =>
        {
            t.UnderAttack(damage);
        });
        return true;
    }
}
