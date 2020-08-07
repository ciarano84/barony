using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEditor.PackageManager;
using System;

public class MapManager : MonoBehaviour
{
    public List<EncounterSite> sites = new List<EncounterSite>();
    public List<Company> companies = new List<Company>();
    public static EncounterSite theCastle;
    public RostaInfo rosta;
    public Text date;
    public GameObject encounterPanel;

    private void Start()
    {
        theCastle = GameObject.Find("The Castle").GetComponent<EncounterSite>();
        rosta = GameObject.Find("PlayerData" + "(Clone)").GetComponent<RostaInfo>();
        if (rosta == null) Debug.LogError("Encounter could not find the player data");

        if (RostaInfo.currentEncounter != null)
        {
            if (RostaInfo.currentEncounter.completionState == Encounter.CompletionState.VICTORY)
            {
                MapUIManager.RequestAlert(RostaInfo.currentEncounter.victoryMapText, "Return");
                RostaInfo.currentEncounter.selectedCompany.targetEncounter = null;
            }
            else if (RostaInfo.currentEncounter.completionState == Encounter.CompletionState.DEFEAT)
            {
                MapUIManager.RequestAlert(RostaInfo.currentEncounter.defeatMapText, "Return");
                RostaInfo.currentEncounter.selectedCompany.targetEncounter = null;
            }
        }

        foreach (Encounter e in RostaInfo.encounters)
        {
            e.GetReferences();
            e.runCompanySelectSetUp = false;
            e.site = GameObject.Find(e.site.SiteName).GetComponent<EncounterSite>();
            e.site.encounter = e;
            e.site.ShowEncounter();
        }

        for (int i = RostaInfo.companies.Count - 1; i >= 0; i--)
        {
            if (RostaInfo.companies[i].units.Count == 0)
            {
                RostaInfo.companies.RemoveAt(i);
            }
            else RostaInfo.companies[i].CreateCompany();
        }

        date.text = ("Day " + RostaInfo.date);
        CheckForAvailableEncounters();
    }

    public void NewDay()
    {
        if (MapUIManager.uiState == MapUIManager.UIState.standard)
        {
            RostaInfo.date++;
            date.text = ("Day " + RostaInfo.date);
            GenerateEncounters();
            foreach (EncounterSite site in sites)
            {
                if (site.encounter != null)
                {
                    if (!site.encounter.permanent)
                    {
                        site.encounter.DaysRemaining--;
                        if (site.encounter.DaysRemaining < 0)
                        {
                            RostaInfo.encounters.Remove(site.encounter);
                            site.ClearEncounter();
                        }
                        else site.ShowEncounter();
                    }
                    else site.ShowEncounter();
                }
            }
            MoveCompanys();
            CheckForAvailableEncounters();
        } 
    }

    public void ReturnCompanysToCastle()
    {
        //This list is used to rebuild the company list after it's been cleared out. 
        List<Company> tempList = new List<Company>();

        foreach (Company company in companies)
        {
            if (Vector3.Distance(company.transform.position, theCastle.transform.position) <= 0.01f && company.companyInfo.targetEncounter == null)
            {
                foreach (UnitInfo unitInfo in company.companyInfo.units)
                {
                    rosta.castle.Add(unitInfo);
                }
                Destroy(company.gameObject);
            } else tempList.Add(company);
        }
        companies.Clear();
        foreach (Company c in tempList)
        {
            companies.Add(c);
        }
    }

    void GenerateEncounters()
    {
        int roll = UnityEngine.Random.Range(1, 11);
        switch (RostaInfo.encounters.Count)
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
        encounter.permanent = true;
        encounter.GetReferences();
        FindLocation(encounter);
        RostaInfo.encounters.Add(encounter);
    }

    void FindLocation(Encounter encounter)
    {
        List<EncounterSite> availableSites = encounter.FindSuitableSites();

        if (availableSites.Count == 0)
        { Debug.LogWarning("no available site found"); }
        else
        {
            EncounterSite selectedSite = availableSites[UnityEngine.Random.Range(0, availableSites.Count)];
            selectedSite.encounter = encounter;
            encounter.site = selectedSite;
            encounter.SetEncounterData();
        }
    }

    void MoveCompanys()
    {
        foreach (Company company in companies)
        {
            company.Travel();
        }
    }

    public void CheckForAvailableEncounters()
    {
        foreach (Transform child in encounterPanel.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Encounter encounter in RostaInfo.encounters)
        {
            foreach (Company company in companies)
            {
                if (company.companyInfo.targetEncounter == encounter)
                {
                    if (Vector3.Distance(company.transform.position, encounter.site.transform.position) <= 0.01f)
                    {
                        EncounterButton encounterButton = Instantiate(GameAssets.i.encounterButton, encounterPanel.transform).GetComponent<EncounterButton>();
                        encounterButton.company = company;
                        encounterButton.encounter = encounter;
                        encounterButton.buttonText.text = encounter.encounterButtonText;
                    }
                } 
            }
        }
    }


}

public abstract class Encounter
{
    public MapManager mapManager;
    public RostaInfo rosta;
    public int DaysRemaining;
    public bool permanent;
    public EncounterSite site;
    public CompanyInfo selectedCompany;

    //This tracks the victory/defeat state of the encounter.
    public enum CompletionState { INPROGRESS, VICTORY, DEFEAT, ESCAPED };
    public CompletionState completionState = CompletionState.INPROGRESS;

    //This is used to record the starting point of a company. 
    public Transform origin;

    //Data about the encounter
    public string RallyConfirmationQuestionText;
    public string RallyConfirmationYesText;
    public string RallyConfirmationNoText;
    public string CompanySelectProceedText;
    public string EncounterStartConfirmationQuestionText;
    public string EncounterStartConfirmationYesText;
    public string EncounterStartConfirmationNoText;
    public string encounterButtonText;
    public string victoryMapText;
    public string defeatMapText;

    public bool runCompanySelectSetUp;

    public void GetReferences()
    {
        mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        if (mapManager == null) Debug.LogError("Encounter couldn't get reference to map manager");
        rosta = GameObject.Find("PlayerData" + "(Clone)").GetComponent<RostaInfo>();
        if (rosta == null) Debug.LogError("Encounter could not find the player data");
    }

    public virtual void SetEncounterData()
    { }

    public virtual void Selected()
    {
    }

    public virtual void GoToCompanySelect()
    {
        SceneManager.LoadScene("SquadView");
    }

    public virtual void StartEncounter()
    {
        RostaInfo.squad.Clear();
        RostaInfo.squad = selectedCompany.units;
        RostaInfo.currentEncounter = this;
        SceneManager.LoadScene("Arena0");
    }

    public virtual List<EncounterSite> FindSuitableSites()
    {
        return new List<EncounterSite>();
    }

    public virtual void ProceedFromCompanySelect()
    { }

    public virtual void AssignCompany()
    {
        if (runCompanySelectSetUp) return;

        int unitLimit = Math.Min(3, rosta.castle.Count - 1);
        for (int count = unitLimit; count >= 0; count--)
        {
            //Add unit to squad.
            RostaInfo.squad.Add(rosta.castle[count]);

            //Remove it from rosta.
            rosta.castle.Remove(rosta.castle[count]);
        }
        runCompanySelectSetUp = true;
    }

    public virtual void EncounterButtonSelected()
    {
    }
}
