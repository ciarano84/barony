using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortbowData : RangedWeaponData
{
    public override void SetData(UnitInfo unitInfo)
    {
        imageFile = "Shortbow";
        unitInfo.offHandData = null;
    }

    public override void EquipItem(Unit unit)
    {
        RangedWeapon weapon = unit.gameObject.AddComponent<Shortbow>();
        weapon.owner = unit.gameObject.GetComponent<PlayerCharacter>();
        unit.unitInfo.weaponData = this;
        unit.currentWeapon = weapon;
        weapon.rangeType = Weapon.Range.ranged;
        weapon.range = 200;
        weapon.actionsPerAttack = 1;
        weapon.missDistance = 20;
        weapon.maxAmmo = 1;
        weapon.currentAmmo = 1;
        unit.unitInfo.currentDamage = 2;

        //Set up for an action. 
        Reload reload = new Reload();
        reload.SetActionButtonData(unit);
        unit.actions.Add(reload);
    }
}

public class Shortbow : RangedWeapon
{
}
