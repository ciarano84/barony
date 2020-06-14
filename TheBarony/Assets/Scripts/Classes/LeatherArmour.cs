using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeatherArmourData : ItemData
{
    public override void SetData()
    {
        imageFile = "LeatherArmour";
    }

    public override void EquipItem(Unit unit)
    {
        LeatherArmour shield = unit.gameObject.AddComponent<LeatherArmour>();
        shield.owner = unit.gameObject.GetComponent<PlayerCharacter>();
        unit.unitInfo.currentToughness += 1;
    }
}

public class LeatherArmour : Item
{
}
