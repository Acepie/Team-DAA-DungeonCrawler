using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Needs to be cleaned up
public class Whirlwind : AbstractCloseAoe {

    // Use this for initialization
    void Start () {
        skillName = "Whirlwind";
        skillCooldown = 3;
        skillCost = 3;
        skillDescription = "Deal damage to all nearby enemies. Cooldown of: " + skillCooldown;
        skillRadius = 2;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected override bool performSkill(List<AbstractCreature> target, CombatData data)
    {
        Collider2D[] combatantColliders = getNearbyTargets();
        int i = 0;
        while (combatantColliders[i] != null)
        {
            if (combatantColliders[i].GetComponent<AbstractCreature>().name != this.GetComponentInParent<Transform>().name)
            {
                combatantColliders[i].GetComponent<AbstractCreature>().UnderAttack(data.attackpower);
            }
            i++;
        }
        return true;
    }
}
