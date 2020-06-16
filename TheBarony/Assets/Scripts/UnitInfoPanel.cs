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
        //gammy way of doing this. Should set up a delegate on player data that announces when everything is in place. 
        yield return new WaitForSeconds(0.2f);
        rosta = GameObject.Find("PlayerData" + "(Clone)").GetComponent<RostaInfo>();
        yield break;
    }   

    public void SetUnit(UnitInfo unitInfo)
    {
        unit = unitInfo;
        //might need to tie the aspect data to the unitdata somehow. 
        unit.aspectData.SetAspectData();
        unitName.text = unit.unitName;
        //This is where we will have to get it to choose WHAT weapon data it puts in. Should probably make an equipment manager's job.  
        switch (unit.aspectData.className)
        {
            case "Defender":
                unit.weaponData = new MeleeWeaponData();
                unit.offHandData = new ShieldData();
                unit.armourData = new LeatherArmourData();
                unit.aspectData = new DefenderData();
                break;
            case "Scout":
                unit.weaponData = new ShortbowData();
                break;
            case "Priest":
                unit.weaponData = new MeleeWeaponData();
                break;
        }

        Sprite sprite;
        unit.weaponData.SetData(unit);
        sprite = Resources.Load<Sprite>(unit.weaponData.imageFile);
        unitWeaponImage.sprite = sprite;

        //This is ultimately how I'll have to access the visual from gameassets. 
        Instantiate(unit.aspectData.GetVisual(), point.transform);
    }

    public void SelectUnit()
    {
        rosta.companyPosition = position;
        SceneManager.LoadScene("TroopRosta");
    }
}
