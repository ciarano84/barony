using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompanyManager : MonoBehaviour
{
    RostaInfo rosta;

    public UnitInfoPanel unitInfoPanel1;
    public UnitInfoPanel unitInfoPanel2;
    public UnitInfoPanel unitInfoPanel3;
    public UnitInfoPanel unitInfoPanel4;

    List<UnitInfoPanel> unitInfoPanels = new List<UnitInfoPanel>();

    void Start()
    {
        //Need this till the data persists. 
        rosta = GameObject.Find("PlayerData").GetComponent<RostaInfo>();
        unitInfoPanels.Add(unitInfoPanel1);
        unitInfoPanels.Add(unitInfoPanel2);
        unitInfoPanels.Add(unitInfoPanel3);
        unitInfoPanels.Add(unitInfoPanel4);
        StartCoroutine(PopulateUnitPanels());
    }

    IEnumerator PopulateUnitPanels()
    {
        yield return new WaitForSeconds(0.1f);
        for (int count = 0; count < 4; count++)
        {
            UnitInfo unit = rosta.rosta[count];
            if (unit == null) yield break;
            PopulateUnitPanel(count, unit);
        }
        yield break;
    }

    void PopulateUnitPanel(int count, UnitInfo unit)
    {
        UnitInfoPanel panel = unitInfoPanels[count];
        panel.unitName.text = unit.unitName;
        panel.weaponImage.sprite = Resources.Load<Sprite>(unit.weapon1.imageFile);
    }
}
