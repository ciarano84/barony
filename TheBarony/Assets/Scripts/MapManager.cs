using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEditor.PackageManager;
using System;
using JetBrains.Annotations;

public class MapManager : MonoBehaviour
{
    public List<EncounterSite> sites = new List<EncounterSite>();
    public List<Company> companies = new List<Company>();
    public static EncounterSite theCastle;
    public RostaInfo rosta;
    public Text date;
    public GameObject encounterPanel;
    public CampaignManager campaign;

    private void Start()
    {
        theCastle = GameObject.Find("The Castle").GetComponent<EncounterSite>();
        rosta = GameObject.Find("PlayerData" + "(Clone)").GetComponent<RostaInfo>();
        if (rosta == null) Debug.LogError("Encounter could not find the player data");

        if (RostaInfo.currentEncounter != null)
        {
            if (RostaInfo.currentEncounter.completionState == Encounter.CompletionState.VICTORY)
            {
                if (RostaInfo.currentEncounter.encounterType == Encounter.EncounterType.RECLAIM) RostaInfo.ReclaimedSites += 1;
                if (campaign.CheckForCampaignCompletion() == true) return;

                MapUIManager.RequestAlert(RostaInfo.currentEncounter.victoryMapText, "Return");
                RostaInfo.currentEncounter.selectedCompany.targetEncounter = null;
                RostaInfo.currentEncounter.site.encounter = null;
                RostaInfo.encounters.Remove(RostaInfo.currentEncounter);
            }
            else if (RostaInfo.currentEncounter.completionState == Encounter.CompletionState.DEFEAT)
            {
                MapUIManager.RequestAlert(RostaInfo.currentEncounter.defeatMapText, "Return");
                RostaInfo.currentEncounter.selectedCompany.targetEncounter = null;
                RostaInfo.currentEncounter.selectedCompany = null;
                RostaInfo.currentEncounter.completionState = Encounter.CompletionState.UNASSIGNED;
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
        //Will need to change this formula if I want to get 3 by 3 arenas, but for now we're just making arenasizes 4 (2 b 2) and 6 (2 b 3). 
        encounter.arenaSize = (((int)UnityEngine.Random.Range(1, 3)) * 2) + 2;
        encounter.difficulty = UnityEngine.Random.Range(0, 2);
        encounter.enemyCompany = EncounterTable.CreateEnemyCompany(encounter);
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
    public int arenaSize;
    public int difficulty;
    public EnemyType enemyType;
    public EnemyCompany enemyCompany;
    public bool permanent;
    public EncounterSite site;
    public CompanyInfo selectedCompany;

    //This tracks the victory/defeat state of the encounter.
    public enum CompletionState { INPROGRESS, VICTORY, DEFEAT, ESCAPED, UNASSIGNED };
    public CompletionState completionState = CompletionState.INPROGRESS;

    //This tells you the type of encounter
    public enum EncounterType { RECLAIM, PURGE, DEFENCE, AMBUSH };
    public EncounterType encounterType = EncounterType.RECLAIM;

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
    public string companyAlreadyAssignedText;

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
        RostaInfo.currentEncounter = this;
        RostaInfo.encounter = true;
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
            //Add unit to company.
            selectedCompany.units.Add(rosta.castle[count]);

            //Remove it from rosta.
            rosta.castle.Remove(rosta.castle[count]);
        }
        runCompanySelectSetUp = true;
    }

    public virtual void EncounterButtonSelected()
    {
    }
}
