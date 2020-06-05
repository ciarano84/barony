using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompanySelectManager : MonoBehaviour
{
    public RostaInfo rosta;
    public UnitInfoPanel infoPanel1;
    public UnitInfoPanel infoPanel2;
    public UnitInfoPanel infoPanel3;
    public UnitInfoPanel infoPanel4;
    List<UnitInfoPanel> unitInfoPanels = new List<UnitInfoPanel>();

    void Start()
    {
        StartCoroutine(SetInfoPanels());
    }

    IEnumerator SetInfoPanels()
    {
        yield return new WaitForSeconds(0.1f);
        unitInfoPanels.Add(infoPanel1);
        unitInfoPanels.Add(infoPanel2);
        unitInfoPanels.Add(infoPanel3);
        unitInfoPanels.Add(infoPanel4);

        rosta = GameObject.Find("PlayerData").GetComponent<RostaInfo>();
        for (int count = 0; count < 4; count++)
        {
            unitInfoPanels[count].SetUnit(rosta.rosta[count]);
        }
        yield break;
    }

    public void BeginEncounter()
    {
        //set troops into the squad
        for (int count = 0; count < 4; count++)
        {
            rosta.squad.Add(unitInfoPanels[count].unit);
        }

        SceneManager.LoadScene("Arena0");
    }
}
