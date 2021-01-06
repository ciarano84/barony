﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrecisionStrike : Action
{
    Weapon.Target target;
    
    public override void SetActionButtonData(Unit unit)
    {
        actioningUnit = unit;
        buttonText = "Precision Strike";
        actionCost = ActionCost.main;
    }

    public override void ExecuteAction(ActionCost actionCost)
    {
        UIManager.RequestConfirmation("Precision Strike \n Attack with a bonus and increased crit range against focus. Costs 2 breath and leaves exposed.", "Confirm", "Cancel");
        //move camera to focus on the target. 
        ConfirmationPopUp.onConfirm += OnConfirm;
        ConfirmationPopUp.onCancel += OnCancel;
    }

    //this bool is used to decide if the action is avialable to the player or not. 
    public override bool CheckAvailable()
    {
        if (actioningUnit.unitInfo.currentBreath >= 2)
        {
            if (actioningUnit.focus != null)
            {
                if (actioningUnit.GetComponent<TacticsMovement>().remainingActions >= 1)
                {
                    foreach (Weapon.Target t in actioningUnit.mainWeapon.targets)
                    {
                        if (t.unitTargeted == actioningUnit.focus)
                        {
                            if (RangeFinder.LineOfSight(actioningUnit, t.unitTargeted))
                            {
                                target = t;
                                return true;
                            }
                        }
                    }
                }
            }
        }
        target = null;
        return false;
    }

    public override Sprite SetImage()
    {
        return GameAssets.i.PrecisionStrikeIcon;
    }

    public void OnConfirm()
    {
        //Do the strike
        actioningUnit.gameObject.GetComponent<UnitPopUpManager>().AddPopUpInfo("Precision Strike");
        actioningUnit.UpdateBreath(-2, true);
        Initiative.queuedActions += actioningUnit.mainWeapon.weaponData.actionsPerAttack;
        Unsubscribe();
        AttackManager.OnAttack += BoostAttack;
        actioningUnit.mainWeapon.StartCoroutine("Attack", target);

        //add exposed effect
        if (actioningUnit.GetComponent<Exposed>() == null)
        {
            Exposed exposed = actioningUnit.gameObject.AddComponent<Exposed>();
            exposed.AddEffect(actioningUnit.gameObject, GameEvent.TURNSTART);
        }
        else if (!actioningUnit.GetComponent<Exposed>().enabled)
        {
            actioningUnit.GetComponent<Exposed>().enabled = true;
            actioningUnit.GetComponent<Exposed>().AddEffect(actioningUnit.gameObject);
        }
        //Initiative.EndAction();
    }

    public void OnCancel()
    {
        Unsubscribe();
        target = null;
    }

    public void BoostAttack(Unit a, Unit d)
    {
        if (a == actioningUnit)
        {
            if (d == target.unitTargeted)
            {
                Debug.Log("precision attack added");
                AttackManager.bonuses++;
                AttackManager.critRange += 1;
                Unsubscribe();
            }
        }
    }

    public override void Unsubscribe()
    {
        AttackManager.OnAttack -= BoostAttack;
        ConfirmationPopUp.onConfirm -= OnConfirm;
        ConfirmationPopUp.onCancel -= OnCancel;
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }
}