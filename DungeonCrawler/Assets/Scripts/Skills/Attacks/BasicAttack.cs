using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : AbstractSkill {

    private int attackDamage = 1;
	// Use this for initialization
	void Start () {
        skillName = "Basic Attack";
        skillDescription = "Deal " + attackDamage + " damage to the target ";
        skillOnUseText = "Hit for " + attackDamage + " damage";
        skillCost = 1;
        skillCooldown = 1;
	}

    protected override bool performSkill(AbstractCreature target)
    {
        target.UnderAttack(attackDamage);
        return true;
    }
}
