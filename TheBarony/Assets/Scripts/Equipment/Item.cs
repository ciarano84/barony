using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData
{
    //As I add classes that inherit, we can get move variation. Crucially there should be a 1:1 relaionship between weapon scripts and WeaponData scripts.
    public string name;
    public string description;
    public abstract Sprite SetImage();

    public abstract void SetData(UnitInfo unitInfo);

    public abstract void EquipItem(Unit unit);
}

public class Item : MonoBehaviour
{
    public PlayerCharacter owner;
    public ItemData itemData;
}
