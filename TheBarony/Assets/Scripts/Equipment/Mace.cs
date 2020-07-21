using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MaceData : MeleeWeaponData
{
    public override Sprite SetImage()
    {
        return GameAssets.i.Mace;
    }

    public override void SetData(UnitInfo unitInfo, Slot slotToEquipTo = Slot.mainHand)
    {
        name = "Mace";
        actionsPerAttack = 1;
        rangeType = Range.melee;
        slot = Slot.oneHanded;
        weight = Weight.medium;
        description = "A metal club. \r\nSupposedly good against heavily armoured enemies. Supposedly.";

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
        GameObject model = GameObject.Instantiate(GameAssets.i.MaceModel, unit.mainHandSlot);
        model.transform.position = unit.mainHandSlot.position;

        Mace weapon = unit.gameObject.AddComponent<Mace>();
        weapon.owner = unit.gameObject.GetComponent<TacticsMovement>();
        weapon.weaponData = this;

        unit.mainWeapon = weapon;

        //not sure what we're gonna do when they have TWO shortswords. 
        Animator animator = weapon.owner.rig.gameObject.GetComponent<Animator>();
        animator.runtimeAnimatorController = GameAssets.i.OneHanded as RuntimeAnimatorController;

        //Will have to have an amendment when we get off hand weapons. 
    }
}

public class Mace : MeleeWeapon
{
    public override void GetItemData()
    {
        itemData = new MaceData();
    }
}