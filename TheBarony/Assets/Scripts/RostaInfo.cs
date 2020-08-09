using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class RostaInfo : MonoBehaviour
{
    public List<UnitInfo> castle = new List<UnitInfo>();
    public static List<CompanyInfo> companies = new List<CompanyInfo>();
    public static List<Encounter> encounters = new List<Encounter>();

    //used to detirmine a character's position in the squadview.
    public int currentUnitShown = 0;
    public int companyPosition;
    
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

    private void Update()
    {
        foreach (CompanyInfo c in companies)
        {
            Debug.Log(c.units.Count);
        }
    }
}
