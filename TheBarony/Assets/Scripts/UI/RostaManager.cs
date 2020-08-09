using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class RostaManager : MonoBehaviour
{
    public GameObject itemInformationPanel;
    public GameObject pedestal;
    public static Slot slotSelected;
    public UnitInfo unit;
    public GameObject unitPrefab;

    EquipmentInfoPanel equipPanel;
    RostaInfo rosta;

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
    public GameObject mainHandSlotHighlight;

    //Off Hand Item
    public TextMeshProUGUI offHandText;
    public Image offHandImage;
    public Button offHandSelectButton;
    public GameObject offHandSlotHighlight;

    //Attire
    public TextMeshProUGUI attireText;
    public Image attireImage;
    public Button attireSelectButton;
    public GameObject armourSlotHighlight;

    //Accessory 1
    public TextMeshProUGUI accessory1Text;
    public Image accessory1Image;
    public Button accessory1SelectButton;
    public GameObject accessory1SlotHighlight;

    //Accessory 2
    public TextMeshProUGUI accessory2Text;
    public Image accessory2Image;
    public Button accessory2Button;
    public GameObject accessory2SlotHighlight;

    GameObject[] highlights;

    private void Start()
    {
        rosta = GameObject.Find("PlayerData" + "(Clone)").GetComponent<RostaInfo>();
        StartCoroutine(WaitAndShowStats());
        equipPanel = itemInformationPanel.GetComponent<EquipmentInfoPanel>();
        highlights = new GameObject[] { mainHandSlotHighlight, offHandSlotHighlight, armourSlotHighlight, accessory1SlotHighlight, accessory2SlotHighlight };
    }

    IEnumerator WaitAndShowStats()
    {
        yield return new WaitForSeconds(0.1f);

        //Move the troop into the castle (but keep them in the company as well, for this scene);
        rosta.castle.Add(RostaInfo.currentEncounter.selectedCompany.units[rosta.companyPosition]);
        //RostaInfo.currentEncounter.selectedCompany.units.Remove(RostaInfo.currentEncounter.selectedCompany.units[rosta.companyPosition]);
        rosta.currentUnitShown = rosta.castle.Count-1;

        ShowStats();
        yield break;
    }

    public void ShowStats()
    {
        Destroy(unitPrefab);
        unit = rosta.castle[rosta.currentUnitShown];
        unit.mainWeaponData.SetData(unit);
        unitPrefab = Instantiate(GameAssets.i.PlayerUnit, pedestal.transform);
        unitPrefab.GetComponent<Unit>().unitInfo = unit;

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

        if (equipPanel.isActiveAndEnabled)
        {
            SetHighlight(true, equipPanel.slotToSwapOut);
        } else SetHighlight(false);
    }

    public void OnRightButtonClick() { GetNextsUnit(); }
    public void GetNextsUnit()
    {
        itemInformationPanel.SetActive(false);
        ShowUnit(Direction.right);
    }

    public void OnLeftButtonClick() { GetPreviousUnit(); }
    public void GetPreviousUnit()
    {
        itemInformationPanel.SetActive(false);
        ShowUnit(Direction.left);
    }

    public void SelectUnit()
    {
        //Add unit to company.
        RostaInfo.currentEncounter.selectedCompany.units[rosta.companyPosition] = rosta.castle[rosta.currentUnitShown];

        //Remove it from rosta.
        rosta.castle.Remove(rosta.castle[rosta.currentUnitShown]);

        SceneManager.LoadScene("SquadView");
    }

    void ShowUnit(Direction direction)
    {
        if (direction == Direction.right)
        {
            rosta.currentUnitShown++;
            if (rosta.currentUnitShown >= rosta.castle.Count)
            {
                rosta.currentUnitShown = 0;
            }
        }
        else
        {
            rosta.currentUnitShown--;
            if (rosta.currentUnitShown < 0)
            {
                rosta.currentUnitShown = (rosta.castle.Count - 1);
            }
        }
        ShowStats();
    }

    public void MainHandSelect()
    {
        slotSelected = Slot.mainHand;
        SetEquipmentPanel(unit.mainWeaponData, Slot.mainHand);
    }

    public void OffHandSelect()
    {
        slotSelected = Slot.offHand;
        SetEquipmentPanel(unit.offHandData, Slot.offHand);
    }

    public void ArmourSelect()
    {
        slotSelected = Slot.armour;
        SetEquipmentPanel(unit.armourData, Slot.armour);
    }

    public void Accessory1Select()
    {
        slotSelected = Slot.accessory;
        SetEquipmentPanel(unit.accessory1, Slot.accessory1);
    }

    public void Accessory2Select()
    {
        slotSelected = Slot.accessory;
        SetEquipmentPanel(unit.accessory2, Slot.accessory2);
    }

    void SetEquipmentPanel(ItemData itemData, Slot slot)
    {
        itemInformationPanel.SetActive(true);
        equipPanel.UpdateEquipmentInfoPanel(itemData, slot);
        SetHighlight(true, slot);
    }

    public void SetHighlight(bool on, Slot slot = Slot.mainHand)
    {
        foreach (GameObject g in highlights)
        {
            g.SetActive(false);
        }
        if (on)
        {
            switch (slot)
            {
                case Slot.mainHand:
                    mainHandSlotHighlight.SetActive(true);
                    break;
                case Slot.offHand:
                    offHandSlotHighlight.SetActive(true);
                    break;
                case Slot.armour:
                    armourSlotHighlight.SetActive(true);
                    break;
                case Slot.accessory1:
                    accessory1SlotHighlight.SetActive(true);
                    break;
                case Slot.accessory2:
                    accessory2SlotHighlight.SetActive(true);
                    break;
            }
        }
    }
}
