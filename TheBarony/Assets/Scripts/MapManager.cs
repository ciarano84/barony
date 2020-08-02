using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEditor.PackageManager;

public class MapManager : MonoBehaviour
{
    public List<EncounterSite> sites = new List<EncounterSite>();
    public static List<Encounter> encounters = new List<Encounter>();
    public List<Company> companies = new List<Company>();
    public static EncounterSite theCastle;
    public RostaInfo rosta;
    public Text date;
    public enum UIState { standard, confirmation };
    public static UIState uiState = UIState.standard;

    //simple day tracker till a calander is in place. 
    public int day = 1;

    private void Start()
    {
        theCastle = GameObject.Find("The Castle").GetComponent<EncounterSite>();
        rosta = GameObject.Find("PlayerData" + "(Clone)").GetComponent<RostaInfo>();
        if (rosta == null) Debug.LogError("Encounter could not find the player data");

        foreach (Encounter e in encounters) e.runCompanySelectSetUp = false;
        foreach (CompanyInfo c in rosta.companies)
        {
            c.CreateCompany();
            companies.Add(c.company);
        }

                //add to list of companies
    }

    public void NewDay()
    {
        if (uiState == UIState.standard)
        day++;
        date.text = ("Day " + day);
        GenerateEncounters();
        foreach (EncounterSite site in sites)
        {
            if (site.encounter != null)
            {
                site.encounter.DaysRemaining--;
                if (site.encounter.DaysRemaining < 0)
                {
                    encounters.Remove(site.encounter);
                    site.ClearEncounter();
                }
                else
                {
                    site.ShowEncounter();
                }
            }
        }
        MoveCompanys();
    }

    void GenerateEncounters()
    {
        int roll = Random.Range(1, 11);
        switch (encounters.Count)
        {
            case 0:
                roll += 6;
                break;
            case 1:
                roll += 2;
                break;
            default:
                roll = 0;
                break;
        }
        //Debug.Log("modified roll is " + roll);
        if (roll >= 10)
        {
            CreateEncounter();
        }
    }
    void CreateEncounter()
    {
        Encounter encounter = new Reclaim();
        encounter.DaysRemaining = Random.Range(6, 15);
        encounter.GetReferences();
        FindLocation(encounter);
        encounters.Add(encounter);
    }

    void FindLocation(Encounter encounter)
    {
        List<EncounterSite> availableSites = encounter.FindSuitableSites();

        if (availableSites.Count == 0)
        { Debug.LogWarning("no available site found"); }
        else
        {
            EncounterSite selectedSite = availableSites[Random.Range(0, availableSites.Count)];
            selectedSite.encounter = encounter;
            encounter.site = selectedSite;
            encounter.SetEncounterData();
        }
    }

    void MoveCompanys()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("company"))
        {
            companies.Add(go.GetComponent<Company>());
        }
        foreach (Company company in companies)
        {
            company.Travel();
        }
    }
}

//public enum EncounterType { RECLAIM };

public abstract class Encounter
{
    public MapManager mapManager;
    public ConfirmationPopUp confirmationPopUp;
    public RostaInfo rosta;
    public int DaysRemaining;
    public EncounterSite site;

    //This is used to record the starting point of a company. 
    public Transform origin;

    //Data about the encounter
    public string ConfirmationQuestionText;
    public string ConfirmationYesText;
    public string ConfirmationNoText;
    public string CompanySelectProceedText;

    public bool runCompanySelectSetUp;

    public void GetReferences()
    {
        mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        if (mapManager == null) Debug.LogError("Encounter couldn't get reference to map manager");
        confirmationPopUp = GameObject.Find("ConfirmationPopUp").GetComponent<ConfirmationPopUp>();
        if (confirmationPopUp == null) Debug.LogError("Encounter could not find the confirmation popup");
        rosta = GameObject.Find("PlayerData" + "(Clone)").GetComponent<RostaInfo>();
        if (rosta == null) Debug.LogError("Encounter could not find the player data");
    }

    public virtual void SetEncounterData()
    { }

    public virtual void Selected()
    { }

    public virtual void GoToCompanySelect()
    {
        SceneManager.LoadScene("SquadView");
    }

    public virtual void StartEncounter()
    { }

    public virtual List<EncounterSite> FindSuitableSites()
    {
        return new List<EncounterSite>();
    }

    public virtual void ProceedFromCompanySelect()
    { }

    public virtual void AssignCompany()
    {
        if (runCompanySelectSetUp) return;

        int unitLimit = 4;
        for (int count = 0; count < unitLimit; count++)
        {
            if (rosta.castle[count] == null) Debug.LogError("No Units left in the castle");

            //Add unit to squad.
            rosta.squad.Add(rosta.castle[count]);

            //Remove it from rosta.
            rosta.castle.Remove(rosta.castle[count]);
        }
        runCompanySelectSetUp = true;
    }

    public void CreateCompanyInfo()
    {
        CompanyInfo companyInfo = new CompanyInfo();
        companyInfo.units = rosta.squad;
        companyInfo.targetEncounter = this;
        companyInfo.origin = origin;
        companyInfo.destination = site.transform;
        rosta.squad.Clear();
        rosta.companies.Add(companyInfo);
    }
}
