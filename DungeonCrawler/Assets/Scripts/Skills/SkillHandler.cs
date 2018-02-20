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

public class SkillHandler : MonoBehaviour {

    private AbstractSkill[] skillBar;
    public Text skillText;
    private bool fadeTextPlaying;
    public Text skillDescription;

    public bool skillPerformed;

    private IEnumerator coroutine;

    void Awake()
    {
        skillBar = GetComponents<AbstractSkill>();
    }

    //Allow player to set their own skills for a future skillbar
    public void setSkillAtIndex(int i, AbstractSkill skilltoSet)
    {
        //Incoming  value will be of keyboard input (1-9), set i to match array indexing
        i -= 1;
        if (i < 0 || i >= skillBar.Length)
        {
            //Attempting to set a skill out of array bounds
            return;
        }
        skillBar[i] = skilltoSet;
    }

    // Performs a skill at a given index within the skillbar
    public void performSkillAtIndex(int i, AbstractCreature target)
    {
        i -= 1;
        if (i < 0 || i >= skillBar.Length)
        {
            //Attempting to get a skill out of array bounds
            SkillEvent("No skill found!");
            skillPerformed = false;
            return;
        } else
        {
            SkillEvent(skillBar[i].attemptSkill(target));
            skillPerformed = skillBar[i].skillSuccessful;
        }
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
