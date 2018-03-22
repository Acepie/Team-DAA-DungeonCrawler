using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : AbstractPickupItem {

    public int health = 5;

	public override void onCollect(PlayerUIController uiController, CombatData data) {
        if (data.currentHealth + health > data.maxHealth)
        {
            data.currentHealth = data.maxHealth;
        }
        else
        {
            data.currentHealth += health;
        }
        uiController.PickupEvent("Healed for 5 hit points!");
    }
}