using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaShield : AbstractSkill {

    private Shield manaShield;

    // Use this for initialization
    void Start () {
        skillName = "Mana Shield";
        skillCost = 1;
        skillCooldown = 4;
        skillDescription = "Shield yourself with a mana barrier. Grants you an extra 35 hit points for two turns. Cooldown of: " + skillCooldown;
        skillRadius = 0;
        manaShield = new Shield(2, 35, "Manashield");
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected override bool performSkill(List<AbstractCreature> target, CombatData data)
    {
        return true;
    }
}
