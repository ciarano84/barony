using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderData : AspectData
{
    public override void SetAspectData(UnitInfo unit)
    {
        className = "Defender";
        unitInfo = unit;
        unit.className = className;
        unit.mainWeaponData = new ShortswordData();
        unit.offHandData = new ShieldData();
        unit.armourData = new LeatherArmourData();
        unit.accessory1 = new BlankItemData();
        unit.accessory2 = new BlankItemData();
    }

    public override void Level1()
    {
        unitInfo.baseBreath += 1;
        unitInfo.baseStrength += 1;
        unitInfo.baseToughness += 1;
    }

    public override void GetAspect(Unit unit)
    {
        Defender defenderAspect = unit.gameObject.AddComponent<Defender>();
        defenderAspect.owner = unit;
    }

    public override GameObject GetVisual() { return GameAssets.i.DefenderVisual; }
}

public class Defender : Aspect
{
    private void Start()
    {
        AttackManager.OnGraze += SoakDamage;
        Unit.onKO += UnSubscribe;
    }

    public void SoakDamage(Unit attacker, Unit defender)
    {
        if (defender == owner)
        {
            AttackManager.grazeDamage = -1;
        }
    }

    public void UnSubscribe(Unit unit)
    {
        if (unit == owner)
        {
            AttackManager.OnGraze -= SoakDamage;
            Unit.onKO -= UnSubscribe;
        }
    }
}


