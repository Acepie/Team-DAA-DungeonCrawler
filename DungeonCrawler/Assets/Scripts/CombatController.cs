using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour{

	AbstractCreature activeCreature;

	List<AbstractCreature> combatants;
	private bool quitCombat;
	ContactFilter2D cf2d;
	private int roundCount;
	private int turnCount;
	int numOfCombatants;
	List<AbstractCreature> nonplayers;
	List<AbstractCreature> players;
	private bool turnHasStarted;

	AbstractCreature playerTarget;

	void Awake() {
		cf2d.layerMask = 12; // Layer 12 is Player Layer, Where enemies will be as well
		quitCombat = false;

		CircleCollider2D combatRadius = gameObject.AddComponent<CircleCollider2D> ();
		combatRadius.radius = 10;
		combatRadius.isTrigger = true;

		Collider2D[] combatantColliders;
		combatantColliders = new Collider2D[100]; //Max of 100 creatures in a combat
		numOfCombatants = combatRadius.OverlapCollider (cf2d, combatantColliders);

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
			if (creature is PlayerController) {
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
		turnHasStarted = false;
	}

	void Start(){
		 StartCoroutine("turnTracker");
	}


	IEnumerator turnTracker(){
		while(!quitCombat){
			activeCreature = combatants [turnCount];
			if(activeCreature.CompareTag("Player")){
				yield return StartCoroutine(PlayerTurn());
			} else {
				yield return StartCoroutine(EnemyTurn());
			}

			if (turnCount == numOfCombatants - 1) {
				turnCount = 0;
			} else {
				turnCount++;
			}

			if (nonplayers.Count == 0) {
				quitCombat = true;
				Debug.Log ("Combat has ended");

				foreach(var p in players){
					p.CombatEnded ();
				}
			}
		}
		yield return null;
	}

	IEnumerator PlayerTurn(){

		Debug.Log ("It is the player's turn");

		yield return PlayerSelectTarget();

		playerTarget.UnderAttack (10);
		if (playerTarget.IsDead()) {
			nonplayers.Remove (playerTarget);
			Debug.Log (nonplayers.Count);
		}
	}

	IEnumerator EnemyTurn(){
		Debug.Log ("It is the enemies turn");
		yield return null;

	}

	IEnumerator PlayerSelectTarget(){
		LayerMask lm = 12;
		Camera cam = activeCreature.GetComponent<PlayerController> ().GetComponentInChildren<Camera> ();
		RaycastHit2D hit = new RaycastHit2D();
	
		bool targetSelected = false;

		while (!targetSelected) {
			//Coroutine in player that waits until they left click the mouse
			yield return StartCoroutine (activeCreature.GetComponent<PlayerController> ().waitPlayerLeftClick ());
			hit = Physics2D.Raycast (cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				//targetCollider = Physics2D.Raycast (ray, 0.1f, lm).collider;
			if (hit.collider != null) {
				playerTarget = hit.collider.gameObject.GetComponent<AbstractCreature> ();
				Debug.Log (playerTarget.name);
				targetSelected = true;
			}
		}
		yield return null;
	}

	/*void Update()
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
	}*/
}
