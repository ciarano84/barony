using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload : Action
{
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
}
