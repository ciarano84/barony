using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponData : WeaponData
{
    
    public override void SetWeaponData()
    {
        imageFile = "Shortsword";
        rangeType = Range.ranged;
    }

    public override void CreateWeapon(Unit unit)
    {
        MeleeWeapon weapon = unit.gameObject.AddComponent<MeleeWeapon>();
        weapon.owner = unit.gameObject.GetComponent<PlayerCharacter>();
        weapon.weaponData = this;
        unit.weapon1 = weapon;
    }
}

public class MeleeWeapon : Weapon
{
    List<Tile> selectableTiles = new List<Tile>();    //Target class to replace the dictionary, and associated list. 
    new public int actionsPerAttack = 2;

    public override IEnumerator Attack(Target target)
    {
        owner.MoveToTile(target.tileToAttackFrom, target.unitTargeted.currentTile.transform.position);

        yield return new WaitUntil(() => !owner.moving);
        
        owner.unitAnim.SetTrigger("melee");
        yield return new WaitForSeconds(0.3f);

        AttackManager.AttackRoll(owner,target.unitTargeted.GetComponent<Unit>());

        yield return new WaitForSeconds(2f);
        owner.remainingActions--;
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
