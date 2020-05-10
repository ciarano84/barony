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

    private void Start()
    {
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

        //Debugging
        foreach (TacticsMovement x in order)
        {
            Debug.Log("unit with initiative of " + x.currentInitiative);
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
        ActionUIManager.UpdateActions(currentUnit);
    }

    public static void EndTurn()
    {
        TacticsMovement unit = order.Dequeue();
        unit.EndTurn();
        StartTurn();
        //Unsure if this will work as intended. 
        order.Enqueue(unit);
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
