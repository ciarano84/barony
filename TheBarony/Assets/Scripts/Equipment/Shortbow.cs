using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortbowData : RangedWeaponData
{
    public override Sprite SetImage()
    {
        return GameAssets.i.ShortBow;
    }

    public override void SetData(UnitInfo unitInfo, Slot slotToEquipTo = Slot.mainHand)
    {
        name = "Shortbow";
        actionsPerAttack = 1;
        slot = Slot.twoHanded;
        rangeType = WeaponData.Range.ranged;
        weight = Weight.light;
        description = "A potentially ornemantal shortbow. \r\nLiable to break. But hopefully liable to hurt someone at least once beforehand.";
        
        range = 200;
        missDistance = 20;
        maxAmmo = 1;
        currentAmmo = 1;
        rangedDamage = 2;
        reloadSpeed = ActionCost.move;

        if (unitInfo != null)
        {
            unitInfo.mainWeaponData = this;
        }
    }

    public override void EquipItem(Unit unit)
    {
        GameObject model = GameObject.Instantiate(GameAssets.i.ShortbowModel, unit.offHandSlot);
        model.transform.position = unit.offHandSlot.position;

        Shortbow weapon = unit.gameObject.AddComponent<Shortbow>();
        weapon.owner = unit.gameObject.GetComponent<PlayerCharacter>();
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

public class Shortbow : RangedWeapon
{
    public override void GetItemData()
    {
        itemData = new ShortbowData();
    }
}
