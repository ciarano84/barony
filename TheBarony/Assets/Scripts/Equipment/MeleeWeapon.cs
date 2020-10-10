using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeaponData : WeaponData
{
    
}

public class MeleeWeapon : Weapon
{ 
    public MeleeWeaponData meleeWeaponData;
    bool flanking = false;

    public override IEnumerator Attack(Target target)
    {
        targets.Clear();
        currentTarget = target;
        owner.remainingActions--;

        //Find out what is adjacent to the target.
        RangeFinder.FindAdjacentUnits(currentTarget.unitTargeted);
        bool adjacent = false;
        foreach (Unit _unit in currentTarget.unitTargeted.adjacentUnits)
        {
            if (_unit == owner) adjacent = true;
        }

        if (adjacent)
        {
            owner.GetComponent<TacticsMovement>().FaceDirection(currentTarget.unitTargeted.gameObject.transform.position);
        }
        else
        {
            Initiative.queuedActions++;
            owner.MoveToTile(currentTarget.tileToAttackFrom, currentTarget.unitTargeted.currentTile.transform.position);
        }

        yield return new WaitUntil(() => !owner.moving);

        //See if any of the adjacent units to the target allow you to flank. 
        foreach (Unit unit in currentTarget.unitTargeted.adjacentUnits)
        {
            if (unit.unitInfo.faction != currentTarget.unitTargeted.unitInfo.faction)
            {
                Vector3 relTargetPosition = transform.InverseTransformPoint(currentTarget.unitTargeted.transform.position);
                Vector3 relOtherAttackerPosition = transform.InverseTransformPoint(unit.transform.position);
                if (relOtherAttackerPosition.z > (relTargetPosition.z + 0.1f))
                {
                    flanking = true;
                    break;
                }
            }
        }

        owner.unitAnim.SetTrigger("melee");

        yield break;
    }

    public override void AttackEvent()
    {
        int bonuses = 0;
        if (flanking) bonuses += 1;
        Result hit = AttackManager.AttackRoll(owner, currentTarget.unitTargeted.GetComponent<Unit>(), bonuses);
        MeleeAttackOutcome(hit);
    }

    public void MeleeAttackOutcome(Result result)
    {
        if (result == Result.SUCCESS)
        {
            AttackManager.DamageRoll(owner, currentTarget.unitTargeted.GetComponent<Unit>());
        }

        if (result == Result.PARTIAL)
        {
            AttackManager.DamageRoll(owner, currentTarget.unitTargeted.GetComponent<Unit>());
        }

        if (result == Result.FAIL)
        {
            DamagePopUp.Create(currentTarget.unitTargeted.gameObject.transform.position + new Vector3(0, currentTarget.unitTargeted.gameObject.GetComponent<TacticsMovement>().halfHeight), "miss", false);
        }

        Initiative.EndAction();
    }

    //This function is definitely gammy. 
    public override void GetTargets()
    {
        targets.Clear();        
        Tile tileToMeleeAttackFrom = null;
        //selectableTiles = owner.selectableTiles;

        //Go through each unit on the battlefield, get squares next to it, then work out which can be walked to. Pick one per unit. 
        foreach (TacticsMovement unit in Initiative.order)
        {
            if (unit != owner.GetComponent<TacticsMovement>())
            {
                unit.GetCurrentTile();
                //float maxDistance = Mathf.Infinity;
                bool targetFound = false;

                //This way of finding adjacents by distance is flawed, particularly if and when they are up or down. But kinda works.  
                if (Vector3.Distance(owner.currentTile.transform.position, unit.currentTile.transform.position) < 1.6f)
                {
                    AddTarget(unit, owner.currentTile);
                }

                else
                {
                    tileToMeleeAttackFrom = RangeFinder.FindTileNextToTarget(owner, unit);
                    if (tileToMeleeAttackFrom != null) targetFound = true;
                }
                    
                if (targetFound)
                {
                    AddTarget(unit, tileToMeleeAttackFrom);
                    targetFound = false;
                }
                unit.currentTile.adjacencyList.Clear();
                unit.currentTile.diagonalAdjacencyList.Clear();
            } 
        }
    }

    public override void AddTarget(TacticsMovement unit, Tile tileToAttackFrom)
    {
        Target target = new Target
        {
            unitTargeted = unit,
            tileToAttackFrom = tileToAttackFrom
        };
        targets.Add(target);
    }
}
