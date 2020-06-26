using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemEntryButton : MonoBehaviour
{
    //hold a reference to the specific item
    public ItemEntry itemEntry;
    public EquipmentInfoPanel equipPanel;
    public Image image;
    public Text text;

    private void Awake()
    {
        equipPanel = GameObject.Find("Equipment Information Panel").GetComponent<EquipmentInfoPanel>();
    }

    //have a button that sets the data in the equipment panel
    public void SendItemData()
    {
        equipPanel.SetData(itemEntry.itemData);
    }

    public void SetButtonData(ItemEntry _itemEntry)
    {
        itemEntry = _itemEntry;
        image.sprite = itemEntry.itemData.SetImage();
        if (_itemEntry.infinite)
        {
            text.text = "-";
        }
        else { text.text = _itemEntry.amount.ToString(); }
        
    }

    public void UnsubscribeAndDestroy()
    {
        //unsubscribe
        Destroy(this);
    }
}
