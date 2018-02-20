using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHandler : MonoBehaviour {

    private AbstractSkill[] skillBar;

    void Awake()
    {
        skillBar = GetComponents<AbstractSkill>();
    }

    void Start() {

    }

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

    public AbstractSkill getSkillAtIndex(int i)
    {
        i -= 1;
        if (i < 0 || i >= skillBar.Length)
        {
            //Attempting to get a skill out of array bounds
            return null;
        } else
        {
            return skillBar[i];
        }
        

    }

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
}
