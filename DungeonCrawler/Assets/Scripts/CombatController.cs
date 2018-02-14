using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController{

	Collider2D[] combatantColliders;
	List<AbstractCreature> combatants;
	GameObject combat;
	ContactFilter2D cf2d;

	private static bool combatConstructed = false;

	public CombatController(){
		if (!combatConstructed) {
			combatConstructed = true;
			cf2d.layerMask = 12; // Layer 12 is Player Layer, Where enemies will be as well

			combat = new GameObject ();
			combat.AddComponent<CircleCollider2D> ();
			CircleCollider2D combatRadius = combat.GetComponent<CircleCollider2D> ();
			combatRadius.radius = 10;
			combatRadius.isTrigger = true;

			combatantColliders = new Collider2D[100]; //Max of 100 creatures in a combat
			int numofCombatants = combatRadius.OverlapCollider (cf2d, combatantColliders);
			combatants = new List<AbstractCreature>();


			for(int i = 0; i < numofCombatants; i++) {
				combatants.Add (combatantColliders[i].gameObject.GetComponent<AbstractCreature>());
				Debug.Log ("added to combat: " + combatantColliders[i].gameObject.name);
				combatants [i].Move (0);
				combatants [i].enabled = false;
			}
		}
	}
}
