using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weakened : AbstractStatus {

    private float attackModifier;
    public float AttackModifier { get { return attackModifier; } set { attackModifier = value; } }

    // Use this for initialization
    void Start()
    {
        statusName = "Weakened";
    }

    public Weakened(int d, float a, string s)
    {
        statusDuration = d;
        attackModifier = a;
    }

    public override void applyStatus(AbstractCreature target)
    {
        TurnsUntilRemoved = statusDuration;
        target.data.AttackPower = (int)Math.Floor(target.data.AttackPower * attackModifier);
    }

    public override void removeStatus(AbstractCreature target)
    {
        target.data.AttackPower = target.data.RawAttackPower;
    }
}
