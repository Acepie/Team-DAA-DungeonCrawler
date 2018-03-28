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

    protected override bool performSkill(List<AbstractCreature> target, CombatData data)
    {
        skillOnUseText = "Shielded yourself for " + manaShield.ShieldHealth;
        parent.statusController.addStatus(manaShield);
        return true;
        
    }

    public override string  attemptSkill(List<AbstractCreature> targets, CombatData data)
    {
        skillPrepared = false;
        //Checks to make sure skill is not on CD
        if (this.skillOnCooldown())
        {
            return this.skillName + " is on cooldown for " + turnsUntilOffCD + " more turns";
        }

        skillSuccessful = performSkill(targets, data);
        if (skillSuccessful)
        {
            turnsUntilOffCD = skillCooldown;
            return this.skillOnUseText;
        }
        else
            return "Unable to use " + this.skillName + " at this time";
    }
}
