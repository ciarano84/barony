﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Initiative : MonoBehaviour
{
    public static int queuedActions = 0;
    public int publicQueuedActions;

    
    //I can likely strip this down to just be a list and a queue. 
    static List<TacticsMovement> unsortedUnits = new List<TacticsMovement>();
    public static List<TacticsMovement> sortedUnits = new List<TacticsMovement>();
    public static Queue<TacticsMovement> order = new Queue<TacticsMovement>();

    //Faction Lists for other scripts to use. 
    public static List<Unit> players = new List<Unit>();
    public static List<Unit> enemies = new List<Unit>();

    public static TacticsMovement currentUnit; 
    static ActionUIManager actionUIManager;

    public static Initiative initiativeManager;
    public GameObject selector;

    //OnEncounterStart Delegate. 
    public delegate void OnEncounterStartDelegate(Unit unit);
    public static OnEncounterStartDelegate OnEncounterStart;


    private void Update()
    {
        publicQueuedActions = queuedActions;
    }

    public void Awake()
    {
        initiativeManager = this;
    }

    private void Start()
    { 
        actionUIManager = FindObjectOfType<ActionUIManager>();
        selector = GameObject.FindGameObjectWithTag("selector");
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
        OnEncounterStart(order.Peek()); //Alert all that the encounter has started. 

        foreach (Unit u in order)
        {
            if (u.unitInfo.faction == Factions.players) players.Add(u);
            if (u.unitInfo.faction == Factions.enemies) enemies.Add(u);
        }

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
        GameObject selector = GameObject.FindGameObjectWithTag("selector");
        selector.transform.SetParent(currentUnit.transform, false);
        actionUIManager.UpdateActions(currentUnit.GetComponent<TacticsMovement>());
    }

    public static void EndTurn()
    {
        TacticsMovement unit = order.Dequeue();
        unit.EndTurn();
        order.Enqueue(unit);
        StartTurn();
    }

    public void UpdateUI()
    {
        
    }

    public static IEnumerator CheckForTurnEnd() 
    {
        if (queuedActions > 1)
        {
            queuedActions--;
            yield break;
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            if (currentUnit.remainingMove > 0 || currentUnit.remainingActions > 0)
            {
                currentUnit.GetComponent<TacticsMovement>().BeginTurn();
                actionUIManager.UpdateActions(currentUnit.GetComponent<TacticsMovement>());
                queuedActions--;
                yield break;
            }
            else
            {
                if (currentUnit.focusSwitched == false)
                {
                    currentUnit.canFocusSwitch = true;
                    queuedActions--;
                }
                else
                {
                    EndTurn();
                    queuedActions--;
                    yield break;
                }
            }
        }
    }

    public static void RemoveUnit(Unit unit)
    {
        order = new Queue<TacticsMovement>(order.Where(x => x != unit));

        //ensure the unit is removed from the relevant faction lists. 
        if (unit.unitInfo.faction == Factions.players) players.Remove(unit);
        if (unit.unitInfo.faction == Factions.enemies) enemies.Remove(unit);
        
        Destroy(unit.gameObject);
        EncounterManager.CheckForFactionDeath();
        EndAction();
    }

    public static void EndAction()
    {
        initiativeManager.StartCoroutine(CheckForTurnEnd());
    }
}
