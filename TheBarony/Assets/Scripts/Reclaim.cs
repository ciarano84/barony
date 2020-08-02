using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reclaim : Encounter
{
    public override void SetEncounterData()
    {
        ConfirmationQuestionText = "Rally a company to reclaim " + site.SiteName + "?";
        ConfirmationYesText = "Yes";
        ConfirmationNoText = "No";
        CompanySelectProceedText = "Set Forth";
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
        confirmationPopUp.GetConfirmation(ConfirmationQuestionText, ConfirmationYesText, ConfirmationNoText);
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
        CreateCompanyInfo();
        SceneManager.LoadScene("Map");
    }
}
