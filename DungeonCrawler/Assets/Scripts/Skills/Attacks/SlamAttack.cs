using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlamAttack : AbstractSkill {

    int attackDamage = 3;

	// Use this for initialization
	void Start () {
        skillName = "Slam!";
        skillCooldown = 2;
        skillCost = 1;
        skillOnUseText = "Slammed for " + attackDamage + " damage!";
        skillDescription = "Slam your opponent for massive damage! Cooldown of: " + skillCooldown;
	}

    protected override bool performSkill(AbstractCreature target)
    {
        if (!skillOnCooldown())
        {
            target.UnderAttack(attackDamage);
            turnsUntilOffCD = 2;
            return true;
        } else
        {
            return false;
        }
    }
}
