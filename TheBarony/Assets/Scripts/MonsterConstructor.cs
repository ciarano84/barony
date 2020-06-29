using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterConstructor : MonoBehaviour
{
    public Weapon mainWeapon;
    public Item offHand;
    public Item armour;
    public Aspect aspect;
    public int level;

    public void SetUpMonster()
    {
        mainWeapon.GetItemData();
        mainWeapon.itemData.SetData(gameObject.GetComponent<Unit>().unitInfo, Slot.mainHand);
        mainWeapon.itemData.EquipItem(gameObject.GetComponent<Unit>());
        if (offHand != null)
        {
            offHand.GetItemData();
            offHand.itemData.SetData(gameObject.GetComponent<Unit>().unitInfo, Slot.offHand);
            offHand.itemData.EquipItem(gameObject.GetComponent<Unit>());
        }
        if (armour != null)
        {
            armour.GetItemData();
            armour.itemData.SetData(gameObject.GetComponent<Unit>().unitInfo, Slot.armour);
            armour.itemData.EquipItem(gameObject.GetComponent<Unit>());
        }
        aspect.GetAspectData();
        aspect.aspectData.SetAspectData(gameObject.GetComponent<Unit>().unitInfo);
        aspect.owner = gameObject.GetComponent<Unit>();
        aspect.aspectData.Level1();
    }
}
