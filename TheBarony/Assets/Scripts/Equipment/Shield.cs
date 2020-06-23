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

    public override void SetData(UnitInfo unitInfo)
    {
        name = "Shield";
        defendModifier = 2;
    }

    public override void EquipItem(Unit unit)
    {
        Shield shield = unit.gameObject.AddComponent<Shield>();
        shield.owner = unit.gameObject.GetComponent<PlayerCharacter>();
        unit.unitInfo.currentDefence = unit.unitInfo.baseDefence + defendModifier;
    }
}

public class Shield : Item
{
}
