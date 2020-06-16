using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutData : AspectData
{
    public override void SetAspectData()
    {
        className = "Scout";
    }

    public override void Level1(Unit unit)
    {
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
