using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reclaim : Encounter
{
    UnitInfo headman;
    
    public override void SetEncounterData()
    {
        RallyConfirmationQuestionText = "Rally a company to reclaim " + site.SiteName + "?";
        RallyConfirmationYesText = "Yes";
        RallyConfirmationNoText = "No";
        CompanySelectProceedText = "Set Forth";
        EncounterStartConfirmationQuestionText = "Lead the assult on " + site.SiteName + "?";
        EncounterStartConfirmationYesText = "Proceed";
        EncounterStartConfirmationNoText = "Return";
        encounterButtonText = "Reclaim " + site.SiteName;
        victoryMapText = site.SiteName + " is now an inhabitable district.";
        defeatMapText = "Your company was defeated at " + site.SiteName;
        companyAlreadyAssignedText = "A company has already been dispatched to reclaim " + site.SiteName + ".";
    }

    public override List<EncounterSite> FindSuitableSites()
    {
        List<EncounterSite> availableSites = new List<EncounterSite>();
        foreach (EncounterSite site in mapManager.sites)
        {
            bool potentialSiteFound = false;
            if (!site.reclaim)
                potentialSiteFound = true;

            if (site.encounter == null) potentialSiteFound = true;

            if (potentialSiteFound) availableSites.Add(site);
        }
        return availableSites;
    }

    public override void Selected()
    {
        if (selectedCompany == null)
        {
            if (RostaInfo.castle.Count == 0)
            {
                MapUIManager.RequestAlert("You have no available troops.", "Return");
                return;
            }
            MapUIManager.RequestConfirmation(RallyConfirmationQuestionText, RallyConfirmationYesText, RallyConfirmationNoText);
            ConfirmationPopUp.onConfirm += GoToCompanySelect;
            ConfirmationPopUp.onCancel += CancelRally;
        }
        else
        {
            MapUIManager.RequestAlert(companyAlreadyAssignedText, EncounterStartConfirmationNoText);
        }
    }

    public override void GoToCompanySelect()
    {
        ConfirmationPopUp.onConfirm -= GoToCompanySelect;
        ConfirmationPopUp.onCancel -= CancelRally;

        origin = GameObject.Find("The Castle").transform;
        if (origin == null) Debug.LogError("encounter can't find the castle");

        RostaInfo.CreateCompanyInfo(this, origin.gameObject, site.gameObject);

        RostaInfo.currentEncounter = this;
        base.GoToCompanySelect();
    }

    void CancelRally()
    {
        ConfirmationPopUp.onConfirm -= GoToCompanySelect;
        ConfirmationPopUp.onCancel -= CancelRally;
    }

    public override void ProceedFromCompanySelect()
    {
        SceneManager.LoadScene("Map");
    }

    public override void StartEncounter()
    {
        ConfirmationPopUp.onConfirm -= StartEncounter;
        ConfirmationPopUp.onCancel -= CancelEncounter;
        base.StartEncounter();
    }

    public override void EncounterButtonSelected()
    {
        MapUIManager.RequestConfirmation(EncounterStartConfirmationQuestionText, EncounterStartConfirmationYesText, EncounterStartConfirmationNoText);
        ConfirmationPopUp.onConfirm += StartEncounter;
        ConfirmationPopUp.onCancel += CancelEncounter;
    }

    void CancelEncounter()
    {
        ConfirmationPopUp.onConfirm -= StartEncounter;
        ConfirmationPopUp.onCancel -= CancelEncounter;
        selectedCompany.targetEncounter = null;
        selectedCompany = null;
        mapManager.CheckForAvailableEncounters();
    }

    public override void CreateEnemyCompany()
    {
        base.CreateEnemyCompany();
        int position = Random.Range(0, enemyCompany.cells.Count);
        GameObject headmanObj = GameAssets.i.Headman;
        enemyCompany.cells[position].enemies.Add(headmanObj);
        headman = headmanObj.GetComponent<Unit>().unitInfo;
    }

    public override void OnUnitDeath(Unit unit)
    {
        base.OnUnitDeath(unit);
    }
}