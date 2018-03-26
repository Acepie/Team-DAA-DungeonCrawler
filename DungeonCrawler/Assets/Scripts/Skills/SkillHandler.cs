using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Handles the skills for the player
	*Skillbar - Array of skills attached to the player object. Set on Awake()
	*skillText - UI Text element that informs player of outcome of performing a skill
	*fadeTextPlaying - Bool to keep track of if fadeText coroutine is playing to prevent overlap
	*skillDescription - Potential UI element for skill flavor text
	*skillPerformed - Bool to determine if skill was performed succesfully. Used for player to determine if action point cost should be consumed and end turn conditions
	*coroutine - FadeText Coroutine
*/

public class SkillHandler : MonoBehaviour
{

    //Refactor skillHandler to receive a skill from hotbar and attempt to perform that skill. To allow control of toggleable radiuses for different skills

    private AbstractSkill[] skillBar;
    AbstractSkill skillToUse;
    public Text skillText;
    private bool fadeTextPlaying;
    public Text skillDescription;

    public bool skillPerformed;
    private bool skillRadiusToggle = false;

    private IEnumerator coroutine;

    void Awake()
    {
        skillBar = GetComponents<AbstractSkill>();
    }

    public string getSkillsText()
    {
        int skillIndex = 1;
        string res = "Skill available: \n";
        foreach (var skill in skillBar)
        {
            res += "(" + skillIndex.ToString() + ") " + skill.SkillName + "\n";
            skillIndex++;
        }
        return res;
    }

    // Performs a skill at a given index within the skillbar
    public bool performSkillAtIndex(int i, List<AbstractCreature> targets, CombatData data, AbstractCreature skillUser)
    {
        if (i < 0 || i >= skillBar.Length)
        {
            //Attempting to get a skill out of array bounds
            SkillEvent("No skill found!");
            return false;
        }
        else
        {
            SkillEvent(skillBar[i].prepareSkill(targets, data, skillUser));
            return skillBar[i].skillSuccessful;
        }
    }

    public string getSkillDescriptionAtIndex(int i)
    {
        return skillBar[i].SkillDescription;
    }

    //Decrements all skill's remaining cooldown until available again
    public void decrementSkillsCooldown()
    {
        foreach (AbstractSkill s in skillBar)
        {
            if (s.skillOnCooldown())
            {
                s.decrementCooldownCountdown();
            }
        }
    }

    public void resetCooldowns()
    {
        foreach (AbstractSkill s in skillBar)
        {
            s.resetCooldown();
        }
    }

    //Displays text for usage of a skill
    private void SkillEvent(string s)
    {
        if (fadeTextPlaying)
        {
            StopCoroutine(coroutine); // Stops coroutine in case multiple items are picked up in small time window
        }

        skillText.text = s;
        coroutine = fadeText(skillText, 0.5f);
        StartCoroutine(coroutine);
    }

    //Fades the skillText
    private IEnumerator fadeText(Text t, float duration)
    {
        fadeTextPlaying = true;
        float i = 1;
        while (i >= 0)
        {
            t.color = new Color(t.color.r, t.color.g, t.color.b, i);
            i -= 0.1f;
            yield return new WaitForSeconds(duration);
        }
        t.color = new Color(t.color.r, t.color.g, t.color.b, 0);
        fadeTextPlaying = false;
    }
}
