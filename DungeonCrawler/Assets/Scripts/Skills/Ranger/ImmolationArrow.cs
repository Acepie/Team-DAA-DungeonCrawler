using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmolationArrow : AbstractSkill {

    private Ignited ignite;
    [SerializeField]
    private int igniteDot = 6;
    [SerializeField]
    private int igniteDuration = 3;




    // Use this for initialization
    void Start () {
        ignite = new Ignited(igniteDot, igniteDuration, "Immolation Arrow");
        skillName = "Immolation Arrow";
        skillCooldown = 3;
        skillCost = 2;
        skillRadius = 6;
        skillDescription = "Light an enemy of fire dealing " + igniteDot 
            + "fire damage for " + igniteDuration + "turns. Cooldown: " + skillCooldown;
		
	}

    protected override bool performSkill(List<AbstractCreature> target, CombatData data)
    {
        if (target.Count > 1)
        {
            return false;
        }
        else
        {
            skillOnUseText = "Ignited " + target[0] + "!";
            target[0].statusController.addStatus(target[0], ignite);
            return true;
        }
    }
}
