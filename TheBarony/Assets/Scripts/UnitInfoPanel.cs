using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoPanel : MonoBehaviour
{
    public Text unitName;
    public Image unitWeaponImage;
    public UnitInfo unit;

    public void SetUnit(UnitInfo unitInfo)
    {
        unit = unitInfo;
        unitName.text = unit.unitName;
        unit.weaponData = new WeaponData();

        Sprite sprite;
        sprite = Resources.Load<Sprite>(unit.weaponData.imageFile);
        unitWeaponImage.sprite = sprite;
    }
}
