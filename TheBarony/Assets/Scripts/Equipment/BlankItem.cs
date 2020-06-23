﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlankItemData : ItemData
{
    public override Sprite SetImage()
    {
        return GameAssets.i.BlankItem;
    }

    public override void SetData(UnitInfo unitInfo)
    {
        name = "None";
    }
    

    public override void EquipItem(Unit unit) { }

}
