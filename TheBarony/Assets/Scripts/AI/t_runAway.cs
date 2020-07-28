using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class t_runAway : Task
{
    public override void EvaluateCandidates(NPC unit)
    {
        if (unit.focus != null)
        {
            Task task = new t_runAway();
            task.target = unit.focus;
            unit.GetComponent<AI>().tasks.Add(task);
        }
        else
        {
            foreach (Unit enemy in Initiative.players)
            {
                Task task = new t_runAway();
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
        //just runs.  
        if (unit.remainingMove > 0)
        {
            Initiative.queuedActions++;
            unit.MoveToTile(RangeFinder.FindTileFurthestFromOpponents(unit, unit.selectableTiles));
            flagEndofTurn = true;
        }
    }
}

