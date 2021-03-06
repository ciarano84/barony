﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterData : AspectData
{
    public override void SetAspectData(UnitInfo unit)
    {
        className = "Hunter";

        //Do we need this next line, isn't it just setting A to be A?
        unitInfo = unit;

        unit.mainWeaponData = new DaggerData();
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
    public override Mesh GetVisual() { return GameAssets.i.LightArmourMesh; }
}

public class Hunter : Aspect
{
    private void Start()
    {
        AttackManager.OnAttack += Ghosting;
        Unit.onKO += UnSubscribe;
    }

    public void Ghosting(Unit attacker, Unit defender)
    {
        if (attacker == owner && defender == owner.focus)
        {
            bool focusedOn = false;
            foreach (Unit u in Initiative.order)
            {
                if (u.focus == owner) focusedOn = true;
            }
            if (!focusedOn) AttackManager.damage += 2;
        }
    }

    public void UnSubscribe(Unit unit)
    {
        if (unit == owner)
        {
            AttackManager.OnGraze -= Ghosting;
            Unit.onKO -= UnSubscribe;
        }
    }

    public override void GetAspectData()
    {
        aspectData = new HunterData();
    }
}
