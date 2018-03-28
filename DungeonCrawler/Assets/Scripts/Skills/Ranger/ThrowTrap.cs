using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowTrap : AbstractAoE {


    // Use this for initialization
    void Start () {
        skillName = "Throw Trap";
        skillDescription = "Throw an explosive trap that detonates when an enemy walks near it. Cooldown of: " + skillCooldown;
        skillCost = 1;
        skillCooldown = 3;
        skillRadius = 8;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected override void detonateAoE()
    {
        throw new NotImplementedException();
    }

    protected override bool performSkill(List<AbstractCreature> target, CombatData data)
    {
        return false;
    }
}
