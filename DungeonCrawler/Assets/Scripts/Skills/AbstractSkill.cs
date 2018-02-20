using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSkill : MonoBehaviour {

    protected int skillCooldown;
    protected int turnsUntilOffCD;

    protected int skillCost;

    protected string skillName;
    protected string skillDescription;
    protected string skillOnUseText;

    public bool skillSuccessful;

    public string attemptSkill(AbstractCreature target)
    {
        if (performSkill(target))
        {
            skillSuccessful = true;
            return this.skillOnUseText;
        }
        else
        {
            if (this.skillOnCooldown())
            {
                skillSuccessful = false;
                return this.skillName + " is on cooldown for " + turnsUntilOffCD + " more turns";
            }
            else
            {
                skillSuccessful = false;
                return "Skill performed unsuccessfully";
            }
        }
    }

    protected abstract bool performSkill(AbstractCreature target);

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
