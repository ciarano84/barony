using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancementManager : MonoBehaviour
{
    int level2Threshold = 5;
    int level3Threshold = 30;
    static List<LevelUp> levelUps = new List<LevelUp>();
    public static AdvancementManager instance;
    public MapManager mapManager;

    private void Awake()
    {
        instance = this;
    }

    public void ProcessEncounterRewards()
    {
        if (RostaInfo.currentEncounter.encounterType == Encounter.EncounterType.RECLAIM) RostaInfo.ReclaimedSites += 1;
        foreach (UnitInfo u in RostaInfo.currentEncounter.selectedCompany.units)
        {
            //add a random percentage to the base offering of the encounter.
            int TotalXPavailable = Mathf.RoundToInt(RostaInfo.currentEncounter.XPreward * (1 + (Random.Range(0, 5) / 10)));
            //work out the amount of xp to transfer (choosing the minimum between the clarity and the xp on offer). 
            int clarityTransfered = Mathf.Min(u.clarity, TotalXPavailable);
            u.experience += clarityTransfered;
            u.clarity -= clarityTransfered;
            CheckForLevelUp(u);
        }
        if (levelUps.Count > 0)
        {
            ReportLevelUps(levelUps);
            levelUps.Clear();
        }
    }

    public void CheckForLevelUp(UnitInfo unit)
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
                    levelUps.Add(levelUp);
                }  
                break;
            default:
                break;
        }
    }

    public void ReportLevelUps(List<LevelUp> levelUps)
    {
        Debug.Log("report level ups");
        string levelUpMessage = "";
        foreach (LevelUp u in levelUps)
        {
            levelUpMessage += u.unitInfo.unitName + " to level " + u.levelGointUpTo + ", ";
        }
        char[] MyChar = { ',', ' ' };
        string newlevelUpMessage = levelUpMessage.TrimEnd(MyChar);
        newlevelUpMessage = newlevelUpMessage + ".";

        UIManager.RequestAlert(newlevelUpMessage, "Return");
    }
}

public class LevelUp
{
    public UnitInfo unitInfo;
    public int levelGointUpTo;
}
