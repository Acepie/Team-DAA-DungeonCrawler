using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : AbstractSkill {


    // Use this for initialization
    void Start () {
        skillName = "Fireball";
        skillDescription = "Hurl a small ball of fire towards your foes";
        skillCost = 1;
        skillCooldown = 1;
        skillRadius = 10;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    protected override bool performSkill(List<AbstractCreature> target, CombatData data)
    {
        if (target.Count != 1)
        {
            skillOnUseText = "Can only attack 1 target";
            return false;
        }
        int damage = data.attackpower;
        skillOnUseText = "Hit for " + damage + " damage";
        target[0].UnderAttack(damage);
        return true;
    }
}
