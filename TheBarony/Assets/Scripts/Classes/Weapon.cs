using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //likely will need someway to clear the dictionary in here. 
    
    List<Tile> selectableTiles = new List<Tile>();
    //cos I have this
    public Dictionary<TacticsMovement, Tile> unitsInMeleeReach = new Dictionary<TacticsMovement, Tile>();
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

    public IEnumerator Attack(TacticsMovement target, Tile tile)
    {
        //Move script goes here.
        owner.MoveToTile(tile);

        //Get heading to face target. 
        
        
        
        owner.unitAnim.SetTrigger("melee");
        owner.remainingActions--;
        yield return new WaitForSeconds(0.3f);

        //proxy attack vs 10 in here.
        AbilityChecker.CheckAbility(attackModifier, 10);

        yield return new WaitForSeconds(1f);
        Initiative.CheckForTurnEnd(owner);
        yield break;
    }

    public void GetTargets()
    {
        Tile tileToMeleeAttackFrom = null;
        selectableTiles.Clear();

        //This will need removing
        unitsInMeleeReach.Clear();
        
        selectableTiles = GetComponent<TacticsMovement>().selectableTiles;
        List<TacticsMovement> units = Initiative.sortedUnits;

        //Go through each unit on the battlefield, get squares next to it, then work out which can be walked to. Pick one per unit. 
        foreach (TacticsMovement unit in units)
        {
            if (unit != owner.GetComponent<TacticsMovement>())
            {
                unit.GetCurrentTile();

                foreach (Tile tileNextToTarget in unit.currentTile.adjacencyList)
                {
                    foreach (Tile tileCanBeWalkedTo in selectableTiles)
                    {
                        if (tileCanBeWalkedTo == tileNextToTarget)
                        {
                            if (tileToMeleeAttackFrom == null) tileToMeleeAttackFrom = tileCanBeWalkedTo;
                            //Compare against the initial tile. 
                            else if (Vector3.Distance(owner.transform.position, tileCanBeWalkedTo.transform.position) < Vector3.Distance(owner.transform.position, tileToMeleeAttackFrom.transform.position))
                            {
                                tileToMeleeAttackFrom = tileCanBeWalkedTo;
                            }
                            Target target = new Target();
                            target.unitTargeted = unit;
                            target.tileToAttackFrom = tileToMeleeAttackFrom;

                            targets.Add(target);
                        }
                    }
                }
            } 
        }
    }
}
