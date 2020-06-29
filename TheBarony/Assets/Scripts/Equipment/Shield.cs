using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldData : ItemData
{
    public int defendModifier;

    public override Sprite SetImage()
    {
        return GameAssets.i.Shield;
    }

    public override void SetData(UnitInfo unitInfo, Slot slotToEquipTo = Slot.offHand)
    {
        name = "Shield";
        slot = Slot.offHand;
        weight = Weight.medium;
        description = "A shield made up largely of planks. \r\nThe nails sticking out of it would be a real benefit if they weren't coming out both sides.";
        defendModifier = 2;

        if (unitInfo != null)
        {
            unitInfo.offHandData = this;
        }
    }

    public override void EquipItem(Unit unit)
    {
        Shield shield = unit.gameObject.AddComponent<Shield>();
        shield.owner = unit.gameObject.GetComponent<PlayerCharacter>();
        shield.itemData = this;
        unit.unitInfo.currentDefence = unit.unitInfo.baseDefence + defendModifier;
    }
}

public class Shield : Item
{
    public override void GetItemData()
    {
        itemData = new ShieldData();
    }
}
