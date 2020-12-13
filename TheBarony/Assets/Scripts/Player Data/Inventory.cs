using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//These are NOT related to the respective items, but just allow me to find them via the GetItem method below.
public enum ItemRef { SHORTBOW, NONE, SHORTSWORD, LONGBOW, LEATHERARMOUR, CHAINSHIRTARMOUR, MACE, GREATAXE, DAGGER, SHIELD, DEFAULT }

public class ItemEntry
{
    public bool infinite = false;
    public int amount = 0;
    public ItemData itemData;
    public ItemEntry(ItemData _itemData, int _count, bool _infinite = false)
    {
        itemData = _itemData;
        amount = _count;
        infinite = _infinite; 
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

    public void UpdateEntry(ItemData _itemData, int _amount, bool _infinite = false)
    {
        if (_amount < -1)
        {
            Debug.LogError("the inventory has been asked to deduct more than 1 of an item. You only decrement the inventory by a maximum of 1.");
        }
        bool itemInInventory = false;
        for (int count = 0; count < inventory.Count; count++)
        {
            if (inventory[count].itemData.name == _itemData.name)
            {
                itemInInventory = true;
                inventory[count].amount += _amount;

                //Clear the itementry from the inventory if it is reduced to 0. 
                if ((inventory[count].amount == 0) && !inventory[count].infinite) { inventory.RemoveAt(count); }

                break;
            }
        }
        if (!itemInInventory)
        {
            //This seems hacky but works for now. I'm supposed to set item data to a unit. But there's no unit, so setting it to null. 
            _itemData.SetData(null);
            inventory.Add(new ItemEntry(_itemData, _amount, _infinite));
        }
    }

    public static ItemData GetItem(ItemRef reference)
    {
        switch (reference)
        {
            case ItemRef.SHORTBOW: return new ShortbowData();
            case ItemRef.NONE: return null;
            case ItemRef.SHORTSWORD: return new ShortswordData();
            case ItemRef.LONGBOW: return new LongbowData();
            case ItemRef.LEATHERARMOUR: return new LeatherArmourData();
            case ItemRef.CHAINSHIRTARMOUR: return new ChainShirtArmourData();
            case ItemRef.MACE: return new MaceData();
            case ItemRef.GREATAXE: return new GreataxeData();
            case ItemRef.DAGGER: return new DaggerData();
            case ItemRef.SHIELD: return new ShieldData();
            default: return null;
        }
    }
}
