using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunningStrike : AbstractSkill {

    private Stunned stun;
    [SerializeField]
    private int stunDuration;

    public int StunDuration {set { stunDuration = value; } }
    // Use this for initialization
    void Start()
    {
        skillName = "Stunning Strike";
        skillDescription = "Stun an enemy for a turn, preventing them from acting";
        skillCost = 3;
        skillCooldown = 5;
        skillRadius = 6;
        stunDuration = 2;
    }

    protected override bool performSkill(List<AbstractCreature> target, CombatData data)
    {
        if (target.Count != 1)
        {
            skillOnUseText = "Can only attack 1 target";
            return false;
        }

        stun = new Stunned(stunDuration);
        skillOnUseText = "Stunned " + target[0] + "for " + stunDuration + " turn";
        target[0].statusController.addStatus(target[0], stun);
        return true;
    }
}
