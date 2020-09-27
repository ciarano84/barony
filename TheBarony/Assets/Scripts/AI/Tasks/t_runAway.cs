using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class t_runAway : Task
{
    bool dashed;
    
    public override void EvaluateCandidates(NPC unit, float weighting = 0)
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

                //am I outnumbered?
                task.value += RangeFinder.HowOutnumberedAmI(unit) / 10;

                unit.GetComponent<AI>().tasks.Add(task);
            }
        }
    }

    public override void DoTask(NPC unit, Unit targetUnit = null, Tile targetTile = null)
    {
        //Dash if that's needed
        if (unit.remainingActions > 0 && dashed == false)
        {
            unit.dash.ExecuteAction(ActionCost.main);
            dashed = true;
            return;
        }

        //Then run 
        if (unit.remainingMove > 0)
        {
            unit.FindSelectableTiles();
            Initiative.queuedActions++;
            unit.MoveToTile(RangeFinder.FindTileFurthestFromOpponents(unit, unit.selectableTiles));
            flagEndofTurn = true;
        }
    }
}

