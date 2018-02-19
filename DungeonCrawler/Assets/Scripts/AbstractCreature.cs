using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCreature : MonoBehaviour {

	public int currentHealth;
	public int maxHealth;
	public float speed;

	protected bool inCombat;

	public abstract void Move(float speed);

	public virtual void UnderAttack(int damageTaken) {
		currentHealth -= damageTaken;
	}

	public abstract void MakeAttack(List<AbstractCreature> targets);

	public virtual bool IsDead() {
		return currentHealth <= 0;
	}

	public abstract void OnDeath();

	public abstract bool TurnOver();

	public abstract void StartTurn();

	public virtual void CombatStarted() {
		inCombat = true;
		Move(0);
	}

	public virtual void CombatEnded() {
		inCombat = false;
		Move (speed);
	}

	public abstract IEnumerator PerformTurn(List<AbstractCreature> targets);
}
