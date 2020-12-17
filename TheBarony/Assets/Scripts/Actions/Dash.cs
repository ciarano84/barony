using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : Action
{
    public override void SetActionButtonData(Unit unit)
    {
        actioningUnit = unit;
        buttonText = "Dash";
        actionCost = ActionCost.main;
    }

    public override void ExecuteAction(ActionCost actionCost)
    {
        Initiative.queuedActions++;
        actioningUnit.GetComponent<TacticsMovement>().remainingMove += actioningUnit.unitInfo.currentMove;
        actioningUnit.GetComponent<TacticsMovement>().remainingActions -= 1;
        actioningUnit.UpdateBreath(-1, true);
        Initiative.EndAction();
    }

    //this bool is used to decide if the action is avialable to the player or not. 
    public override bool CheckAvailable()
    {
        if (actioningUnit.unitInfo.currentBreath > 0)
        {
           return true;
        }

        else { return false; }
    }

    public override Sprite SetImage()
    {
        return GameAssets.i.Dash;
    }

    public override void Unsubscribe() { }
}
