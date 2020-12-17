﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderData : AspectData
{
    public override void SetAspectData(UnitInfo unit)
    {
        unitInfo = unit;
        unit.mainWeaponData = new ShortswordData();
        unit.offHandData = new ShieldData();
        unit.armourData = new ChainShirtArmourData();
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
        Debug.Log("levelled up");

        //set tier
        tier = 2;

        //health up
        unitInfo.baseBreath += 1;
        unitInfo.firstBreath += 1;
        if (unitInfo.fate != Fate.Drudge)
        {
            unitInfo.flaggingBreath += 1;
            unitInfo.baseBreath += 1;
        }
    }

    public override void GetAspect(Unit unit)
    {
        //not sure about this. for monsters it means they get the defender script added a second time. But they DO want the owner part. 
        Defender defenderAspect = unit.gameObject.AddComponent<Defender>();
        defenderAspect.owner = unit;

        if (tier >= 2) defenderAspect.SetRoarAction();
    }

    public override Mesh GetVisual() { return GameAssets.i.MediumArmourMesh; }
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
            AttackManager.grazeDamage += 1;
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

    public override void GetAspectData()
    {
        aspectData = new DefenderData();
    }

    public void SetRoarAction()
    {
        Debug.Log("Set Roar Action called");
        Roar r = new Roar();
        r.SetActionButtonData(owner);
        owner.actions.Add(r);
    }

    private void OnDestroy()
    {
        UnSubscribe(gameObject.GetComponent<Unit>());
    }
}


