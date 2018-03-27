using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelManager {

    private static int currentLevel = 1;
    private static int currentExp = 0;
    private static int expToNextLevel = 100;
    private static PlayerController player;
    private static PlayerUIController playerUIController;


    // Use this for initialization
    public static void Init(PlayerController p, PlayerUIController uiController)
    {
        player = p;
        playerUIController = uiController;
    }

    public static void addExp(int exp)
    {
        currentExp += exp;
        playerUIController.PickupEvent("You gained " + exp + " exp!");
        if (currentExp >= expToNextLevel)
        {
            currentExp %= expToNextLevel;
            levelUp();
            expToNextLevel = currentLevel * expToNextLevel;
        }
    }

    private static void levelUp()
    {
        currentLevel++;
        player.data.AttackPower += 5;
        player.data.MaxHealth += 10;
        player.data.CurrentHealth += 10;
        playerUIController.PickupEvent("You leveled up! Health increased by 10. Damage increased by 10");
    }
}
