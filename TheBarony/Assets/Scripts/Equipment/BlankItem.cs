using System.Collections;
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
    }

    public override void EquipItem(Unit unit) { }
}
