using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour{

	List<AbstractCreature> combatants;
	private int roundCount;
	private int turnCount;
	List<AbstractCreature> nonplayers;
	List<AbstractCreature> players;

	void Awake() {
		ContactFilter2D cf2d = new ContactFilter2D();
		cf2d.layerMask = 12; // Layer 12 is Player Layer, Where enemies will be as well

		CircleCollider2D combatRadius = gameObject.AddComponent<CircleCollider2D> ();
		combatRadius.radius = 3;
		combatRadius.isTrigger = true;

		Collider2D[] combatantColliders;
		combatantColliders = new Collider2D[100]; //Max of 100 creatures in a combat
		int numOfCombatants = combatRadius.OverlapCollider (cf2d, combatantColliders);

		if (numOfCombatants == 0) {
			Debug.Log("nobody in combat");
			return;
		}

		combatants = new List<AbstractCreature>();
		nonplayers = new List<AbstractCreature>();
		players = new List<AbstractCreature>();
		for(int i = 0; i < numOfCombatants; i++) {
			AbstractCreature creature = combatantColliders[i].gameObject.GetComponent<AbstractCreature>();
			if (creature == null) {
				continue;
			}
			if (creature.CompareTag("Player")) {
				players.Add((PlayerController) creature);
			} else {
				nonplayers.Add(creature);

			}
			creature.CombatStarted();
			combatants.Add(creature);
		}
		
		Destroy(combatRadius);

		roundCount = 0;
		turnCount = 0;

		StartCoroutine("DoCombat");
	}

	IEnumerator DoCombat(){
		while (true) {
			AbstractCreature combatant = combatants[turnCount];

			// A combatant is already dead
			if (combatant == null) {
				combatants.RemoveAt(turnCount);
				turnCount = turnCount % combatants.Count;
				continue;
			}

			yield return StartCoroutine(DoTurn(combatant));
		}
	}

	IEnumerator DoTurn(AbstractCreature combatant) {
		// get list of valid targets for combatant
		List<AbstractCreature> targetList;
		if (combatant.CompareTag("Player")) {
			targetList = nonplayers;
		} else {
			targetList = players;
		}

		// if no valid targets then combat should end
		HasWon(combatant, targetList);
		yield return null;

		combatant.StartTurn(targetList);

		do {
			combatant.MakeAttack(targetList);

			// kill any dead targets
			targetList.FindAll((target) => {
				return target.IsDead();
			}).ForEach((d) => {
				targetList.Remove(d);
				d.OnDeath();
			});
			HasWon(combatant, targetList);
			yield return null;
		} while (!combatant.TurnOver());
		
		turnCount = (turnCount + 1) % combatants.Count;
		yield return null;
	}

	void HasWon(AbstractCreature combatant, List<AbstractCreature> targetList) {
		if (targetList.Count == 0) {
			if (combatant.CompareTag("Player")) {
				Debug.Log("Players won");
                GameObject.Find("Canvas/CombatCenterText").GetComponent<CombatTextController>().displayWinner("Players won!");
				foreach(var player in players) {
					if (player != null) {
						player.CombatEnded();
					}
				}
			} else {
				Debug.Log("Monsters won");
                GameObject.Find("Canvas/CombatCenterText").GetComponent<CombatTextController>().displayWinner("Monsters won!");

			}

			Destroy(this.gameObject);
		}
	}
}
