using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortbowData : RangedWeaponData
{
    public override Sprite SetImage()
    {
        return GameAssets.i.ShortBow;
    }

    public override void SetData(UnitInfo unitInfo)
    {
        name = "Shortbow";
        //unitInfo.offHandData = null;
        rangeType = WeaponData.Range.ranged;
        range = 200;
        actionsPerAttack = 1;
        missDistance = 20;
        maxAmmo = 1;
        currentAmmo = 1;
        rangedDamage = 2;
    }

    public override void EquipItem(Unit unit)
    {
        Shortbow weapon = unit.gameObject.AddComponent<Shortbow>();
        weapon.owner = unit.gameObject.GetComponent<PlayerCharacter>();
        weapon.rangedWeaponData = this;
        weapon.weaponData = this;
        unit.mainWeapon = weapon;
        //unit.mainWeapon.rangedWeaponData = this;

        //Set up for an action. 
        Reload reload = new Reload();
        reload.SetActionButtonData(unit);
        unit.actions.Add(reload);
    }
}

public class Shortbow : RangedWeapon
{
}
