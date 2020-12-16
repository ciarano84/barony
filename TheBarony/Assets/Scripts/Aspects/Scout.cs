using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutData : AspectData
{
    public override void SetAspectData(UnitInfo unit)
    {
        className = "Scout";

        //Do we need this next line, isn't it just setting A to be A?
        unitInfo = unit;

        unit.mainWeaponData = new LongbowData();
        unit.offHandData = new BlankItemData();
        unit.armourData = new BlankItemData();
        unit.accessory1 = new BlankItemData();
        unit.accessory2 = new BlankItemData();
    }

    public override void Tier1()
    {
        unitInfo.baseAttack += 1;
        unitInfo.baseDefence += 1;
        unitInfo.baseMove += 1;
    }

    public override void Tier2()
    {
        throw new System.NotImplementedException();
    }

    public override void GetAspect(Unit unit)
    {
        Scout scoutAspect = unit.gameObject.AddComponent<Scout>();
        scoutAspect.owner = unit;
    }

    public override Mesh GetVisual() { return GameAssets.i.LightArmourMesh; }
}

public class Scout : Aspect
{
    public override void GetAspectData()
    {
        aspectData = new ScoutData();
    }
}
