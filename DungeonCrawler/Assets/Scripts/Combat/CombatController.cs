﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{

    List<AbstractCreature> combatants;
    private int turnCount;
    List<AbstractCreature> nonplayers;
    List<AbstractCreature> players;
    public float radius = 1.5f;

    /* Create our combat controller for a single combat.
    Create a circle collider to determine all combatants that should be in this combat.
    All creatures within that combat are considered in combat, and set appropriately.
    Create list of nonplayers/players in combat for valid targets during appropraite turns
    */
    void Awake()
    {
        ContactFilter2D cf2d = new ContactFilter2D();
        cf2d.layerMask = LayerMask.GetMask("Player", "Enemy");
        cf2d.useLayerMask = true;

        CircleCollider2D combatRadius = gameObject.AddComponent<CircleCollider2D>();
        combatRadius.radius = radius;
        combatRadius.isTrigger = true;

        Collider2D[] combatantColliders;
        combatantColliders = new Collider2D[100]; //Max of 100 creatures in a combat
        int numOfCombatants = combatRadius.OverlapCollider(cf2d, combatantColliders);

        if (numOfCombatants == 0)
        {
            return;
        }

        combatants = new List<AbstractCreature>();
        nonplayers = new List<AbstractCreature>();
        players = new List<AbstractCreature>();
        for (int i = 0; i < numOfCombatants; i++)
        {
            AbstractCreature creature = combatantColliders[i].gameObject.GetComponent<AbstractCreature>();
            if (creature == null)
            {
                continue;
            }
            if (creature.CompareTag("Player"))
            {
                players.Add((PlayerController)creature);
            }
            else
            {
                nonplayers.Add(creature);

            }
            creature.CombatStarted();
            combatants.Add(creature);
        }

        Destroy(combatRadius);
        turnCount = 0;

        StartCoroutine("DoCombat");
    }


    IEnumerator DoCombat()
    {
        while (true)
        {
            AbstractCreature combatant = combatants[turnCount];

            // A combatant is already dead
            if (combatant == null)
            {
                combatants.RemoveAt(turnCount);
                turnCount = turnCount % combatants.Count;
                continue;
            }

            yield return StartCoroutine(DoTurn(combatant));
        }
    }

    IEnumerator DoTurn(AbstractCreature combatant)
    {
        // get list of valid targets for combatant
        List<AbstractCreature> targetList;
        if (combatant.CompareTag("Player"))
        {
            targetList = nonplayers;
        }
        else
        {
            targetList = players;
        }

        // if no valid targets then combat should end
        HasWon(combatant, targetList);

        combatant.StartTurn();
        yield return StartCoroutine(combatant.PerformTurn(targetList));

        // kill any dead targets
        targetList.FindAll((target) =>
        {
            return target.IsDead();
        }).ForEach((d) =>
        {
            targetList.Remove(d);
            d.OnDeath();
        });

        HasWon(combatant, targetList);
        turnCount = (turnCount + 1) % combatants.Count;
        yield return null;
    }

    void HasWon(AbstractCreature combatant, List<AbstractCreature> targetList)
    {
        if (targetList.Count == 0)
        {
            if (combatant.CompareTag("Player"))
            {
                GameObject.Find("Canvas/CombatCenterText").GetComponent<CombatTextController>().displayWinner("Players won!");
                foreach (var player in players)
                {
                    if (player != null)
                    {
                        player.CombatEnded();
                    }
                }
            }
            else
            {
                GameObject.Find("Canvas/CombatCenterText").GetComponent<CombatTextController>().displayWinner("Monsters won!");
            }

            Destroy(this.gameObject);
        }
    }
}
