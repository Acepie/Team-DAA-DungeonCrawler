using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCloseAoe : AbstractSkill {


    private GameObject aoe;

    
    protected Collider2D[] getNearbyTargets()
    {
        Transform parent = this.GetComponentInParent<Transform>();
        aoe = new GameObject("aoe");
        aoe.transform.position = parent.transform.position;
        CircleCollider2D skillRadiusCol = aoe.AddComponent<CircleCollider2D>();
        skillRadiusCol.radius = skillRadius;
        skillRadiusCol.isTrigger = true;

        ContactFilter2D cf2d = new ContactFilter2D();
        int playerLayer = 8;
        int enemyLayer = 9;
        int playerMask = 1 << playerLayer;
        int enemyMask = 1 << enemyLayer;

        if (parent.CompareTag("Player"))
        {
            cf2d.layerMask = enemyMask;
        }
        else
        {
            cf2d.layerMask = playerMask;
        }

        cf2d.useLayerMask = true;

        Collider2D[] combatantColliders;
        combatantColliders = new Collider2D[100]; //Max of 100 creatures in a combat
        int numOfCombatants = skillRadiusCol.OverlapCollider(cf2d, combatantColliders);
        Destroy(aoe);
        return combatantColliders;
    }
}
