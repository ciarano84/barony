using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortswordData : MeleeWeaponData
{
    public override Sprite SetImage()
    {
        return GameAssets.i.ShortSword;
    }

    public override void SetData(UnitInfo unitInfo, Slot slotToEquipTo = Slot.mainHand)
    {
        name = "Shortsword";
        actionsPerAttack = 1;
        rangeType = Range.melee;
        slot = Slot.oneHanded;
        weight = Weight.medium;
        description = "A short sword. \r\nWell made but really nothing to write home about.";

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
        Shortsword weapon = unit.gameObject.AddComponent<Shortsword>();
        weapon.owner = unit.gameObject.GetComponent<PlayerCharacter>();
        weapon.weaponData = this; 
        
        unit.mainWeapon = weapon;

        //Will have to have an amendment when we get off hand weapons. 
    }
}

public class Shortsword : MeleeWeapon
{
    public override void GetItemData()
    {
        itemData = new ShortswordData();
    }
}
