using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor.ProjectWindowCallback;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.WSA.Input;

public class Initiative : MonoBehaviour
{
    public static int queuedActions;
    
    //I can likely strip this down to just be a list and a queue. 
    static List<TacticsMovement> unsortedUnits = new List<TacticsMovement>();
    public static List<TacticsMovement> sortedUnits = new List<TacticsMovement>();
    public static Queue<TacticsMovement> order = new Queue<TacticsMovement>();

    //static bool combatStarted = false;
    public static TacticsMovement currentUnit; 
    static ActionUIManager actionUIManager;

    public static Initiative initiativeManager;
    public GameObject selector;


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
        GameObject selector = GameObject.FindGameObjectWithTag("selector");
        selector.transform.SetParent(currentUnit.transform, false);
        actionUIManager.UpdateActions(currentUnit.GetComponent<PlayerCharacter>());
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
        queuedActions--;
        if (queuedActions > 0)
        {
            yield break;
        }
        else
        {
            yield return new WaitForSeconds(1f);
            if (currentUnit.remainingMove > 0 || currentUnit.remainingActions > 0)
            {
                actionUIManager.UpdateActions(currentUnit.GetComponent<PlayerCharacter>());
                currentUnit.GetComponent<TacticsMovement>().BeginTurn();
                yield break;
            }
            else
            {
                EndTurn();
                yield break;
            }
        }
    }

    public static void RemoveUnit(Unit unit)
    {
        order = new Queue<TacticsMovement>(order.Where(x => x != unit));
        Destroy(unit.gameObject);
        EndAction();
    }

    public static void EndAction()
    {
        initiativeManager.StartCoroutine(CheckForTurnEnd());
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
