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
    public float skillRadius;
    private GameObject skillRadiusIndicator;

    protected AbstractCreature skillUser;

    protected string skillName;
    public string SkillName { get { return skillName; } }
    protected string skillDescription;
    public string SkillDescription { get { return skillDescription; } }
    protected string skillOnUseText;
    public string SkillOnUseText { get { return skillOnUseText; } }
    protected int ignoreLayer;

    public bool skillSuccessful;

    protected bool skillPrepared = false;

    /* Attempt to perform the skill. Takes in a target for the skill to be used on
       *Returns a string detailing information about the usage of the skill for the skillHandler to use
    */
    public string attemptSkill(List<AbstractCreature> targets, CombatData data)
    {  
        //Checks to make sure skill is not on CD
        if (this.skillOnCooldown())
        {
            return this.skillName + " is on cooldown for " + turnsUntilOffCD + " more turns";
        }

        //Confirms a target is selected
        if (targets.Count == 0)
        {
            return "No targets selected!";
        }

        //Checks to make sure target(s) are in range
        foreach (AbstractCreature t in targets)
        {
            Vector3 start = skillUser.transform.position;
            Vector3 end = t.transform.position;
            Vector3 direction = end - start;
            //Debug.Log(Vector3.Distance(start, end));
            if (Vector3.Distance(start, end) > skillRadius)
            {
                return "Target out of range";
            }

            //Need to ignore layer of object that uses skill 
            //because raycast originates inside of game object and instantly collidesd
            ignoreLayer = 1 << skillUser.gameObject.layer;
            ignoreLayer = ~ignoreLayer;

            RaycastHit2D rayHit = Physics2D.Raycast(start, direction, skillRadius, ignoreLayer);
           // Debug.Log(rayHit.collider);
            if(rayHit.collider != t.GetComponent<Collider2D>())
            {
                Debug.Log(rayHit.collider.name);
                Debug.Log(start);
                return "Target out of sight";
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

    public string prepareSkill(List<AbstractCreature> targets, CombatData data, AbstractCreature su)
    {
        skillUser = su;
        skillSuccessful = false;
        if (skillPrepared)
        {
            Destroy(skillRadiusIndicator);
            skillPrepared = false;
            return attemptSkill(targets, data);
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
