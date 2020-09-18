using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongbowData : RangedWeaponData
{
    public override Sprite SetImage()
    {
        return GameAssets.i.Longbow;
    }

    public override void SetData(UnitInfo unitInfo, Slot slotToEquipTo = Slot.mainHand)
    {
        name = "Longbow";
        actionsPerAttack = 1;
        slot = Slot.twoHanded;
        rangeType = WeaponData.Range.ranged;
        weight = Weight.medium;
        description = "A potentially ornemantal longbow. \r\nIt is indeed long. A day of carrying this around and you'll wish you'd just been shot by it.";

        range = 200;
        missDistance = 20;
        maxAmmo = 1;
        currentAmmo = 1;
        rangedDamage = 4;
        reloadSpeed = ActionCost.main;

        if (unitInfo != null)
        {
            unitInfo.mainWeaponData = this;
        }
    }

    public override void EquipItem(Unit unit)
    {
        itemModel = GameObject.Instantiate(GameAssets.i.LongbowModel, unit.offHandSlot);
        itemModel.transform.position = unit.offHandSlot.position;

        Shortbow weapon = unit.gameObject.AddComponent<Shortbow>();
        weapon.owner = unit.gameObject.GetComponent<TacticsMovement>();
        weapon.rangedWeaponData = this;
        //this next line looks like it's not needed BUT IT REALLY IS
        weapon.weaponData = this;
        unit.mainWeapon = weapon;

        //not sure what we're gonna do when they have TWO shortswords. 
        Animator animator = weapon.owner.rig.gameObject.GetComponent<Animator>();
        animator.runtimeAnimatorController = GameAssets.i.OneHanded as RuntimeAnimatorController;

        //Set up for an action. 
        Reload reload = new Reload();
        reload.SetActionButtonData(unit);
        unit.actions.Add(reload);
    }
}

public class Longbow : RangedWeapon
{
    public override void GetItemData()
    {
        itemData = new LongbowData();
    }
}
