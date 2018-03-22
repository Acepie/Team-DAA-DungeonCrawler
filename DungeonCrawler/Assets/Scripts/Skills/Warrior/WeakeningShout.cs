using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakeningShout : AbstractSkill {


    // Use this for initialization
    void Start () {
        skillName = "Weakening Shout";
        skillCooldown = 3;
        skillCost = 2;
        skillDescription = "Weaken your enemies. Reducing their damage dealt by 25%" + skillCooldown;
        skillRadius = 8;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    protected override bool performSkill(List<AbstractCreature> target, CombatData data)
    {
        return false;
    }
}
