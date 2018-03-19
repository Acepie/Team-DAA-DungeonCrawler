using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : AbstractPickupItem {

    public int armorAmount = 2;

	public override void onCollect(PlayerUIController uiController, CombatData data) {
        uiController.PickupEvent("You got some armor! Your health has permanently increased by 2 points!");
        data.currentHealth += armorAmount;
        data.maxHealth += armorAmount;
    }
}