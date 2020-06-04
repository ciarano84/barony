using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoPanel : MonoBehaviour
{
    public Text unitName;
    public Image unitWeaponImage;
    public UnitInfo unit;
    public GameObject point;

    public void SetUnit(UnitInfo unitInfo)
    {
        unit = unitInfo;
        unitName.text = unit.unitName;
        unit.weaponData = new WeaponData();

        Sprite sprite;
        sprite = Resources.Load<Sprite>(unit.weaponData.imageFile);
        unitWeaponImage.sprite = sprite;

        GameObject instance = Instantiate(Resources.Load(unit.unitVisual)) as GameObject;
        instance.transform.position = point.transform.position;
        //this is nonsense just to get stuff showing right for the time being:
        instance.transform.Rotate(0,90,0);
    }
}
