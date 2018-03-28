using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ignited : AbstractStatus
{

    private int fireDamage;
    public int FireDamage { get { return fireDamage; } }

    public Ignited(int dotDmg, int duration, string name)
    {
        fireDamage = dotDmg;
        statusDuration = duration;
        statusName = name;
    }

    public override void applyStatus(AbstractCreature target)
    {
        TurnsUntilRemoved = statusDuration;
    }

    public override void removeStatus(AbstractCreature target)
    {
        
    }


}
