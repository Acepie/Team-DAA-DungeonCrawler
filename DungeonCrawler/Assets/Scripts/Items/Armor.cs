using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : AbstractPickupItem {

    public int armorAmount = 10;

	public override void onCollect(PlayerUIController uiController, CombatData data) {
        uiController.PickupEvent("You got some armor! Your health has permanently increased by " + armorAmount + " points!");
        data.CurrentHealth += armorAmount;
        data.MaxHealth += armorAmount;
    }
}