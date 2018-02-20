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
        skillDescription = "Slam your opponent for massive damage! Cooldown of: " + skillCooldown;
	}

    public override void performSkill()
    {
        if (!skillOnCooldown())
        {
            target.UnderAttack(attackDamage);
            turnsUntilOffCD = 2;
        } else
        {
            // "Skill is on cooldown!"
            //Notify player somehow!
        }
    }
}
