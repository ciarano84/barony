using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterData : AspectData
{
    public override void SetAspectData(UnitInfo unit)
    {
        className = "Hunter";

        //Do we need this next line, isn't it just setting A to be A?
        unitInfo = unit;

        unit.mainWeaponData = new ShortbowData();
        unit.offHandData = new BlankItemData();
        unit.armourData = new LeatherArmourData();
        unit.accessory1 = new BlankItemData();
        unit.accessory2 = new BlankItemData();
    }

    public override void Level1()
    {
        unitInfo.baseAttack += 1;
        unitInfo.baseDefence += 1;
        unitInfo.baseMove += 1;
    }

    public override void GetAspect(Unit unit)
    {
        Hunter hunterAspect = unit.gameObject.AddComponent<Hunter>();
        hunterAspect.owner = unit;
    }

    //this needs assigning to a new visual. 
    public override GameObject GetVisual() { return GameAssets.i.ScoutVisual; }
}

public class Hunter : Aspect
{
    public override void GetAspectData()
    {
        aspectData = new HunterData();
    }
}
