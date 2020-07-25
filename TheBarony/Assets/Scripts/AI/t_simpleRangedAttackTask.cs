using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class t_simpleRangedAttackTask : Task
{
    public override void EvaluateCandidates(NPC unit)
    {
        if (unit.focus != null)
        {
            Task task = new t_simpleRangedAttackTask();
            task.target = unit.focus;
            unit.GetComponent<AI>().tasks.Add(task);
        }
        else
        {
            foreach (Unit enemy in Initiative.players)
            {
                Task task = new t_simpleRangedAttackTask();
                task.value = 1 / Vector3.Distance(unit.transform.position, enemy.transform.position);
                task.tile = null;
                task.target = enemy.GetComponent<Unit>();
                if (!RangeFinder.LineOfSight(unit, enemy.GetComponent<Unit>()))
                {
                    task.value -= 1;
                }
                unit.GetComponent<AI>().tasks.Add(task);
            }
        }
    }

    public override void DoTask(NPC unit, Unit targetUnit = null, Tile targetTile = null)
    {
        //check to see if an enemy is adjacent and then move. 
        if (unit.remainingActions > 0)
        {
            RangeFinder.FindAdjacentUnits(unit);
            foreach (Unit u in unit.adjacentUnits)
            {
                if (u.unitInfo.faction != unit.unitInfo.faction)
                {
                    List<Tile> tiles = RangeFinder.FindTilesWithLineOfSight(unit, unit.selectableTiles, target.GetComponent<TacticsMovement>());
                    Initiative.queuedActions++;
                    unit.MoveToTile(tiles[Random.Range(0, tiles.Count)]);
                }
            }
        }
    }
}
