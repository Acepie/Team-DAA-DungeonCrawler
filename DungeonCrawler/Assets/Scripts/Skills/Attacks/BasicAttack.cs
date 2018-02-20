using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : AbstractSkill
{

    // Use this for initialization
    void Start()
    {
        skillName = "Basic Attack";
        skillDescription = "A basic attack that deals damage to the target";
        skillCost = 1;
        skillCooldown = 1;
    }

    protected override bool performSkill(AbstractCreature target, PlayerData data)
    {
        int damage = data.attackpower;
        skillOnUseText = "Hit for " + damage + " damage";
        target.UnderAttack(damage);
        return true;
    }
}
