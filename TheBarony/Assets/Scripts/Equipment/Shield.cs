using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldData : ItemData
{
    public override void SetData(UnitInfo unitInfo)
    {
        imageFile = "Shield";
    }

    public override void EquipItem(Unit unit)
    {
        Shield shield = unit.gameObject.AddComponent<Shield>();
        shield.owner = unit.gameObject.GetComponent<PlayerCharacter>();
        unit.unitInfo.currentDefence = unit.unitInfo.baseDefence + 2;
    }
}

public class Shield : Item
{
    public int defendModifier = 2;
}
