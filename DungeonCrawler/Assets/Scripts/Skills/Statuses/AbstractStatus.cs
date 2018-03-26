using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractStatus {


    protected int statusDuration;
    protected int turnsUntilRemoved;
    protected string statusName;
    public int StatusDuration { get { return statusDuration; } }
    public int TurnsUntilRemoved
    {
        get { return turnsUntilRemoved; }
        set { turnsUntilRemoved = value; }
    }


    public abstract void applyStatus(AbstractCreature target, int duration);
    public abstract void removeStatus(AbstractCreature target);

}
