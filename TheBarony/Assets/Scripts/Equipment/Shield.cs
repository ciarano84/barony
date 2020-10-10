using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldData : ItemData
{
    public int shieldModifier;

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
        shieldModifier = 2;

        if (unitInfo != null)
        {
            unitInfo.offHandData = this;
        }
    }

    public override void EquipItem(Unit unit)
    {
        GameObject model = GameObject.Instantiate(GameAssets.i.ShieldModel, unit.offHandSlot);
        model.transform.position = unit.offHandSlot.position;

        Shield shield = unit.gameObject.AddComponent<Shield>();
        shield.owner = unit.gameObject.GetComponent<TacticsMovement>();
        shield.itemData = this;
        //shielding as an event is handled by the defend method in tactics movement. 
    }
}

public class Shield : Item
{
    public override void GetItemData()
    {
        itemData = new ShieldData();
    }
}
