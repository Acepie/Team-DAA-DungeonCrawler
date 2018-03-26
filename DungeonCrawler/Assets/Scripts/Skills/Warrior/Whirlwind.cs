using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Needs to be cleaned up
public class Whirlwind : AbstractCloseAoe
{

    // Use this for initialization
    void Start()
    {
        skillName = "Whirlwind";
        skillCooldown = 3;
        skillCost = 3;
        skillDescription = "Deal damage to all nearby enemies. Cooldown of: " + skillCooldown;
        skillRadius = 2;
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override bool performSkill(List<AbstractCreature> target, CombatData data)
    {
        List<AbstractCreature> targets = getNearbyTargets();
        int i = 0;
        foreach (AbstractCreature t in targets)
        {
            if (t.GetComponent<AbstractCreature>().name != this.GetComponentInParent<Transform>().name)
            {
                t.UnderAttack(data.AttackPower);
            }
        }
        return true;
    }
}
