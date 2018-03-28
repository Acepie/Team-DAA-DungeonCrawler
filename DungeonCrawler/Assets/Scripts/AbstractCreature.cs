using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCreature : MonoBehaviour
{

    public CombatData data;
    public StatusController statusController = new StatusController();
    public float speed;

    protected bool inCombat;

    public bool InCombat { get { return inCombat; } }

    public abstract void Move(float speed);

    public virtual void UnderAttack(int damageTaken)
    {
        if(data.TemporaryHealth > 0)
        {
            data.TemporaryHealth -= damageTaken;
            if(data.TemporaryHealth < 0)
            {
                //Overflow damage
                data.CurrentHealth += data.TemporaryHealth;
            }
        } else
        data.CurrentHealth -= damageTaken;
    }

    public virtual bool IsDead()
    {
        return data.CurrentHealth <= 0;
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
    protected abstract void endTurn();
}
