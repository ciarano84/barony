﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.WSA.Input;

public class Initiative : MonoBehaviour
{
    public UnityEvent newTurn;
    
    //I can likely strip this down to just be a list and a queue. 
    static List<TacticsMovement> unsortedUnits = new List<TacticsMovement>();
    public static List<TacticsMovement> sortedUnits = new List<TacticsMovement>();
    static Queue<TacticsMovement> order = new Queue<TacticsMovement>();

    //static bool combatStarted = false;
    public static TacticsMovement currentUnit; 
    static ActionUIManager actionUIManager;

    private void Start()
    {
        actionUIManager = FindObjectOfType<ActionUIManager>();
        StartCoroutine("StartEncounter");
    }

    IEnumerator StartEncounter()
    {
        yield return new WaitForSeconds(2f);
        sortedUnits = unsortedUnits.OrderByDescending(o => o.currentInitiative).ToList();
        foreach (TacticsMovement u in sortedUnits)
        {
            order.Enqueue(u);
        }
        
        //combatStarted = true;
        StartTurn();
        yield break;
    }

    public static void AddUnit(TacticsMovement unit)
    {
        unsortedUnits.Add(unit);
    }

    static void StartTurn()
    {
        order.Peek().BeginTurn();
        currentUnit = order.Peek();
        actionUIManager.UpdateActions(currentUnit.GetComponent<PlayerCharacter>());
    }

    public static void EndTurn()
    {
        TacticsMovement unit = order.Dequeue();
        unit.EndTurn();
        StartTurn();
        order.Enqueue(unit);

    }

    public void UpdateUI()
    {
        
    }

    public static void CheckForTurnEnd(TacticsMovement unit) 
    {
        //Check its a player Character
        if (unit.GetComponent<PlayerCharacter>() != null)
        {
            if (unit.remainingMove > 0 || unit.remainingActions > 0)
            {
                actionUIManager.UpdateActions(currentUnit.GetComponent<PlayerCharacter>());
                unit.GetComponent<TacticsMovement>().BeginTurn();
                return;
            }
            else
            {
                EndTurn();
            }
        }
    }

    public static void RemoveUnit(Unit unit)
    {
        Debug.Log("remove code would be called");
    }

    void AddUnitMidCombat()
    {
        //This is likely to be a lot more complex. the below throws an error. 

        /*foreach (TacticsMovement u in sortedUnits)
        {
            if (u.currentInitiative > initiativeCount)
            {
                sortedUnits.Remove(u);
                sortedUnits.Add(u);
            }
        }*/
    }
}
