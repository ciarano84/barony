using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curse : Action
{
    public override void SetActionButtonData(Unit unit)
    {
        actioningUnit = unit;
        buttonText = "Curse";
        actionCost = ActionCost.main;
    }

    public override void ExecuteAction(ActionCost actionCost)
    {
        UIManager.RequestConfirmation("Curse \n Focus becomes cursed, granting a bonus to all attacks against them until you lose focus. Costs 1 breath.", "Confirm", "Cancel");
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
                if (actioningUnit.focus != null)
                {
                    return true;
                } 
            }
        }
        return false;
    }

    public override Sprite SetImage()
    {
        return GameAssets.i.CurseIcon;
    }

    public void OnConfirm()
    {
        Initiative.queuedActions++;

        actioningUnit.focus.gameObject.GetComponent<UnitPopUpManager>().AddPopUpInfo("Curse");
        actioningUnit.UpdateBreath(-1, true);

        if (actioningUnit.GetComponent<Cursed>() == null)
        {
            Cursed cursed = actioningUnit.focus.gameObject.AddComponent<Cursed>();
            cursed.AddEffect(actioningUnit.focus.gameObject);
            cursed.cursingUnit = actioningUnit;
        }
        else if (!actioningUnit.GetComponent<Cursed>().enabled)
        {
            actioningUnit.focus.GetComponent<Cursed>().enabled = true;
            actioningUnit.focus.GetComponent<Cursed>().AddEffect(actioningUnit.gameObject);
            actioningUnit.GetComponent<Cursed>().cursingUnit = actioningUnit;
        }

        actioningUnit.GetComponent<TacticsMovement>().remainingActions -= 1;

        actioningUnit.TriggerAnimation("curse");

        Initiative.EndAction();
    }

    public void OnCancel() { }

    public override void Unsubscribe()
    {
        //Do unsubscribes if there are any. 
    }
}
