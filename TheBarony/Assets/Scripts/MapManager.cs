using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapManager : MonoBehaviour
{
    public List<EncounterSite> sites = new List<EncounterSite>();
    public static List<Encounter> encounters = new List<Encounter>();
    public List<Company> companies = new List<Company>();
    public static EncounterSite theCastle;
    public Text date;

    //simple day tracker till a calander is in place. 
    public int day = 1;

    private void Awake()
    {
        theCastle = GameObject.Find("The Castle").GetComponent<EncounterSite>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            NewDay();
        }
    }

    void NewDay()
    {
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
        Encounter encounter = new Encounter();
        encounter.DaysRemaining = Random.Range(6, 15);
        encounter.type = EncounterType.RECLAIM;
        FindLocation(encounter);
        encounters.Add(encounter);
    }

    void FindLocation(Encounter encounter)
    {
        List<EncounterSite> availableSites = new List<EncounterSite>();
        foreach (EncounterSite site in sites)
        {
            bool potentialSiteFound = false;
            switch (encounter.type)
            {
                case EncounterType.RECLAIM:
                    if (!site.reclaim)
                        potentialSiteFound = true;
                        break;
                default:
                    break;
            }

            if (site.encounter == null) potentialSiteFound = true;

            if (potentialSiteFound) availableSites.Add(site);
        }

        if (availableSites.Count == 0)
        { Debug.LogWarning("no available site found"); }
        else
        {
            EncounterSite selectedSite = availableSites[Random.Range(0, availableSites.Count)];
            selectedSite.encounter = encounter;
            encounter.site = selectedSite;
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

public enum EncounterType { RECLAIM };

public class Encounter
{
    public int DaysRemaining;
    public EncounterSite site;
    public EncounterType type;
}
