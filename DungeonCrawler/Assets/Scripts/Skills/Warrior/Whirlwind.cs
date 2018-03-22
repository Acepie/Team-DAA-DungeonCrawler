using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whirlwind : AbstractSkill {


    // Use this for initialization
    void Start () {
        skillName = "Whirlwind";
        skillCooldown = 3;
        skillCost = 3;
        skillDescription = "Deal damage to all nearby enemies. Cooldown of: " + skillCooldown;
        skillRadius = 5;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected override bool performSkill(List<AbstractCreature> target, CombatData data)
    {
        return false;
    }
}
