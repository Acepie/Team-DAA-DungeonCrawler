using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalReposition : AbstractSkill {


    // Use this for initialization
    void Start () {
        skillName = "Tactical Reposition";
        skillCooldown = 3;
        skillCost = 1;
        skillDescription = "Teleport instantly to another location in sight and range. Cooldown of" + skillCooldown;
        skillRadius = 12;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected override bool performSkill(List<AbstractCreature> target, CombatData data)
    {
        return false;
    }
}
