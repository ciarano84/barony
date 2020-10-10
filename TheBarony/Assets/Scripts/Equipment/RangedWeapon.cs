using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedWeaponData : WeaponData
{
    public int missDistance = 20;
    public int maxAmmo;
    public int currentAmmo;
    public int rangedDamage;

    public ActionCost reloadSpeed;
}

public class RangedWeapon : Weapon
{
    public RangedWeaponData rangedWeaponData;
    public bool NextToEnemy;

    public override IEnumerator Attack(Target target)
    {
        currentTarget = target;
        RangeFinder.FindAdjacentUnits(owner);
        foreach (Unit unit in owner.adjacentUnits)
        {
            if (unit.unitInfo.faction != owner.unitInfo.faction)
            {
                NextToEnemy = true;
                break;
            }
        }

        owner.remainingActions--;
        owner.aimingBow = true;
        owner.FaceDirection(currentTarget.unitTargeted.gameObject.transform.position);
        yield return new WaitForSeconds(0.3f);
        owner.unitAnim.SetTrigger("rangedAttack");

        //attack event code WAS here. 

        yield break;
    }

    public override void AttackEvent()
    {
        int bonuses = 0;
        if (NextToEnemy) bonuses = -1;
        Result hit = AttackManager.AttackRoll(owner, currentTarget.unitTargeted.GetComponent<Unit>(), bonuses);
        GameObject missile = Instantiate(GameAssets.i.ArrowModel, owner.offHandSlot.position, owner.transform.rotation) as GameObject;
        missile.GetComponent<Missile>().target = currentTarget.unitTargeted.transform.position + new Vector3(0, currentTarget.unitTargeted.GetComponent<TacticsMovement>().halfHeight);

        missile.GetComponent<Missile>().Launch(hit);
        missile.GetComponent<Missile>().targetUnit = currentTarget.unitTargeted;
        missile.GetComponent<Missile>().firingWeapon = this;

        if (hit == Result.FAIL)
        { 
            DamagePopUp.Create(transform.position + new Vector3(0, 2 * GetComponent<TacticsMovement>().halfHeight), "miss", false);
        }

        owner.aimingBow = false;
        NextToEnemy = false;
        owner.FaceDirection(currentTarget.unitTargeted.gameObject.transform.position);
        rangedWeaponData.currentAmmo--;
        Initiative.EndAction();
    }

    public void DamageEvent(Unit unit, Result _result)
    {
        int storedDamage = owner.unitInfo.currentDamage;
        owner.unitInfo.currentDamage = rangedWeaponData.rangedDamage;

        //handle a block;
        if (_result == Result.PARTIAL)
        {
            if (currentTarget.unitTargeted.GetComponent<Shield>())
            {
                AttackManager.DamageRoll(owner, currentTarget.unitTargeted.GetComponent<Unit>());
            }
        }
            
        else AttackManager.DamageRoll(owner, currentTarget.unitTargeted.GetComponent<Unit>());
        owner.unitInfo.currentDamage = storedDamage;
    }

    public override void GetTargets()
    {
        targets.Clear();
        if (rangedWeaponData.currentAmmo == 0) return;

        foreach (TacticsMovement unit in Initiative.order)
        {
            if (unit != owner.GetComponent<TacticsMovement>())
            {
                if (RangeFinder.LineOfSight(owner, unit))
                {
                    AddTarget(unit, owner.currentTile); ;
                }
            }
        }
    }

    public void Reload(ActionCost actionCost)
    {
        rangedWeaponData.currentAmmo = rangedWeaponData.maxAmmo;
        DamagePopUp.Create(transform.position + new Vector3(0, gameObject.GetComponent<TacticsMovement>().halfHeight), "Arrow nocked", false);
        if (actionCost == ActionCost.main)
        {
            owner.remainingActions -= 1; }
        else 
        { owner.remainingMove = 0; }
        Initiative.queuedActions++;
        Initiative.EndAction();
    }
}
