using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : AbstractPickupItem {

    public int health = 15;

	public override void onCollect(PlayerUIController uiController, CombatData data) {
        if (data.CurrentHealth + health > data.MaxHealth)
        {
            data.CurrentHealth = data.MaxHealth;
        }
        else
        {
            data.CurrentHealth += health;
        }
        uiController.PickupEvent("Healed for " + health + " hit points!");
    }
}