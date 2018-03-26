using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {

	public List<GameObject> switchables;
	
	public void ActivateSwitch() {
		switchables.ForEach((switchable) => {
			SwitchActivated sa = switchable.GetComponent<SwitchActivated>();
			if (sa) {
				sa.OnSwitch();
			} else {
				Debug.Log("Switch contains switchable with no switchactivated component");
			}
		});
	}
}
