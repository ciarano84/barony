using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainShirtArmourData : ItemData
{
    public override Sprite SetImage()
    {
        return GameAssets.i.ChainShirtArmour;
    }

    //looks like there is a mistake in the parameters here. 
    public override void SetData(UnitInfo unitInfo, Slot slotToEquipTo = Slot.offHand)
    {
        name = "Chain shirt armour";
        slot = Slot.armour;
        weight = Weight.medium;
        description = "A shoddy and somewhat ragged chain shirt.\r\n an ancient suit of armour with the power to chaff no matter how many layers you wear.";

        if (unitInfo != null)
        {
            unitInfo.armourData = this;
        }
    }

    public override void EquipItem(Unit unit)
    {
        ChainShirtArmour armour = unit.gameObject.AddComponent<ChainShirtArmour>();
        armour.owner = unit.gameObject.GetComponent<TacticsMovement>();
        armour.itemData = this;
        unit.unitInfo.currentArmour += 3;
    }
}

public class ChainShirtArmour : Item
{
    public override void GetItemData()
    {
        itemData = new ChainShirtArmourData();
    }
}
