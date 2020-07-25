using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class t_sneakyMeleeAttack : Task
{
    public override void EvaluateCandidates(NPC unit)
    {
        foreach (Unit target in Initiative.players)
        {
            //check it can find a route. 

            unit.destination = target.gameObject;
            target.GetComponent<TacticsMovement>().GetCurrentTile();
            Tile t = unit.FindPath(target.GetComponent<TacticsMovement>().currentTile);
            unit.destination = null;
            if (t == null) return;

            Task task = new t_sneakyMeleeAttack();
            task.value = 1 / Vector3.Distance(unit.transform.position, target.transform.position);
            task.tile = t;
            task.target = target.GetComponent<Unit>();
            
            if (!RangeFinder.LineOfSight(unit, target.GetComponent<Unit>())) task.value -= 1;
            if (unit.focus == target) task.value += 0.2f;
            if (target.focus == unit) task.value -= 0.3f;
            if (RangeFinder.FindFlankingTile(unit, unit.selectableTiles, target.GetComponent<TacticsMovement>()) != null) task.value += 0.2f;

            unit.GetComponent<AI>().tasks.Add(task);
        }
    }

    public override void DoTask(NPC unit, Unit targetUnit = null, Tile targetTile = null)
    {
        //if a flank is available, move to it. 
        Tile flankingTile = RangeFinder.FindFlankingTile(unit, unit.selectableTiles, target.GetComponent<TacticsMovement>());
        if (flankingTile != null && unit.remainingMove > 0)
        {
            Initiative.queuedActions++;
            unit.MoveToTile(flankingTile);
            return;
        }

        //Move as close as possible if main action is available and you're not next to the target. 
        RangeFinder.FindAdjacentUnits(unit);
        if (!unit.adjacentUnits.Contains(target))
        {
            if (unit.remainingActions > 0 && unit.remainingMove > 0)
            {
                unit.destination = target.gameObject;
                return;
            }
        }

        //Attack if in the right position
        if (unit.remainingActions > 0)
        {
            if (unit.adjacentUnits.Contains(target))
            {
                foreach (Weapon.Target t in unit.mainWeapon.targets)
                {
                    if (t.unitTargeted == target)
                    {
                        Initiative.queuedActions += 1;
                        unit.mainWeapon.StartCoroutine("Attack", t);
                        return;
                    }
                }
            }
        }

        //Move away if you have move left
        if (unit.remainingMove > 0)
        {
            unit.FindSelectableTiles();
            List<Tile> preferedTiles = RangeFinder.FindTilesNotNextToEnemy(unit, unit.selectableTiles, target.GetComponent<TacticsMovement>());
            if (preferedTiles.Count > 0)
            {
                Initiative.queuedActions++;
                unit.MoveToTile(preferedTiles[Random.Range(0, preferedTiles.Count)]);
                return;
            }
        }

        unit.EndNPCTurn();
    }
}
