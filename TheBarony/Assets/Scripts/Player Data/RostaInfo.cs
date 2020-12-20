using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class RostaInfo : MonoBehaviour
{
    public static List<UnitInfo> castle = new List<UnitInfo>();
    public static List<CompanyInfo> companies = new List<CompanyInfo>();
    
    //used to detirmine a character's position in the squadview.
    public int currentUnitShown = 0;
    public int companyPosition;

    public static List<Encounter> encounters = new List<Encounter>();

    //Tracks campaign progress
    public static int ReclaimedSites = 0;

    //The encounter currently being handled. 
    public static Encounter currentEncounter;

    //whether an encounter is active or not
    public static bool encounter;

    //Calander Info
    public static int date = 1;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public static void CreateCompanyInfo(Encounter encounter = null, GameObject origin = null, GameObject destination = null)
    {
        CompanyInfo companyInfo = new CompanyInfo();
        companyInfo.targetEncounter = encounter;
        companyInfo.originSave = TransformSave.StoreTransform(origin);
        companyInfo.destinationSave = TransformSave.StoreTransform(destination);
        encounter.selectedCompany = companyInfo;
        companies.Add(companyInfo);
    }

    public static void NewDay()
    {
        date++;
        foreach (UnitInfo u in castle)
        {
            u.clarity++;
        }
    }

    public static void ProcessEncounterRewards()
    {
        if (currentEncounter.encounterType == Encounter.EncounterType.RECLAIM) ReclaimedSites += 1;
        foreach (UnitInfo u in currentEncounter.selectedCompany.units)
        {
            //add a random percentage to the base offering of the encounter.
            int TotalXPavailable = Mathf.RoundToInt(currentEncounter.XPreward * (1 + (Random.Range(0, 5) / 10)));
            //work out the amount of xp to transfer (choosing the minimum between the clarity and the xp on offer). 
            int clarityTransfered = Mathf.Min(u.clarity, TotalXPavailable);
            u.experience += clarityTransfered;
            u.clarity -= clarityTransfered;
            AdvancementManager.CheckForLevelUp(u);
        }
    }
}
