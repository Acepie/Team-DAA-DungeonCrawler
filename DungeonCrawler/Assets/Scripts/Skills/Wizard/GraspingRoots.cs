using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraspingRoots : AbstractSkill {


    // Use this for initialization
    void Start () {
            skillName = "Grasping Roots";
            skillCooldown = 3;
            skillCost = 2;
            skillDescription = "Roots spurt forth from the ground ensnaring enemies caught. They are unable to move for one turn. Cooldown of: " + skillCooldown;
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
