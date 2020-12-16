using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BruteData : AspectData
{
    public override void SetAspectData(UnitInfo unit)
    {
        unitInfo = unit;
        unit.mainWeaponData = new GreataxeData();
        unit.armourData = new LeatherArmourData();
        unit.accessory1 = new BlankItemData();
        unit.accessory2 = new BlankItemData();
    }

    public override void Tier1()
    {
        unitInfo.baseBreath += 1;
        unitInfo.firstBreath += 1;
        if (unitInfo.fate != Fate.Drudge)
        {
            unitInfo.flaggingBreath += 1;
            unitInfo.baseBreath += 1;
        }  
        unitInfo.baseStrength += 1;
        unitInfo.baseToughness += 1;
    }

    public override void Tier2()
    {
        throw new System.NotImplementedException();
    }

    public override void GetAspect(Unit unit)
    {
        //not sure about this. for monsters it means they get the defender script added a second time. But they DO want the owner part. 
        Brute bruteAspect = unit.gameObject.AddComponent<Brute>();
        bruteAspect.owner = unit;
    }

    public override Mesh GetVisual() { return GameAssets.i.MediumArmourMesh; }
}

public class Brute : Aspect
{
    private void Start()
    {
        AttackManager.OnGraze += Batter;
        Unit.onKO += UnSubscribe;
    }

    public void Batter(Unit attacker, Unit defender)
    {
        if (attacker == owner)
        {
            AttackManager.grazeDamage -= 1;
        }
    }

    public void UnSubscribe(Unit unit)
    {
        if (unit == owner)
        {
            AttackManager.OnGraze -= Batter;
            Unit.onKO -= UnSubscribe;
        }
    }

    public override void GetAspectData()
    {
        aspectData = new DefenderData();
    }
}



