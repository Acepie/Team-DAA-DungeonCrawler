using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : AbstractPickupItem {

    public int weaponPower = 10;

	public override void onCollect(PlayerUIController uiController, CombatData data) {
        uiController.PickupEvent("Picked up a weapon! Your strength has increased by " + weaponPower + " points!");
        data.AttackPower += weaponPower;
    }
}