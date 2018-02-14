using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour{

	List<AbstractCreature> combatants;
	ContactFilter2D cf2d;
	private int turn;
	List<AbstractCreature> nonplayers;
	List<AbstractCreature> players;
	private bool turnHasStarted;

	void Awake() {
		cf2d.layerMask = 12; // Layer 12 is Player Layer, Where enemies will be as well

		CircleCollider2D combatRadius = gameObject.AddComponent<CircleCollider2D> ();
		combatRadius.radius = 10;
		combatRadius.isTrigger = true;

		Collider2D[] combatantColliders;
		combatantColliders = new Collider2D[100]; //Max of 100 creatures in a combat
		int numofCombatants = combatRadius.OverlapCollider (cf2d, combatantColliders);

		if (numofCombatants == 0) {
			Debug.Log("nobody in combat");
			return;
		}

		Debug.Log(numofCombatants);
		Debug.Log(combatantColliders);

		combatants = new List<AbstractCreature>();
		nonplayers = new List<AbstractCreature>();
		players = new List<AbstractCreature>();
		for(int i = 0; i < numofCombatants; i++) {
			AbstractCreature creature = combatantColliders[i].gameObject.GetComponent<AbstractCreature>();
			if (creature == null) {
				continue;
			}
			if (creature is PlayerController) {
				players.Add((PlayerController) creature);
			} else {
				nonplayers.Add(creature);
			}
			creature.CombatStarted();
			combatants.Add(creature);
		}
		
		Destroy(combatRadius);

		turn = 0;
		turnHasStarted = false;
	}

	void Update()
	{
		AbstractCreature combatant = combatants[turn];

		// A combatant is already dead
		if (combatant == null) {
			combatants.RemoveAt(turn);
			turn = turn % combatants.Count;
			return;
		}

		// the combatant's turn has already ended
		if (combatant.TurnOver()) {
			turn = (turn + 1) % combatants.Count;
			turnHasStarted = false;
		}

		// get list of valid targets for combatant
		List<AbstractCreature> targetList;
		if (combatant is PlayerController) {
			targetList = nonplayers;
		} else {
			targetList = players;
		}

		// if no valid targets then combat should end
		if (targetList.Count == 0) {
			if (combatant is PlayerController) {
				Debug.Log("Players won");
				foreach(var player in players) {
					if (player != null) {
						player.CombatEnded();
					}
				}
			} else {
				Debug.Log("Monsters won");
			}
			Destroy(this.gameObject);
		}

		if (!turnHasStarted) {
			combatant.StartTurn(targetList);
			turnHasStarted = true;
		}

		combatant.MakeAttack(targetList);

		// kill any dead targets
		targetList.FindAll((target) => {
			return target.IsDead();
		}).ForEach((d) => {
			targetList.Remove(d);
			d.OnDeath();
		});
	}
}
