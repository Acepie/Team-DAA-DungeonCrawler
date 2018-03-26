using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : AbstractStatus {

    private int shieldHealth;

    public Shield(int d, int h, string s)
    {
        shieldHealth = h;
        statusDuration = d;
        statusName = s;
    }


    public override void applyStatus(AbstractCreature target)
    {
        target.data.TemporaryHealth += shieldHealth;
        TurnsUntilRemoved = statusDuration;
    }

    public override void removeStatus(AbstractCreature target)
    {
        target.data.TemporaryHealth = 0;
    }
}
