using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreataxeData : MeleeWeaponData
{
    public override Sprite SetImage()
    {
        return GameAssets.i.Greataxe;
    }

    public override void SetData(UnitInfo unitInfo, Slot slotToEquipTo = Slot.mainHand)
    {
        name = "Greataxe";
        actionsPerAttack = 1;
        slot = Slot.twoHanded;
        rangeType = Range.melee;
        weight = Weight.heavy;
        description = "A greataxe. \r\nAt least, a great, big axe.";

        weaponDamage = 4;

        if (unitInfo != null)
        {
            unitInfo.mainWeaponData = this;
        }
    }

    public override void EquipItem(Unit unit)
    {
        itemModel = GameObject.Instantiate(GameAssets.i.GreataxeModel, unit.mainHandSlot);
        //GameObject model = GameObject.Instantiate(GameAssets.i.GreataxeModel, unit.mainHandSlot);
        itemModel.transform.position = unit.mainHandSlot.position;

        Greataxe weapon = unit.gameObject.AddComponent<Greataxe>();
        weapon.owner = unit.gameObject.GetComponent<TacticsMovement>();
        weapon.weaponData = this;

        unit.mainWeapon = weapon;

        //not sure what we're gonna do when they have TWO shortswords. 
        Animator animator = weapon.owner.rig.gameObject.GetComponent<Animator>();
        animator.runtimeAnimatorController = GameAssets.i.TwoHanded as RuntimeAnimatorController;

        //Will have to have an amendment when we get off hand weapons. 
    }
}

//consider moving this into a parent 'great weapon' class so I don't have to maintain the attack code for several differnt weapons. 
public class Greataxe : MeleeWeapon
{
    public override void GetItemData()
    {
        itemData = new GreataxeData();
    }

    public override void AttackEvent()
    {
        
        int bonuses = 0;

        //This next bit is greatweapon specific for when they attack next to obstacles. 
        owner.GetComponent<TacticsMovement>().GetCurrentTile();
        if (owner.currentTile.barrierCount > 0)
        {
            bonuses--;
            DamagePopUp.Create(gameObject.transform.position + new Vector3(0, (gameObject.GetComponent<TacticsMovement>().halfHeight) + 0.5f), "Hindered", false);
            if (owner.currentTile.barrierCount > 5) bonuses--;
        }

        Result hit = AttackManager.AttackRoll(owner, currentTarget.unitTargeted.GetComponent<Unit>(), bonuses);

        MeleeAttackOutcome(hit);
    }

    public override Critical CoreCritical() { return new c_Cripple(); }
}
