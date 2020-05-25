using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    List<Tile> selectableTiles = new List<Tile>();
    public PlayerCharacter owner;
    int attackModifier = 0;
    int damageModifier = 2;

    //Target class to replace the dictionary, and associated list. 
    public class Target 
    {
        public TacticsMovement unitTargeted;
        public Tile tileToAttackFrom;   
    }

    public List<Target> targets = new List<Target>();

    private void Start()
    {
        owner = GetComponent<PlayerCharacter>();
        attackModifier += owner.attackModifier;
        damageModifier += owner.damageModifier;
    }

    public IEnumerator MeleeAttack(Weapon.Target target)
    {
        //Move script goes here.
        owner.MoveToTile(target.tileToAttackFrom, target.unitTargeted.currentTile.transform.position);

        yield return new WaitUntil(() => !owner.moving);
        
        owner.unitAnim.SetTrigger("melee");
        yield return new WaitForSeconds(0.3f);

        //proxy attack vs 10 in here.
        AttackManager.AttackRoll(this.GetComponent<Unit>(),target.unitTargeted.GetComponent<Unit>());

        yield return new WaitForSeconds(1f);
        owner.remainingActions--;
        Initiative.CheckForTurnEnd();
        yield break;
    }

    //This function is definitely gammy. 
    public void GetTargets()
    {
        targets.Clear();        
        Tile tileToMeleeAttackFrom = null;
        selectableTiles = GetComponent<TacticsMovement>().selectableTiles;
        //List<TacticsMovement> units = Initiative.sortedUnits;

        //Go through each unit on the battlefield, get squares next to it, then work out which can be walked to. Pick one per unit. 
        foreach (TacticsMovement unit in Initiative.order)
        {
            if (unit != owner.GetComponent<TacticsMovement>())
            {
                unit.GetCurrentTile();
                float maxDistance = 200f;
                bool targetFound = false;

                //This way of finding adjacents by distance is flawed, particularly if and when they are up or down. But kinda works. 
                if (Vector3.Distance(owner.currentTile.transform.position, unit.currentTile.transform.position) < 1.3f)
                {
                    AddTarget(unit, owner.currentTile);
                } 
                
                else foreach (Tile tileNextToTarget in unit.currentTile.adjacencyList)
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
                if (targetFound)
                {
                    AddTarget(unit, tileToMeleeAttackFrom);
                    targetFound = false;
                }
                unit.currentTile.adjacencyList.Clear();
            } 
        }
    }

    void AddTarget(TacticsMovement unit, Tile tileToAttackFrom)
    {
        Target target = new Target();
        target.unitTargeted = unit;
        target.tileToAttackFrom = tileToAttackFrom;
        targets.Add(target);
    }
}
