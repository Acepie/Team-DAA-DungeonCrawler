using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCreature : MonoBehaviour {

	public int health;
	public float speed;

	protected bool inCombat;

	public abstract void Move(float speed);

	public virtual void UnderAttack(int damageTaken) {
		health -= damageTaken;
        Debug.Log("Hit taken for: " + damageTaken + "  Total health: " + health);
	}

	public abstract void MakeAttack(List<AbstractCreature> targets);

	public virtual bool IsDead() {
		return health <= 0;
	}

	public abstract void OnDeath();

	public abstract bool TurnOver();

	public abstract void StartTurn(List<AbstractCreature> targets);

	public virtual void CombatStarted() {
		inCombat = true;
		Move(0);
	}

	public virtual void CombatEnded() {
		inCombat = false;
		Move (speed);
	}
}
