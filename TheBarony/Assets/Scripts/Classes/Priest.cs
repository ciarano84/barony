using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestData : AspectData
{
    public override void SetAspectData()
    {
        className = "Priest";
    }

    public override void Level1(Unit unit)
    {

    }

    public override void GetAspect(Unit unit)
    {
        Priest priestAspect = unit.gameObject.AddComponent<Priest>();
        priestAspect.owner = unit;
    }

    public override GameObject GetVisual() { return GameAssets.i.PriestVisual; }
}

public class Priest : Aspect
{
    
}
