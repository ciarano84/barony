using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.WSA.Input;

public class Initiative : MonoBehaviour
{
    //Action bool is TRUE when events are unfolding in the scene, and the intiative order needs to wait for them before moving on. 
    public static bool action = true;
    
    static List<TacticsMovement> unsortedUnits = new List<TacticsMovement>();
    public static List<TacticsMovement> sortedUnits = new List<TacticsMovement>();
    public static Queue<TacticsMovement> order = new Queue<TacticsMovement>();

    public static TacticsMovement currentUnit; 

    //trying out the delegate feature - this should have action manager, tiles and weapon subscribe.  
    public delegate void OnAwaitPlayerInput();
    public static OnAwaitPlayerInput onAwaitPlayerInput;

    private void Start()
    {
        StartCoroutine("StartEncounter");
    }

    private void Update()
    {
        //Debug.Log("current unit is " + currentUnit);
    }

    IEnumerator StartEncounter()
    {
        yield return new WaitForSeconds(2f);
        sortedUnits = unsortedUnits.OrderByDescending(o => o.currentInitiative).ToList();
        foreach (TacticsMovement u in sortedUnits)
        {
            order.Enqueue(u);
        }
        BeginTurn();
        yield break;
    }

    static void BeginTurn()
    {
        action = false;
        currentUnit = order.Peek();
        currentUnit.NextAction();
        onAwaitPlayerInput();
    }

    public static void EndTurn()
    {
        if (action) return;
        TacticsMovement unit = order.Dequeue();
        order.Enqueue(unit);
        unit.EndTurn();
        BeginTurn();
    }

    public static void ResumeAction()
    {
        action = true;
    }

    public static void CheckForTurnEnd() 
    {
        //Check its a player Character
        if ((currentUnit.GetComponent<PlayerCharacter>() != null))
        {
            currentUnit.RemoveSelectableTiles();
            if (currentUnit.remainingMove > 0 || currentUnit.remainingActions > 0)
            {
                action = false;
                onAwaitPlayerInput();
                currentUnit.GetComponent<TacticsMovement>().NextAction();
                return;
            }
            else
            {
                EndTurn();
            }
        }
    }

    public static void RemoveUnit(TacticsMovement unit)
    {
        order = new Queue<TacticsMovement>(order.Where(x => x != unit));
        Destroy(unit.gameObject);
        CheckForTurnEnd();
    }

    public static void AddUnit(TacticsMovement unit)
    {
        unsortedUnits.Add(unit);
    }
}
