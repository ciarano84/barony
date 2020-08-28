﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AspectPack { SCOUT, DEFENDER, PRIEST, HUNTER, BRUTE };

public class MonsterConstructor : MonoBehaviour
{
    public AspectPack aspectPack;
    public Fate fate = Fate.Drudge; 

    public void SetUpMonster()
    {
        UnitInfo unitInfo = GetComponent<Unit>().unitInfo;
        Unit unit = gameObject.GetComponent<Unit>();

        if (fate == Fate.Elite)
        {
            unitInfo.fate = Fate.Elite;
            unitInfo.flaggingBreath = unitInfo.firstBreath;
            unitInfo.baseBreath = unitInfo.firstBreath + unitInfo.flaggingBreath;
            unitInfo.currentBreath = unitInfo.baseBreath;
        }
        else unitInfo.fate = Fate.Drudge;
            

        switch (aspectPack)
        {
            case AspectPack.SCOUT:
                unitInfo.aspectData = new ScoutData();
                break;
            case AspectPack.DEFENDER:
                unitInfo.aspectData = new DefenderData();
                break;
            case AspectPack.PRIEST:
                unitInfo.aspectData = new PriestData();
                break;
            case AspectPack.HUNTER:
                unitInfo.aspectData = new HunterData();
                break;
            case AspectPack.BRUTE:
                unitInfo.aspectData = new BruteData();
                break;
            default:
                break;
        }

        
        unitInfo.aspectData.SetAspectData(unitInfo);

        if (unitInfo.mainWeaponData != null) unitInfo.mainWeaponData.SetData(unitInfo);
        if (unitInfo.offHandData != null) unitInfo.offHandData.SetData(unitInfo);
        if (unitInfo.armourData != null) unitInfo.armourData.SetData(unitInfo);
        if (unitInfo.accessory1 != null) unitInfo.accessory1.SetData(unitInfo);
        if (unitInfo.accessory2 != null) unitInfo.accessory2.SetData(unitInfo);

        //Do I need this any more? 
        unit.SetSlots();
        unitInfo.mainWeaponData.EquipItem(unit);
        if (gameObject.name == "Orc Defender(Clone)")
        {
            //Debug
        }
        if (unitInfo.offHandData != null) unitInfo.offHandData.EquipItem(unit);
        unitInfo.armourData.EquipItem(unit);
        unitInfo.aspectData.GetAspect(unit);

        unitInfo.aspectData.Level1();
    }
}
