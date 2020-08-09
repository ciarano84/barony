using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompanySelectManager : MonoBehaviour
{
    public RostaInfo rosta;
    public UnitInfoPanel infoPanel1;
    public UnitInfoPanel infoPanel2;
    public UnitInfoPanel infoPanel3;
    public UnitInfoPanel infoPanel4;
    public Text proceedText;
    List<UnitInfoPanel> unitInfoPanels = new List<UnitInfoPanel>();

    void Start()
    {
        proceedText.text = RostaInfo.currentEncounter.CompanySelectProceedText;
        StartCoroutine(SetInfoPanels());
        RostaInfo.currentEncounter.AssignCompany();
    }

    IEnumerator SetInfoPanels()
    {
        yield return new WaitForSeconds(0.1f);
        unitInfoPanels.Add(infoPanel1);
        unitInfoPanels[0].position = 0;
        unitInfoPanels.Add(infoPanel2);
        unitInfoPanels[1].position = 1;
        unitInfoPanels.Add(infoPanel3);
        unitInfoPanels[2].position = 2;
        unitInfoPanels.Add(infoPanel4);
        unitInfoPanels[3].position = 3;

        rosta = GameObject.Find("PlayerData"+"(Clone)").GetComponent<RostaInfo>();
        CompanyInfo company = RostaInfo.currentEncounter.selectedCompany;
        int max = Math.Min(3, company.units.Count);
        for (int count = max; count >= 0; count--)
        {
            unitInfoPanels[count].SetUnit(company.units[count]);
        }
        yield break;
    }

    public void BeginEncounter()
    {
        RostaInfo.currentEncounter.ProceedFromCompanySelect();
    }
}
