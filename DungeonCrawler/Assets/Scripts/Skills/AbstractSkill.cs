using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSkill : MonoBehaviour {

    protected int skillCooldown;
    protected int turnsUntilOffCD;

    protected int skillCost;

    protected string skillName;
    protected string skillDescription;

    protected AbstractCreature target;

    public virtual void selectTarget(AbstractCreature t)
    {
        target = t;
    }

    public abstract void performSkill();

    public bool skillOnCooldown()
    {
        return turnsUntilOffCD != 0;
    }

    public void decrementCooldownCountdown()
    {
        if(turnsUntilOffCD > 0)
        {
            turnsUntilOffCD -= 1;
        }
    }
}
