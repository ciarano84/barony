using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload : Action
{
    //needs some scrutiny that it follows the right proceedure with regards to action counting, queued actions etc. 
    
    RangedWeapon rangedWeapon;

    public override void SetActionButtonData(Unit unit)
    {
        actioningUnit = unit;
        buttonText = "Reload";
        rangedWeapon = unit.gameObject.GetComponent<RangedWeapon>();
    }

    public override void ExecuteAction()
    {
        rangedWeapon.Reload();
    }

    //this bool is used to decide if the action is avialable to the player or not. 
    public override bool CheckAvailable()
    {
        if (actioningUnit.GetComponent<RangedWeapon>().currentAmmo < actioningUnit.GetComponent<RangedWeapon>().maxAmmo
            && actioningUnit.GetComponent<TacticsMovement>().remainingMove == actioningUnit.unitInfo.move) return true;
        else { return false; }
    }
}
