using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        Debug.Log(actioningUnit);
        Debug.Log(actioningUnit.GetComponent<RangedWeapon>());
        Debug.Log(actioningUnit.GetComponent<RangedWeapon>().rangedWeaponData);
        
        
        if (actioningUnit.GetComponent<RangedWeapon>().rangedWeaponData.currentAmmo < actioningUnit.GetComponent<RangedWeapon>().rangedWeaponData.maxAmmo
            && actioningUnit.GetComponent<TacticsMovement>().remainingMove == actioningUnit.unitInfo.currentMove)
        {
            return true;
        }
            
        else { return false; }
    }

    public override Sprite SetImage()
    {
        return GameAssets.i.Reload;
    }
}
