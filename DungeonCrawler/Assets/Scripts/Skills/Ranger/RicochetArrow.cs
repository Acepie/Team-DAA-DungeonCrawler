using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetArrow : AbstractSkill {

    HashSet<AbstractCreature> unavailableTargets = new HashSet<AbstractCreature>();
    [SerializeField]
    private float ricochetRadius = 2.5f;
    ContactFilter2D cf2d = new ContactFilter2D();

    // Use this for initialization
    void Start () {
        skillName = "Ricochet Arrow";
        skillCost = 2;
        skillCooldown = 4;
        skillRadius = 6;
        skillDescription = "Shoot an arrow that ricochets into multiple enemies. Cooldown: " + skillCooldown;
        unavailableTargets.Add((parent.GetComponent<AbstractCreature>()));

        
        if (parent.CompareTag("Player"))
        {
            cf2d.layerMask = LayerMask.GetMask("Enemy");
        }
        else
        {
            cf2d.layerMask = LayerMask.GetMask("Player");
        }
        cf2d.useLayerMask = true;

    }


    protected override bool performSkill(List<AbstractCreature> target, CombatData data)
    {
        SkillOnUseText = "Hit ";

        if (target.Count != 1)
        {
            skillOnUseText = "Can only attack 1 target";
            return false;
        }

        target[0].UnderAttack(data.AttackPower);
        unavailableTargets.Add(target[0]);
        skillOnUseText = "Hit " + target[0].name + "for " + data.AttackPower + " damage\n ";
        richochet(getNearbyTargets(target[0].transform.position), data);
        clearUnavailableTargets();
        return true;
    }

    private void richochet(List<AbstractCreature> targets, CombatData data)
    {
        foreach(AbstractCreature t in targets)
        {
            t.UnderAttack(data.AttackPower);
            skillOnUseText += "Hit " + t.name + "for " + data.AttackPower + " damage\n";
            richochet(getNearbyTargets(t.transform.position), data);
        }
    }

    protected List<AbstractCreature> getNearbyTargets(Vector3 origin)
    {
        GameObject ricochetArrow = new GameObject();
        ricochetArrow.transform.position = origin;
        CircleCollider2D skillRadiusCol = ricochetArrow.AddComponent<CircleCollider2D>();
        skillRadiusCol.isTrigger = true;
        skillRadiusCol.radius = ricochetRadius;
        



        Collider2D[] combatantColliders;
        combatantColliders = new Collider2D[100];
        int numOfCombatants = skillRadiusCol.OverlapCollider(cf2d, combatantColliders);

        int i = 0;
        List<AbstractCreature> creaturesInCollision = new List<AbstractCreature>();
        while (combatantColliders[i] != null)
        {
            if (unavailableTargets.Contains(combatantColliders[i].GetComponent<AbstractCreature>()) == false)
            {
                creaturesInCollision.Add(combatantColliders[i].GetComponent<AbstractCreature>());
                unavailableTargets.Add(combatantColliders[i].GetComponent<AbstractCreature>());
            }
            i++;
        }
        Destroy(ricochetArrow);
        return creaturesInCollision;
    }

    private void clearUnavailableTargets()
    {
        unavailableTargets.Clear();
        unavailableTargets.Add(parent.GetComponent<AbstractCreature>());
    }
}
