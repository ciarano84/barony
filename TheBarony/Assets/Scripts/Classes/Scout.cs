﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutData : AspectData
{
    public override void SetAspectData(UnitInfo unit)
    {
        className = "Scout";
        unitInfo = unit;
    }

    public override void Level1()
    {
        unitInfo.baseAttack += 1;
        unitInfo.baseDefence += 1;
        unitInfo.baseMove += 1;
    }

    public override void GetAspect(Unit unit)
    {
        Scout scoutAspect = unit.gameObject.AddComponent<Scout>();
        scoutAspect.owner = unit;
    }

    public override GameObject GetVisual() { return GameAssets.i.ScoutVisual; }
}

public class Scout : Aspect
{
    
}
