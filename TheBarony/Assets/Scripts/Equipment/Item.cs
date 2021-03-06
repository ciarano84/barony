﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Slot { mainHand, offHand, oneHanded, twoHanded, armour, accessory, accessory1, accessory2}

[System.Serializable]
public abstract class ItemData
{
    //As I add classes that inherit, we can get move variation. Crucially there should be a 1:1 relaionship between weapon scripts and WeaponData scripts.
    public string name;
    public string description;
    public Slot slot;
    public GameObject itemModel;
    public enum Weight { light, medium, heavy, cumbersome };
    public Weight weight = Weight.light;

    public abstract Sprite SetImage();

    public abstract void SetData(UnitInfo unitInfo, Slot slot = Slot.mainHand);

    public abstract void EquipItem(Unit unit);
}

public abstract class Item : MonoBehaviour
{
    public TacticsMovement owner;
    public ItemData itemData;

    public abstract void GetItemData();
}
