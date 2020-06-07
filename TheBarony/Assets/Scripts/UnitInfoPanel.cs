using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UnitInfoPanel : MonoBehaviour
{
    public Text unitName;
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
        yield return new WaitForSeconds(0.2f);
        rosta = GameObject.Find("PlayerData" + "(Clone)").GetComponent<RostaInfo>();
        yield break;
    }   

    public void SetUnit(UnitInfo unitInfo)
    {
        unit = unitInfo;
        unitName.text = unit.unitName;
        //This is where we will have to get it to choose WHAT weapon data it puts in. 
        unit.weaponData = new MeleeWeaponData();

        Sprite sprite;
        sprite = Resources.Load<Sprite>(unit.weaponData.imageFile);
        unitWeaponImage.sprite = sprite;

        GameObject instance = Instantiate(Resources.Load(unit.unitVisual)) as GameObject;
        instance.transform.position = point.transform.position;
        //this is nonsense just to get stuff showing right for the time being:
        instance.transform.Rotate(0,90,0);
    }

    public void SelectUnit()
    {
        rosta.companyPosition = position;
        SceneManager.LoadScene("TroopRosta");
    }
}
