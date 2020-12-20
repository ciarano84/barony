using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancementManager
{
    static int level2Threshold = 10;
    static int level3Threshold = 30;
    static List<LevelUp> levelUps = new List<LevelUp>();

    public static void CheckForLevelUp(UnitInfo unit)
    {
        switch (unit.level)
        {
            case 1:
                if (unit.experience >= level2Threshold)
                {
                    unit.aspectData.Tier2();
                    LevelUp levelUp = new LevelUp();
                    levelUp.unitInfo = unit;
                    levelUp.levelGointUpTo = 2;
                }
                    
                break;
            case 2:

                break;
            
            default:
                break;
        }
        if (levelUps.Count > 0) ReportAdvancement();
    }

    public static void ReportAdvancement()
    {
        MapManager mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        mapManager.ReportLevelUps(levelUps);
        levelUps.Clear();
    }
}

public class LevelUp
{
    public UnitInfo unitInfo;
    public int levelGointUpTo;
}
