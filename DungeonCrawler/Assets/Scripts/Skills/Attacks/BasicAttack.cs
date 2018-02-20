using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : AbstractSkill {

    private int attackDamage = 1;
	// Use this for initialization
	void Start () {
        skillName = "Basic Attack";
        skillDescription = "Deal " + attackDamage + " damage to the target ";

        skillCost = 1;
        skillCooldown = 1;
	}

    public override void performSkill()
    {
        target.UnderAttack(attackDamage);
    }
}
