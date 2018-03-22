﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaWave : AbstractProjectile {


    // Use this for initialization
    void Start () {
        skillName = "Lava Wave";
        skillDescription = "Shoot forth a cascading wave of lava";
        skillCost = 2;
        skillCooldown = 1;
        skillRadius = 10;
    }


    protected override bool performSkill(List<AbstractCreature> target, CombatData data)
    {
        fireProjectle(target[0].transform.position);
        return true;
    }


}
