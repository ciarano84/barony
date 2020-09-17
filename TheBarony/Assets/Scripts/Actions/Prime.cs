using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prime : Action
{
    public override void SetActionButtonData(Unit unit)
    {
        actioningUnit = unit;
        buttonText = "Prime";
        actionCost = ActionCost.main;
    }

    public override void ExecuteAction(ActionCost actionCost)
    {
        Initiative.queuedActions++;
        DamagePopUp.Create(actioningUnit.transform.position + new Vector3(0, (actioningUnit.gameObject.GetComponent<TacticsMovement>().halfHeight) + 0.5f), "Attack Primed", false);

        if (actioningUnit.GetComponent<Priming>() == null)
        {
            Priming defending = actioningUnit.gameObject.AddComponent<Priming>();
            defending.AddEffect(actioningUnit.gameObject);
        }
        else if (!actioningUnit.GetComponent<Priming>().enabled)
        {
            actioningUnit.GetComponent<Priming>().enabled = true;
            actioningUnit.GetComponent<Priming>().AddEffect(actioningUnit.gameObject);
        }

        actioningUnit.GetComponent<TacticsMovement>().remainingActions -= 1;
        Initiative.EndAction();
    }

    //this bool is used to decide if the action is avialable to the player or not. 
    public override bool CheckAvailable()
    {
        if (actioningUnit.GetComponent<Priming>() != null)
        {
            if (actioningUnit.GetComponent<Priming>().enabled == true) return false;
        }

        if (actioningUnit.focus == null) return false;
        if (!RangeFinder.LineOfSight(actioningUnit, actioningUnit.focus)) return false;

        if (actioningUnit.mainWeapon.weaponData.rangeType == WeaponData.Range.ranged) { }
        else if (actioningUnit.mainWeapon.weaponData.rangeType == WeaponData.Range.melee && actioningUnit.mainWeapon.weaponData.weight >= ItemData.Weight.heavy) { }
        else return false;

        if (actioningUnit.unitInfo.currentBreath > 0) return true;

        else { return false; }
    }

    public override Sprite SetImage()
    {
        return GameAssets.i.PrimingIcon;
    }
}
