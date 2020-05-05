using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public MovementManager movementManager;
    
    //proxy rosta of troops in lieu of a turn order that builds itself. 
    public GameObject troop1;
    public GameObject troop2;
    public GameObject troop3;
    private int turnNumber = 0;

    public GameObject selector;
    Selector selectorScript;

    private GameObject[] TurnOrder = new GameObject [5];
    private GameObject currentTurnTroop;

    private void Start()
    {
        selectorScript = selector.GetComponent<Selector>();
        PopulateTurnOrder();
        NewTurn();
    }

    public void NewTurn()
    {
        turnNumber++;
        if (turnNumber > TurnOrder.Length) { turnNumber = 1; }
        currentTurnTroop = TurnOrder[turnNumber];
        selectorScript.AssignSelector(currentTurnTroop);
        movementManager.UpdateTroop(currentTurnTroop);
        //the instruction to make it the agent for the navmesh
    }

    void PopulateTurnOrder() {
        //This will do for now. 
        TurnOrder[0] = troop1;
        TurnOrder[1] = troop1;
        TurnOrder[2] = troop2;
        TurnOrder[3] = troop3;
    }
}
