using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class EquipmentInfoPanel : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public Image itemImage;
    ItemData shownItem;

    public void SetData(ItemData item)
    {
        shownItem = item;
        itemName.text = item.name;
        itemDescription.text = item.description;
        itemImage.sprite = item.SetImage();
    }
}
