using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class Initiative : MonoBehaviour
{
    //I can likely strip this down to just be a list and a queue. 

    static List<TacticsMovement> unsortedUnits = new List<TacticsMovement>();
    static List<TacticsMovement> sortedUnits = new List<TacticsMovement>();
    static Queue<TacticsMovement> order = new Queue<TacticsMovement>();

    static bool combatStarted = false;
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
        
        combatStarted = true;
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
        actionUIManager.UpdateActions(currentUnit);
    }

    public static void EndTurn()
    {
        //Debug.Log("EndTurn with current unit as " + order.Peek().name);
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
                return;
            }
            else
            {
                EndTurn();
                Debug.Log("reached");
            }
        }
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
