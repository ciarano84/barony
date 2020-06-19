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
        unitInfoPanels[0].position = 0;
        unitInfoPanels.Add(infoPanel2);
        unitInfoPanels[1].position = 1;
        unitInfoPanels.Add(infoPanel3);
        unitInfoPanels[2].position = 2;
        unitInfoPanels.Add(infoPanel4);
        unitInfoPanels[3].position = 3;

        rosta = GameObject.Find("PlayerData"+"(Clone)").GetComponent<RostaInfo>();
        for (int count = 0; count < 4; count++)
        {
            unitInfoPanels[count].SetUnit(rosta.squad[count]);
        }
        yield break;
    }

    public void BeginEncounter()
    {
        SceneManager.LoadScene("Arena0");
    }
}
