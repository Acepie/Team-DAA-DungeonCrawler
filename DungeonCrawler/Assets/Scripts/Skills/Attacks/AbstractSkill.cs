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

    protected string skillName;
    protected string skillDescription;
    protected string skillOnUseText;

    public bool skillSuccessful;

    /* Attempt to perform the skill. Takes in a target for the skill to be used on
       *Returns a string detailing information about the usage of the skill for the skillHandler to use
    */
    public string attemptSkill(AbstractCreature target, PlayerData data)
    {
        if (performSkill(target, data))
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

    // Performs the skill and all its effects
    protected abstract bool performSkill(AbstractCreature target, PlayerData data);

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
