using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : AbstractPickupItem {

    public int weaponPower = 1;

	public override void onCollect(PlayerUIController uiController, CombatData data) {
        uiController.PickupEvent("Picked up a weapon! Your strength has increased by 1 point!");
        data.attackpower += weaponPower;
    }
}