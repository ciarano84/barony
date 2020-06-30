using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeaponData : WeaponData
{
    
}

public class MeleeWeapon : Weapon
{
    List<Tile> selectableTiles = new List<Tile>();    //Target class to replace the dictionary, and associated list. 
    public MeleeWeaponData meleeWeaponData;

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
        owner.unitAnim.SetTrigger("melee");
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

        bool hit = AttackManager.AttackRoll(owner,target.unitTargeted.GetComponent<Unit>(), bonuses);

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

    //This function is definitely gammy. 
    public override void GetTargets()
    {
        targets.Clear();        
        Tile tileToMeleeAttackFrom = null;
        selectableTiles = owner.selectableTiles;

        //Go through each unit on the battlefield, get squares next to it, then work out which can be walked to. Pick one per unit. 
        foreach (TacticsMovement unit in Initiative.order)
        {
            if (unit != owner.GetComponent<TacticsMovement>())
            {
                unit.GetCurrentTile();
                float maxDistance = 200f;
                bool targetFound = false;

                //This way of finding adjacents by distance is flawed, particularly if and when they are up or down. But kinda works.  
                if (Vector3.Distance(owner.currentTile.transform.position, unit.currentTile.transform.position) < 1.6f)
                {
                    AddTarget(unit, owner.currentTile);
                }

                else
                {
                foreach (Tile tileNextToTarget in unit.currentTile.adjacencyList)
                    {

                        foreach (Tile tileCanBeWalkedTo in selectableTiles)
                        {
                            if (tileCanBeWalkedTo == tileNextToTarget)
                            {
                                targetFound = true;
                                if (Vector3.Distance(owner.transform.position, tileCanBeWalkedTo.transform.position) < maxDistance)
                                {
                                    maxDistance = Vector3.Distance(owner.transform.position, tileCanBeWalkedTo.transform.position);
                                    tileToMeleeAttackFrom = tileNextToTarget;
                                }
                            }
                        }
                    }

                    foreach (Tile tileNextToTarget in unit.currentTile.diagonalAdjacencyList)
                    {

                        foreach (Tile tileCanBeWalkedTo in selectableTiles)
                        {
                            if (tileCanBeWalkedTo == tileNextToTarget)
                            {
                                targetFound = true;
                                if (Vector3.Distance(owner.transform.position, tileCanBeWalkedTo.transform.position) < maxDistance)
                                {
                                    maxDistance = Vector3.Distance(owner.transform.position, tileCanBeWalkedTo.transform.position);
                                    tileToMeleeAttackFrom = tileNextToTarget;
                                }
                            }
                        }
                    }
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
