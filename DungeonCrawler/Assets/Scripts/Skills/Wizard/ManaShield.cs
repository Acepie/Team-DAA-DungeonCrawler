using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaShield : AbstractSkill {


    // Use this for initialization
    void Start () {
        skillName = "Mana Shield";
        skillCost = 1;
        skillCooldown = 4;
        skillDescription = "Shield yourself with a mana barrier. Grants you an extra 5 hit points for two turns. Cooldown of: " + skillCooldown;
        skillRadius = 0;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected override bool performSkill(List<AbstractCreature> target, CombatData data)
    {
        return false;
    }
}
