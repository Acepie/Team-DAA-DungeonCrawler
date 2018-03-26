using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakeningShout : AbstractCloseAoe {

    private Weakened w;

    // Use this for initialization
    void Start () {
        skillName = "Weakening Shout";
        skillCooldown = 4;
        skillCost = 2;
        skillDescription = "Weaken your enemies. Reducing their damage dealt by 50%.\nCooldown: "+ skillCooldown;
        skillRadius = 4;

        w = new Weakened(2, 0.50f, "Weakened");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    protected override bool performSkill(List<AbstractCreature> target, CombatData data)
    {
        List<AbstractCreature> targets = getNearbyTargets();
        SkillOnUseText = "Weakened ";

        foreach (AbstractCreature t in targets)
        {
            if(t.name != parent.name)
            {
                SkillOnUseText += t.name + "\n";
                t.statusController.addStatus(w);
            }
        }
        return true;
    }
}
