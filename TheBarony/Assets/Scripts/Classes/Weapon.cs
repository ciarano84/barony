using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    List<Tile> selectableTiles = new List<Tile>();
    List<TacticsMovement> meleeTargets = new List<TacticsMovement>();
    public PlayerCharacter owner;
    int attackModifier = 0;
    int damageModifier = 2;

    private void Start()
    {
        owner = GetComponent<PlayerCharacter>();
        attackModifier += owner.attackModifier;
        damageModifier += owner.damageModifier;
    }

    public IEnumerator Attack()
    {
        owner.unitAnim.SetTrigger("melee");
        owner.remainingActions--;
        yield return new WaitForSeconds(0.3f);

        //proxy attack vs 10 in here.
        AbilityChecker.CheckAbility(attackModifier, 10);

        yield return new WaitForSeconds(1f);
        Initiative.CheckForTurnEnd(owner);
        yield break;
    }

    public List<TacticsMovement> GetTargets()
    {
        selectableTiles = GetComponent<TacticsMovement>().selectableTiles;
        List<TacticsMovement> units = Initiative.sortedUnits;
        List<TacticsMovement> unitsInMeleeReach = new List<TacticsMovement>();

        //Go through each unit on the battlefield, get squares next to it, then work out which can be walked to. Pick one per unit. 
        foreach (TacticsMovement unit in units)
        {
            if (unit != owner.GetComponent<TacticsMovement>())
            {
                unit.GetCurrentTile();
                unit.currentTile.FindNeighbours(owner.jumpHeight);

                foreach (Tile tileNextToTarget in unit.currentTile.adjacencyList)
                {
                    foreach (Tile tileCanBeWalkedTo in selectableTiles)
                    {
                        if (tileCanBeWalkedTo == tileNextToTarget)
                        {
                            unitsInMeleeReach.Add(unit);
                        }
                    }
                }
            }
        }
        return unitsInMeleeReach;
    }





    /* code for picking the closest square. 
                    //Set the initial distance with the first tile. 
                float distance = -1;
                if (distance < 0)
                {
                    distance = Vector3.Distance(owner.transform.position,t.transform.position);
                    tileToMeleeAttackFrom = t;
                }
                
                //Compare subsequent tiles against it. 
                if (Vector3.Distance(owner.transform.position, t.transform.position) < distance)
                { 

                }
    */
}
