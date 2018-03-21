using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Skill interface for inheritance.
	*skillCooldown - Number of turns skills will be unavailable
	*turnsUntilOffCD - Number of remaining turns until skill is available. Based on skillCooldown
	*skillCost - 'Action Point' cost to use skill, potential feature for future
	*skillName - Name of skill
	*skillDescription - Text to display on skill hover - UI element
	*skillOnUseText - Combat Log text to display when the skill is used successfully
*/

public abstract class AbstractSkill : MonoBehaviour
{

    protected int skillCooldown;
    protected int turnsUntilOffCD;

    protected int skillCost;
    protected float skillRadius;
    private GameObject skillRadiusIndicator;

    protected string skillName;
    protected string skillDescription;
    protected string skillOnUseText;

    public bool skillSuccessful;

    protected bool skillPrepared = false;

    /* Attempt to perform the skill. Takes in a target for the skill to be used on
       *Returns a string detailing information about the usage of the skill for the skillHandler to use
    */
    public string attemptSkill(List<AbstractCreature> targets, CombatData data, AbstractCreature skillUser)
    {  
        if (this.skillOnCooldown())
        {
            return this.skillName + " is on cooldown for " + turnsUntilOffCD + " more turns";
        }

        if (targets.Count == 0)
        {
            return "No targets selected!";
        }

        foreach (AbstractCreature t in targets)
        {
            if (Vector3.Distance(skillUser.transform.position, t.transform.position) > skillRadius)
            {
                return "Target out of range";
            } 
        }

        skillSuccessful = performSkill(targets, data);
        if (skillSuccessful)
        {
            turnsUntilOffCD = skillCooldown;
            return this.skillOnUseText;
        }
        else
            return "Unable to use " + this.skillName + " at this time" ;
        
    }

    public string prepareSkill(List<AbstractCreature> targets, CombatData data, AbstractCreature skillUser)
    {
        skillSuccessful = false;
        if (skillPrepared)
        {
            Destroy(skillRadiusIndicator);
            skillPrepared = false;
            return attemptSkill(targets, data, skillUser);
        }
        skillPrepared = true;
        skillRadiusIndicator = (GameObject)Instantiate(Resources.Load("SRI"));
        skillRadiusIndicator.transform.localScale = skillRadiusIndicator.transform.localScale * skillRadius;
        skillRadiusIndicator.transform.position = skillUser.transform.position;
        return this.skillName + " is ready to be used";
    }

    // Performs the skill and all its effects
    protected abstract bool performSkill(List<AbstractCreature> target, CombatData data);

    public bool skillOnCooldown()
    {
        return turnsUntilOffCD != 0;
    }

    //Decrements the number of turns remaining until the skill is available to be used again
    public void decrementCooldownCountdown()
    {
        if (turnsUntilOffCD > 0)
        {
            turnsUntilOffCD -= 1;
        }
    }

    public void resetCooldown()
    {
        turnsUntilOffCD = 0;
    }
}
