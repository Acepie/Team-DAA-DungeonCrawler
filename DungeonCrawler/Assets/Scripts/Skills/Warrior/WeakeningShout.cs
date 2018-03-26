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
        skillDescription = "Weaken your enemies. Reducing their damage dealt by 50%" + skillCooldown;
        skillRadius = 2;

        w = new Weakened(2, 0.50f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    protected override bool performSkill(List<AbstractCreature> target, CombatData data)
    {
        List<AbstractCreature> targets = getNearbyTargets();

        foreach (AbstractCreature t in targets)
        {
            if(t.name != this.transform.name)
            {
                t.statusController.addStatus(w);
            }
        }
        return true;
    }
}
