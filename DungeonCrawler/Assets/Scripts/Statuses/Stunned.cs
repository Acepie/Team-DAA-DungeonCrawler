using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stunned : AbstractStatus {

    
    public Stunned(int d, string name)
    {
        statusDuration = d;
        statusName = name;
    }

    public override void applyStatus(AbstractCreature target)
    {
        target.data.Stunned = true;
        turnsUntilRemoved = statusDuration;
    }

    public override void removeStatus(AbstractCreature target)
    {
        target.data.Stunned = false;
    }
}
