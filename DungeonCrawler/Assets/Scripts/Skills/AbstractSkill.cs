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
    protected GameObject skillRadiusIndicator;
    public GameObject SkillRadiusIndicator { get { return skillRadiusIndicator; } set { skillRadiusIndicator = value; } }

    [SerializeField]
    protected AbstractCreature parent;

    protected string skillName;
    public string SkillName { get { return skillName; } }
    protected string skillDescription;
    public string SkillDescription { get  { return skillDescription; } }
    protected string skillOnUseText;
    public string SkillOnUseText
    {
        get { return skillOnUseText; }
        set { skillOnUseText = value; }
    }
    protected int ignoreLayer;

    public bool skillSuccessful = false;

    protected bool skillPrepared = false;

    void Awake()
    {
        parent = GetComponentInParent<AbstractCreature>();
    }
    /* Attempt to perform the skill. Takes in a target for the skill to be used on
       *Returns a string detailing information about the usage of the skill for the skillHandler to use
    */
    public virtual string attemptSkill(List<AbstractCreature> targets, CombatData data)
    {
        skillPrepared = false;
        //Checks to make sure skill is not on CD


        //Confirms a target is selected
        if (targets.Count == 0)
        {
            return "No targets selected!";
        }

        //Checks to make sure target(s) are in range
        foreach (AbstractCreature t in targets)
        {
            Vector2 start = parent.transform.position;
            Vector2 end = t.transform.position;
            Vector2 direction = end - start;
            if (Vector2.Distance(start, end) > skillRadius)
            {
                return "Target out of range";
            }

            //Need to ignore layer of object that uses skill 
            //because raycast originates inside of game object and instantly collidesd
            ignoreLayer = 1 << parent.gameObject.layer;
            ignoreLayer = ~ignoreLayer;

            RaycastHit2D rayHit = Physics2D.Raycast(start, direction, skillRadius, ignoreLayer);
            //Introduced a bug that allows you to hit enemies further than intended if collision with another
            //enemy occurs first. Target should be modified to be the first enemy hit via raycast
            //However, this should not be too much of an issue as it will rarely come up
            //Also, this should make melee better 
            if(rayHit.collider.tag != t.tag)
            {
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
            return  this.skillName + " cannot attack multiple targets";
        
    }

    public string prepareSkill()
    {
        skillSuccessful = false;
        if (this.skillOnCooldown())
        {
            return this.skillName + " is on cooldown for " + turnsUntilOffCD + " more turns";
        }

        
        skillPrepared = true;
        return (this.SkillName + " is ready to use");
    }

    public string unprepareSkill()
    {
        skillPrepared = false;
        return (this.SkillName + "is no longer ready to use");
    }

    public bool isPrepared()
    {
        return skillPrepared;
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


    public void destroySRI()
    {
        Destroy(skillRadiusIndicator);
    }

    public void resetCooldown()
    {
        turnsUntilOffCD = 0;
    }
}
