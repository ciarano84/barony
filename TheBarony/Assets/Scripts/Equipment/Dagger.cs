using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerData : MeleeWeaponData
{
    public override Sprite SetImage()
    {
        return GameAssets.i.Dagger;
    }

    public override void SetData(UnitInfo unitInfo, Slot slotToEquipTo = Slot.mainHand)
    {
        name = "Dagger";
        actionsPerAttack = 1;
        slot = Slot.oneHanded;
        rangeType = Range.melee;
        weight = Weight.light;
        description = "A fancy dagger. \r\nYou're not sure how it got here. Someone probably wanted shot of it quick.";

        weaponDamage = -1;

        if (unitInfo != null)
        {
            switch (slotToEquipTo)
            {
                case Slot.mainHand:
                    unitInfo.mainWeaponData = this;
                    break;
                case Slot.offHand:
                    unitInfo.offHandData = this;
                    break;
                default:
                    unitInfo.mainWeaponData = this;
                    break;
            }
        }
    }

    public override void EquipItem(Unit unit)
    {
        itemModel = GameObject.Instantiate(GameAssets.i.DaggerModel, unit.mainHandSlot);
        itemModel.transform.position = unit.mainHandSlot.position;

        Dagger weapon = unit.gameObject.AddComponent<Dagger>();
        weapon.owner = unit.gameObject.GetComponent<TacticsMovement>();
        weapon.weaponData = this;

        unit.mainWeapon = weapon;

        //not sure what we're gonna do when they have TWO shortswords. 
        Animator animator = weapon.owner.rig.gameObject.GetComponent<Animator>();
        animator.runtimeAnimatorController = GameAssets.i.OneHanded as RuntimeAnimatorController;

        //Will have to have an amendment when we get off hand weapons. 
    }
}

public class Dagger : MeleeWeapon
{
    public override void GetItemData()
    {
        itemData = new DaggerData();
    }

    public override Critical CoreCritical() { return new c_Gouge(); }
}
