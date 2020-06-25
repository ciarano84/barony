using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class EquipmentInfoPanel : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public Image itemImage;
    ItemData shownItem;
    Inventory inventory;

    public Image[] inventorySlots = new Image[5];

    public void SetData(ItemData item)
    {
        shownItem = item;
        itemName.text = item.name;
        itemDescription.text = item.description;
        itemImage.sprite = item.SetImage();
    }

    public void OnEnable()
    {
        inventory = GameObject.Find("PlayerData" + "(Clone)").GetComponent<Inventory>();
        UpdateEquipmentInfoPanel();
    }

    void UpdateEquipmentInfoPanel()
    {
        for (int count = 0; count < Inventory.inventory.Count; count++)
        {
            int slotCount = 0;
            foreach (ItemEntry itemEntry in Inventory.inventory)
            {
                if (RostaManager.slotSelected == Inventory.inventory[count].itemData.slot)
                {
                    inventorySlots[slotCount].sprite = Inventory.inventory[count].itemData.SetImage();
                    slotCount++;
                }
                else if (Inventory.inventory[count].itemData.slot == Slot.oneHanded && (RostaManager.slotSelected == Slot.mainHand || RostaManager.slotSelected == Slot.offHand))
                {
                    inventorySlots[slotCount].sprite = Inventory.inventory[count].itemData.SetImage();
                    slotCount++;
                }
                else if (Inventory.inventory[count].itemData.slot == Slot.twoHanded && (RostaManager.slotSelected == Slot.mainHand))
                {
                    inventorySlots[slotCount].sprite = Inventory.inventory[count].itemData.SetImage();
                    slotCount++;
                }
            }
        }
    }
}
