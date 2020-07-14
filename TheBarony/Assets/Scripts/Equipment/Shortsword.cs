using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortswordData : MeleeWeaponData
{
    public override Sprite SetImage()
    {
        return GameAssets.i.ShortSword;
    }

    public override void SetData(UnitInfo unitInfo, Slot slotToEquipTo = Slot.mainHand)
    {
        name = "Shortsword";
        actionsPerAttack = 1;
        slot = Slot.oneHanded;
        rangeType = Range.melee;
        weight = Weight.medium;
        description = "A short sword. \r\nWell made but really nothing to write home about.";

        weaponDamage = 1;

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
        GameObject model = GameObject.Instantiate(GameAssets.i.ShortswordModel, unit.mainHandSlot);
        model.transform.position = unit.mainHandSlot.position;

        Shortsword weapon = unit.gameObject.AddComponent<Shortsword>();
        weapon.owner = unit.gameObject.GetComponent<PlayerCharacter>();
        weapon.weaponData = this; 
        
        unit.mainWeapon = weapon;

        //not sure what we're gonna do when they have TWO shortswords. 
        Animator animator = weapon.owner.rig.gameObject.GetComponent<Animator>();
        animator.runtimeAnimatorController = GameAssets.i.OneHanded as RuntimeAnimatorController;

        //Will have to have an amendment when we get off hand weapons. 
    }
}

public class Shortsword : MeleeWeapon
{
    public override void GetItemData()
    {
        itemData = new ShortswordData();
    }
}
