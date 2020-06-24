﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class RostaManager : MonoBehaviour
{
    //this class is totally borked as it's based on having game objects. Need to update it to use player data. 
    RostaInfo rosta;
    public GameObject pedestal;
    GameObject unitVisual;
    public GameObject itemInformationPanel;
    UnitInfo unit;

    enum Direction {left, right};

    public Text nameText;
    public Text classText;
    public Text breathText;
    public Text attackText;
    public Text defenceText;
    public Text damageText;
    public Text armourText;
    public Text speedText;

    //Main Hand Item 
    public TextMeshProUGUI mainHandText;
    public Image mainHandImage;
    public Button mainHandSelectButton;

    //Off Hand Item
    public TextMeshProUGUI offHandText;
    public Image offHandImage;
    public Button offHandSelectButton;

    //Attire
    public TextMeshProUGUI attireText;
    public Image attireImage;
    public Button attireSelectButton;

    //Accessory 1
    public TextMeshProUGUI accessory1Text;
    public Image accessory1Image;
    public Button accessory1SelectButton;

    //Accessory 2
    public TextMeshProUGUI accessory2Text;
    public Image accessory2Image;
    public Button accessory2Button;

    private void Start()
    {
        rosta = GameObject.Find("PlayerData" + "(Clone)").GetComponent<RostaInfo>();
        StartCoroutine(WaitAndShowStats());
    }

    IEnumerator WaitAndShowStats()
    {
        yield return new WaitForSeconds(0.1f);

        //Move the troop out the squad and into the rosta
        rosta.rosta.Add(rosta.squad[rosta.companyPosition]);
        //rosta.squad.Remove(rosta.squad[rosta.companyPosition]);
        rosta.currentUnitShown = rosta.rosta.Count-1;

        ShowStats();
        yield break;
    }

    public void ShowStats()
    {
        Destroy(unitVisual);
        unit = rosta.rosta[rosta.currentUnitShown];

        unitVisual = Instantiate(unit.aspectData.GetVisual(), pedestal.transform);

        nameText.text = (unit.unitName);
        classText.text = (unit.aspectData.className);
        breathText.text = (unit.baseBreath.ToString());
        attackText.text = (unit.baseAttack.ToString());
        defenceText.text = (unit.baseDefence.ToString()); //This won't show shield effects. might be fine. 
        damageText.text = (unit.baseStrength.ToString());
        armourText.text = (unit.baseToughness.ToString());
        speedText.text = (unit.baseMove.ToString());

        mainHandText.text = unit.mainWeaponData.name;
        mainHandImage.sprite = unit.mainWeaponData.SetImage();

        offHandText.text = unit.offHandData.name;
        offHandImage.sprite = unit.offHandData.SetImage();

        attireText.text = unit.armourData.name;
        attireImage.sprite = unit.armourData.SetImage();

        accessory1Text.text = unit.accessory1.name;
        accessory1Image.sprite = unit.accessory1.SetImage();

        accessory2Text.text = unit.accessory2.name;
        accessory2Image.sprite = unit.accessory2.SetImage();
    }

    public void OnRightButtonClick() { GetNextsUnit(); }
    public void GetNextsUnit()
    {
        ShowUnit(Direction.right);
    }

    public void OnLeftButtonClick() { GetPreviousUnit(); }
    public void GetPreviousUnit()
    {
        ShowUnit(Direction.left);
    }

    public void SelectUnit()
    {
        //Add unit to squad.
        rosta.squad[rosta.companyPosition] = rosta.rosta[rosta.currentUnitShown];

        //Remove it from rosta.
        rosta.rosta.Remove(rosta.rosta[rosta.currentUnitShown]);

        SceneManager.LoadScene("SquadView");
    }

    void ShowUnit(Direction direction)
    {
        if (direction == Direction.right)
        {
            rosta.currentUnitShown++;
            if (rosta.currentUnitShown >= rosta.rosta.Count)
            {
                rosta.currentUnitShown = 0;
            }
        }
        else
        {
            rosta.currentUnitShown--;
            if (rosta.currentUnitShown < 0)
            {
                rosta.currentUnitShown = (rosta.rosta.Count - 1);
            }
        }
        ShowStats();
    }

    public void MainHandSelect()
    {
        itemInformationPanel.SetActive(true);
        itemInformationPanel.GetComponent<EquipmentInfoPanel>().SetData(unit.mainWeaponData);
    }

    public void OffHandSelect()
    {
        itemInformationPanel.SetActive(true);
        itemInformationPanel.GetComponent<EquipmentInfoPanel>().SetData(unit.offHandData);
    }

    public void ArmourSelect()
    {
        itemInformationPanel.SetActive(true);
        itemInformationPanel.GetComponent<EquipmentInfoPanel>().SetData(unit.armourData);
    }

    public void Accessory1Select()
    {
        itemInformationPanel.SetActive(true);
        itemInformationPanel.GetComponent<EquipmentInfoPanel>().SetData(unit.accessory1);
    }

    public void Accessory2Select()
    {
        itemInformationPanel.SetActive(true);
        itemInformationPanel.GetComponent<EquipmentInfoPanel>().SetData(unit.accessory2);
    }
}