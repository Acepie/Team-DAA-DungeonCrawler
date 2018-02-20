﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCreature : MonoBehaviour
{

    public PlayerData data;
    public float speed;

    protected bool inCombat;

    public abstract void Move(float speed);

    public virtual void UnderAttack(int damageTaken)
    {
        data.currentHealth -= damageTaken;
    }

    public virtual bool IsDead()
    {
        return data.currentHealth <= 0;
    }

    public abstract void OnDeath();

    public abstract void StartTurn();

    public virtual void CombatStarted()
    {
        inCombat = true;
        Move(0);
    }

    public virtual void CombatEnded()
    {
        inCombat = false;
    }

    public abstract IEnumerator PerformTurn(List<AbstractCreature> targets);
}
