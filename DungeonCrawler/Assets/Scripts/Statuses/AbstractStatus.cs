using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractStatus {


    protected int statusDuration;
    protected int turnsUntilRemoved;
    protected string statusName;
    public int TurnsUntilRemoved
    {
        get { return turnsUntilRemoved; }
        set { turnsUntilRemoved = value; }
    }

    public string StatusName { get { return statusName; } }


    public abstract void applyStatus(AbstractCreature target);
    public abstract void removeStatus(AbstractCreature target);

}
