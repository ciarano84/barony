using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq.Expressions;

public class EquipmentInfoPanel : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public Image itemImage;
    public RostaManager rostaManager;
    public GameObject ItemSelectParentUI;
    public GameObject ItemEntryButtonPrefab;
    public GameObject equipButton;
    public Slot slotToSwapOut;

    ItemData itemToSwapOut;
    ItemData shownItem;
    Inventory inventory;

    public void SetItemToSwapOut(ItemData item)
    {
        itemToSwapOut = item;
        SetData(itemToSwapOut);
    }

    public void SetData(ItemData item)
    {
        shownItem = item;
        item.SetData(null);
        itemName.text = item.name;
        itemDescription.text = item.description;
        itemImage.sprite = item.SetImage();
        if (shownItem.name != itemToSwapOut.name)
        {
            equipButton.SetActive(true);
        }
        else 
        {
            equipButton.SetActive(false);
        }
    }

    public void OnEnable()
    {
        inventory = GameObject.Find("PlayerData" + "(Clone)").GetComponent<Inventory>();
        //UpdateEquipmentInfoPanel();
    }

    public void UpdateEquipmentInfoPanel(ItemData itemData, Slot slot)
    {
        Clear();
        SetItemToSwapOut(itemData);
        slotToSwapOut = slot;
        ShowInventoryOptions();
    }

    public void CreateItemEntryButton(ItemEntry _itemEntry)
    {
        GameObject button = Instantiate(ItemEntryButtonPrefab);

        button.transform.SetParent(ItemSelectParentUI.transform);
        //button.transform.parent = ItemSelectParentUI.transform;

        button.GetComponent<ItemEntryButton>().SetButtonData(_itemEntry);
    }

    void Clear()
    {
        foreach (Transform child in ItemSelectParentUI.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        itemName.text = "";
        itemDescription.text = "";
    }

    public void Equip()
    {
        //add what the character has equipped to the inventory.
        inventory.UpdateEntry(itemToSwapOut, 1);

        //take one of what they WANT equipped from the inventory. 
        inventory.UpdateEntry(shownItem, -1);

        //put the new itemData for what they WANT equipped onto the character. 
        shownItem.SetData(rostaManager.unit, slotToSwapOut);
        HandleTwoHandedWeapons(shownItem);

        //get the rostmanager to update to reflect the change, including shown item.
        rostaManager.ShowStats();
        UpdateEquipmentInfoPanel(shownItem, shownItem.slot);
    }

    void ShowInventoryOptions()
    {
        for (int count = 0; count < Inventory.inventory.Count; count++)
        {
            if (RostaManager.slotSelected == Inventory.inventory[count].itemData.slot)
            {
                CreateItemEntryButton(Inventory.inventory[count]);
            }
            else if (Inventory.inventory[count].itemData.slot == Slot.oneHanded && (RostaManager.slotSelected == Slot.mainHand || RostaManager.slotSelected == Slot.offHand))
            {
                CreateItemEntryButton(Inventory.inventory[count]);
            }
            else if (Inventory.inventory[count].itemData.slot == Slot.twoHanded && (RostaManager.slotSelected == Slot.mainHand))
            {
                CreateItemEntryButton(Inventory.inventory[count]);
            }
        }
    }

    void HandleTwoHandedWeapons(ItemData newItemData)
    {
        if (newItemData.slot == Slot.twoHanded)
        {
            BlankItemData blank = new BlankItemData();
            blank.SetData(null);
            if (rostaManager.unit.offHandData.name != blank.name)
            {
                inventory.UpdateEntry(rostaManager.unit.offHandData, 1);
                blank.SetData(rostaManager.unit, Slot.offHand);
            }
        }
        if (slotToSwapOut == Slot.offHand)
        {
            if (rostaManager.unit.mainWeaponData.slot == Slot.twoHanded)
            {
                ShortswordData shortsword = new ShortswordData();
                shortsword.SetData(rostaManager.unit, Slot.mainHand);
                inventory.UpdateEntry(rostaManager.unit.mainWeaponData, 1);
            }
        }
    }
}
