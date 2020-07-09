﻿using System.Collections;
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
        Greataxe weapon = unit.gameObject.AddComponent<Greataxe>();
        weapon.owner = unit.gameObject.GetComponent<PlayerCharacter>();
        weapon.weaponData = this;

        unit.mainWeapon = weapon;

        //Will have to have an amendment when we get off hand weapons. 
    }
}

public class Greataxe : MeleeWeapon
{
    public override void GetItemData()
    {
        itemData = new GreataxeData();
    }

    //include something in here for limiting the weapon's usefulness when you are surrounded by barriers. 
    public override IEnumerator Attack(Target target)
    {
        targets.Clear();

        owner.remainingActions--;

        //Find out what is adjacent to the target.
        target.unitTargeted.FindAdjacentUnits();

        bool adjacent = false;
        foreach (Unit _unit in target.unitTargeted.adjacentUnits)
        {
            if (_unit == owner) adjacent = true;
        }

        if (adjacent)
        {
            owner.GetComponent<TacticsMovement>().FaceDirection(target.unitTargeted.gameObject.transform.position);
        }
        else
        {
            Initiative.queuedActions++;
            owner.MoveToTile(target.tileToAttackFrom, target.unitTargeted.currentTile.transform.position);
        }

        yield return new WaitUntil(() => !owner.moving);
        owner.unitAnim.SetTrigger("2Hmelee");
        yield return new WaitForSeconds(0.3f);

        int bonuses = 0;

        //See if any of the adjacent units to the target allow you to flank. 
        foreach (Unit unit in target.unitTargeted.adjacentUnits)
        {
            if (unit.unitInfo.faction != target.unitTargeted.unitInfo.faction)
            {
                Vector3 relTargetPosition = transform.InverseTransformPoint(target.unitTargeted.transform.position);
                Vector3 relOtherAttackerPosition = transform.InverseTransformPoint(unit.transform.position);
                if (relOtherAttackerPosition.z > (relTargetPosition.z + 0.1f))
                {
                    bonuses++;
                    break;
                }
            }
        }

        owner.GetComponent<TacticsMovement>().GetCurrentTile();
        if (owner.currentTile.barrierCount > 0)
        {
            bonuses--;
            DamagePopUp.Create(gameObject.transform.position + new Vector3(0, (gameObject.GetComponent<TacticsMovement>().halfHeight) + 0.5f), "Hindered", false);
            if (owner.currentTile.barrierCount > 5) bonuses--;
        }

        bool hit = AttackManager.AttackRoll(owner, target.unitTargeted.GetComponent<Unit>(), bonuses);

        if (hit)
        {
            AttackManager.DamageRoll(owner, target.unitTargeted.GetComponent<Unit>());
        }

        if (!hit)
        {
            DamagePopUp.Create(target.unitTargeted.gameObject.transform.position + new Vector3(0, target.unitTargeted.gameObject.GetComponent<TacticsMovement>().halfHeight), "miss", false);
        }

        yield return new WaitForSeconds(2f);

        Initiative.EndAction();

        yield break;
    }
}
