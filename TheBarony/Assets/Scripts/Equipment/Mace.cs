using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MaceData : MeleeWeaponData
{
    public override Sprite SetImage()
    {
        return GameAssets.i.Mace;
    }

    public override void SetData(UnitInfo unitInfo)
    {
        name = "Mace";
        actionsPerAttack = 2;
        rangeType = Range.melee;
        slot = Slot.oneHanded;
        description = "A metal club. \r\nSupposedly good against heavily armoured enemies. Supposedly.";
    }

    public override void EquipItem(Unit unit)
    {
        Mace weapon = unit.gameObject.AddComponent<Mace>();
        weapon.owner = unit.gameObject.GetComponent<PlayerCharacter>();
        weapon.weaponData = this;

        unit.mainWeapon = weapon;

        //Will have to have an amendment when we get off hand weapons. 
    }
}

public class Mace : MeleeWeapon
{

}