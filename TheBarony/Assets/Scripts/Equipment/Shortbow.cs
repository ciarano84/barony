﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortbowData : RangedWeaponData
{
    public override Sprite SetImage()
    {
        return GameAssets.i.ShortBow;
    }

    public override void SetData(UnitInfo unitInfo, Slot slotToEquipTo = Slot.mainHand)
    {
        name = "Shortbow";
        actionsPerAttack = 1;
        slot = Slot.twoHanded;
        rangeType = WeaponData.Range.ranged;
        weight = Weight.light;
        description = "A potentially ornemantal shortbow. \r\nLiable to break. But hopefully liable to hurt someone at least once beforehand.";
        
        range = 200;
        missDistance = 20;
        maxAmmo = 1;
        currentAmmo = 1;
        rangedDamage = 2;

        if (unitInfo != null)
        {
            unitInfo.mainWeaponData = this;
        }
    }

    public override void EquipItem(Unit unit)
    {
        Shortbow weapon = unit.gameObject.AddComponent<Shortbow>();
        weapon.owner = unit.gameObject.GetComponent<PlayerCharacter>();
        weapon.rangedWeaponData = this;
        //this next line looks like it's not needed BUT IT REALLY IS
        weapon.weaponData = this;
        unit.mainWeapon = weapon;

        //Set up for an action. 
        Reload reload = new Reload();
        reload.SetActionButtonData(unit);
        unit.actions.Add(reload);
    }
}

public class Shortbow : RangedWeapon
{
    public override void GetItemData()
    {
        itemData = new ShortbowData();
    }
}
