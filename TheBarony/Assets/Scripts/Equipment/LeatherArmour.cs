﻿using System.Collections;
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
        weight = Weight.light;
        description = "A rough, but thick leather jerkin.\r\nYour not entirely sure what it smells of, but it is definitely not you.";

        if (unitInfo != null)
        {
            unitInfo.armourData = this;
        }
    }

    public override void EquipItem(Unit unit)
    {
        LeatherArmour armour = unit.gameObject.AddComponent<LeatherArmour>();
        armour.owner = unit.gameObject.GetComponent<TacticsMovement>();
        armour.itemData = this;
        unit.unitInfo.currentArmour += 2;
    }
}

public class LeatherArmour : Item
{
    public override void GetItemData()
    {
        itemData = new LeatherArmourData();
    }
}
