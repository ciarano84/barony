using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reclaim : Encounter
{
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
        confirmationPopUp.GetConfirmation(RallyConfirmationQuestionText, RallyConfirmationYesText, RallyConfirmationNoText);
        MapManager.uiState = MapManager.UIState.confirmation;
        ConfirmationPopUp.onConfirm += GoToCompanySelect;
        ConfirmationPopUp.onCancel += CancelRally;
    }

    public override void GoToCompanySelect()
    {
        MapManager.uiState = MapManager.UIState.standard;
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
        MapManager.uiState = MapManager.UIState.standard;
        ConfirmationPopUp.onConfirm -= GoToCompanySelect;
        ConfirmationPopUp.onCancel -= CancelRally;
    }

    public override void ProceedFromCompanySelect()
    {
        foreach (UnitInfo u in RostaInfo.squad)
        {
            selectedCompany.units.Add(u);
        }
        RostaInfo.squad.Clear();
        SceneManager.LoadScene("Map");
    }

    public override void StartEncounter()
    {
        base.StartEncounter();
    }

    public override void EncounterButtonSelected()
    {
        confirmationPopUp.GetConfirmation(EncounterStartConfirmationQuestionText, EncounterStartConfirmationYesText, EncounterStartConfirmationNoText);
        MapManager.uiState = MapManager.UIState.confirmation;
        ConfirmationPopUp.onConfirm += StartEncounter;
        ConfirmationPopUp.onCancel += CancelEncounter;
    }

    void CancelEncounter()
    {
        ConfirmationPopUp.onConfirm -= StartEncounter;
        ConfirmationPopUp.onCancel -= CancelEncounter;
        selectedCompany.targetEncounter = null;
    }
}