using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPickupItem : MonoBehaviour {

	public abstract void onCollect(PlayerUIController uiController, CombatData data);

	public void collect(PlayerUIController uiController, CombatData data) {
		onCollect(uiController, data);
		Destroy(gameObject);
	}
}
