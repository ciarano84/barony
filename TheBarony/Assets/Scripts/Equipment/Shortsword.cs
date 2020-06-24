using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortswordData : MeleeWeaponData
{
    public override Sprite SetImage()
    {
        return GameAssets.i.ShortSword;
    }

    public override void SetData(UnitInfo unitInfo)
    {
        name = "Shortsword";
        actionsPerAttack = 2;
        rangeType = Range.melee;
        description = "A short sword. \r\nWell made but really nothing to write home about.";
    }

    public override void EquipItem(Unit unit)
    {
        Shortsword weapon = unit.gameObject.AddComponent<Shortsword>();
        weapon.owner = unit.gameObject.GetComponent<PlayerCharacter>();
        weapon.weaponData = this; 
        
        unit.mainWeapon = weapon;
        //unit.mainWeapon.itemData = this;

        //Will have to have an amendment when we get off hand weapons. 
    }
}

public class Shortsword : MeleeWeapon
{
    
}
