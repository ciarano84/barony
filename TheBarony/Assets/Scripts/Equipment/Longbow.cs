using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongbowData : RangedWeaponData
{
    public override Sprite SetImage()
    {
        return GameAssets.i.Longbow;
    }

    public override void SetData(UnitInfo unitInfo, Slot slotToEquipTo = Slot.mainHand)
    {
        name = "Longbow";
        actionsPerAttack = 1;
        slot = Slot.twoHanded;
        rangeType = WeaponData.Range.ranged;
        weight = Weight.medium;
        description = "A potentially ornemantal longbow. \r\nIt is indeed long. A day of carrying this around and you'll wish you'd just been shot by it.";

        range = 200;
        missDistance = 20;
        maxAmmo = 1;
        currentAmmo = 1;
        rangedDamage = 4;
        reloadSpeed = ActionCost.main;

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

public class Longbow : RangedWeapon
{
    public override void GetItemData()
    {
        itemData = new LongbowData();
    }
}
