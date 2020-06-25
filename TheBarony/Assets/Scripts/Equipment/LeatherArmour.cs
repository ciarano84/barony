using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeatherArmourData : ItemData
{
    public override Sprite SetImage()
    {
        return GameAssets.i.LeatherArmour;
    }

    public override void SetData(UnitInfo unitInfo)
    {
        name = "leather armour";
        slot = Slot.armour;
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
