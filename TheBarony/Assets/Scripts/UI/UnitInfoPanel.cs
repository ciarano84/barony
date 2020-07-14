using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class UnitInfoPanel : MonoBehaviour
{
    public TextMeshProUGUI unitName;
    public Image unitWeaponImage;
    public UnitInfo unit;
    public GameObject point;
    public int position;
    public RostaInfo rosta;

    public void Start()
    {
        StartCoroutine(WaitAndFindPlayerData());
    }

    IEnumerator WaitAndFindPlayerData()
    {
        //gammy way of doing this. Should set up a delegate on player data that announces when everything is in place. 
        yield return new WaitForSeconds(0.2f);
        rosta = GameObject.Find("PlayerData" + "(Clone)").GetComponent<RostaInfo>();
        yield break;
    }   

    public void SetUnit(UnitInfo unitInfo)
    {
        unit = unitInfo;
        unitName.text = unit.unitName;

        //Destroy(unitPrefab);
        GameObject unitPrefab = Instantiate(GameAssets.i.PlayerUnit, point.transform, false);
        unitPrefab.GetComponent<Unit>().unitInfo = unitInfo;

        unit.mainWeaponData.SetData(unit);
        unit.mainWeaponData.EquipItem(unitPrefab.GetComponent<Unit>());
        unitWeaponImage.sprite = unit.mainWeaponData.SetImage();

        //point.GetComponent<SkinnedMeshRenderer>().sharedMesh = unit.aspectData.GetVisual();
    }

    public void SelectUnit()
    {
        rosta.companyPosition = position;
        SceneManager.LoadScene("TroopRosta");
    }
}
