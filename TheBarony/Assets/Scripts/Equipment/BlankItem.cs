﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlankItemData : ItemData
{
    public override Sprite SetImage()
    {
        return GameAssets.i.BlankItem;
    }

    public override void SetData(UnitInfo unitInfo, Slot slotToEquipTo = Slot.accessory1)
    {
        name = "None";
        slot = Slot.accessory;

        //Create an instance
        if (unitInfo != null)
        {
            switch (slotToEquipTo)
            {
                case Slot.offHand:
                    unitInfo.offHandData = this;
                    break;
                case Slot.armour:
                    unitInfo.armourData = this;
                    break;
                case Slot.accessory1:
                    unitInfo.accessory1 = this;
                    break;
                default:
                    unitInfo.accessory2 = this;
                    break;
            }
        }
    }

    public override void EquipItem(Unit unit) { }
}
