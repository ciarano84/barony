using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeatherArmourData : ItemData
{
    public override Sprite SetImage()
    {
        return GameAssets.i.LeatherArmour;
    }

    public override void SetData(UnitInfo unitInfo, Slot slotToEquipTo = Slot.offHand)
    {
        name = "leather armour";
        slot = Slot.armour;
        description = "A rough, but thick leather jerkin.\r\nYour not entirely sure what it smells of, but some of it is definitely not you.";

        //Create an instance
        LeatherArmourData leatherArmourData = new LeatherArmourData();
        if (unitInfo != null)
        {
            unitInfo.armourData = leatherArmourData;
        }
    }

    public override void EquipItem(Unit unit)
    {
        LeatherArmour shield = unit.gameObject.AddComponent<LeatherArmour>();
        shield.owner = unit.gameObject.GetComponent<PlayerCharacter>();
        shield.itemData = this;
        unit.unitInfo.currentToughness += 1;
    }
}

public class LeatherArmour : Item
{
}
