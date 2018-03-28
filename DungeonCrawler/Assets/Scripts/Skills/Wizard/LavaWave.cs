using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaWave : AbstractProjectile {


    // Use this for initialization
    void Start () {
        skillName = "Lava Wave";
        skillCooldown = 2;
        skillDescription = "Shoot forth a cascading wave of lava. Cooldown of: " + skillCooldown;
        skillCost = 2;
        skillRadius = 6;
        projectile = (Rigidbody2D)Resources.Load("LavaWave", typeof(Rigidbody2D));
        projectileSpeed = 2;
    }


    protected override bool performSkill(List<AbstractCreature> target, CombatData data)
    {
        fireProjectle(target[0].transform.position, data.AttackPower);
        return true;
    }


}
