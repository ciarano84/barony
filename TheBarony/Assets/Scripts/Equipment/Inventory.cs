using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntry
{
    public bool infinite = false;
    public int amount = 0;
    public ItemData itemData;
    public ItemEntry(ItemData _itemData, int _count, bool _infinite = false)
    {
        itemData = _itemData;
        amount = _count;
    }
}

public class Inventory : MonoBehaviour
{
    [SerializeField]
    public static List<ItemEntry> inventory = new List<ItemEntry>();


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }


    public void UpdateEntry(ItemData itemData, int amount, bool infinite = false)
    {
        if (amount < -1)
        {
            Debug.LogError("the inventory has been asked to deduct more than 1 of an item. You only decrement the inventory by a maximum of 1.");
        }
        bool itemInInventory = false;
        for (int count = 0; count < inventory.Count; count++)
        {
            if (inventory[count].itemData == itemData)
            {
                itemInInventory = true;
                inventory[count].amount += amount;

                //Clear the itementry from the inventory if it is reduced to 0. 
                if ((inventory[count].amount == 0) && !inventory[count].infinite) { inventory.RemoveAt(count); }

                break;
            }
        }
        if (!itemInInventory)
        {
            //This seems hacky but works for now. I'm supposed to set item data to a unit. But there's no unit, so setting it to null. 
            itemData.SetData(null);
            inventory.Add(new ItemEntry(itemData, amount, infinite));
        }
    }
}
