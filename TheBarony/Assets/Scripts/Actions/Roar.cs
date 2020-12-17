using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Roar : Action
{
    int range = 3;
    
    public override void SetActionButtonData(Unit unit)
    {
        actioningUnit = unit;
        buttonText = "Roar";
        actionCost = ActionCost.main;
    }

    public override void ExecuteAction(ActionCost actionCost)
    {
        UIManager.RequestConfirmation("Roar \n Become the focus of all enemies within 3 tiles and defend. Costs 1 breath and cancels focus.", "Confirm", "Cancel");
        ConfirmationPopUp.onConfirm += OnConfirm;
        ConfirmationPopUp.onCancel += OnCancel;
    }

    //this bool is used to decide if the action is avialable to the player or not. 
    public override bool CheckAvailable()
    {
        if (actioningUnit.unitInfo.currentBreath >= 1)
        {
            if (actioningUnit.GetComponent<TacticsMovement>().remainingActions >= 1)
            {
                Debug.Log("criteria met");
                return true;
            }
        }
        return false;
    }

    public override Sprite SetImage()
    {
        return GameAssets.i.RoarIcon;
    }

    public void OnConfirm()
    {
        Initiative.queuedActions++;

        actioningUnit.gameObject.GetComponent<UnitPopUpManager>().AddPopUpInfo("Roar");
        actioningUnit.UpdateBreath(1, true);
        actioningUnit.focus = null;
        
        foreach(Unit u in Initiative.order)
        {
            if (u.unitInfo.faction != actioningUnit.unitInfo.faction)
            {
                if (RangeFinder.IsItWtihinXTiles(actioningUnit, u, range, true))
                {
                    if (u.focus != actioningUnit) u.focus = actioningUnit;    
                }
            }
        }

        //need to replace this. 

        //if (actioningUnit.GetComponent<Defending>() == null)
        //{
        //    Defending defending = actioningUnit.gameObject.AddComponent<Defending>();
        //    defending.AddEffect(actioningUnit.gameObject);
        //}
        //else if (!actioningUnit.GetComponent<Defending>().enabled)
        //{
        //    actioningUnit.GetComponent<Defending>().enabled = true;
        //    actioningUnit.GetComponent<Defending>().AddEffect(actioningUnit.gameObject);
        //}

        actioningUnit.GetComponent<TacticsMovement>().remainingActions -= 1;

        actioningUnit.TriggerAnimation("roar");

        Initiative.EndAction();
    }

    public void OnCancel() { }

    public override void Unsubscribe() 
    {
    //Do unsubscribes if there are any. 
    }
}
