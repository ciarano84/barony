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
        actionCost = rangedWeapon.rangedWeaponData.reloadSpeed; 
    }

    public override void ExecuteAction(ActionCost actionCost)
    {
        rangedWeapon.Reload(actionCost);
    }

    //this bool is used to decide if the action is avialable to the player or not. 
    public override bool CheckAvailable()
    {
        if (actioningUnit.GetComponent<RangedWeapon>().rangedWeaponData.currentAmmo < actioningUnit.GetComponent<RangedWeapon>().rangedWeaponData.maxAmmo)
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
