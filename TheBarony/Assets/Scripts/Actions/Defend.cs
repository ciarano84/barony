using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defend : Action
{
    public override void SetActionButtonData(Unit unit)
    {
        actioningUnit = unit;
        buttonText = "Defend";
        actionCost = ActionCost.main;
    }

    public override void ExecuteAction(ActionCost actionCost)
    {
        Initiative.queuedActions++;

        if (actioningUnit.GetComponent<Defending>() == null)
        {
            Defending defending = actioningUnit.gameObject.AddComponent<Defending>();
            defending.AddEffect(actioningUnit.gameObject);
        }
        else if (!actioningUnit.GetComponent<Defending>().enabled)
        {
            actioningUnit.GetComponent<Defending>().enabled = true;
            actioningUnit.GetComponent<Defending>().AddEffect(actioningUnit.gameObject);
        }

        actioningUnit.GetComponent<TacticsMovement>().remainingActions -= 1;
        Initiative.EndAction();
    }

    //this bool is used to decide if the action is avialable to the player or not. 
    public override bool CheckAvailable()
    {
        if (actioningUnit.GetComponent<Defending>() != null)
        {
            if (actioningUnit.GetComponent<Defending>().enabled == true)
            {
                return false;
            }
        }
        
        if (actioningUnit.unitInfo.currentBreath > 0)
        {
            return true;
        }

        else { return false; }
    }

    public override Sprite SetImage()
    {
        return GameAssets.i.Defend;
    }
}
