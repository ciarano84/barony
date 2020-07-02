using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerData : MeleeWeaponData
{
    public override Sprite SetImage()
    {
        return GameAssets.i.Dagger;
    }

    public override void SetData(UnitInfo unitInfo, Slot slotToEquipTo = Slot.mainHand)
    {
        name = "Dagger";
        actionsPerAttack = 1;
        slot = Slot.oneHanded;
        rangeType = Range.melee;
        weight = Weight.light;
        description = "A fancy dagger. \r\nYou're not sure how it got here. Someone probably wanted shot of it quick.";

        weaponDamage = -1;

        if (unitInfo != null)
        {
            switch (slotToEquipTo)
            {
                case Slot.mainHand:
                    unitInfo.mainWeaponData = this;
                    break;
                case Slot.offHand:
                    unitInfo.offHandData = this;
                    break;
                default:
                    unitInfo.mainWeaponData = this;
                    break;
            }
        }
    }

    public override void EquipItem(Unit unit)
    {
        Dagger weapon = unit.gameObject.AddComponent<Dagger>();
        weapon.owner = unit.gameObject.GetComponent<PlayerCharacter>();
        weapon.weaponData = this;

        unit.mainWeapon = weapon;

        //Will have to have an amendment when we get off hand weapons. 
    }
}

public class Dagger : MeleeWeapon
{
    public override void GetItemData()
    {
        itemData = new DaggerData();
    }
}
