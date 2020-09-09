using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class t_defend : Task
{
    bool moved;

    public override void EvaluateCandidates(NPC unit, float weighting = 0)
    {
        Task task = new t_defend { tile = null, value = weighting };

        task.taskName = "defend";

        //am I being focused on. 
        foreach (Unit enemy in Initiative.players)
        {
            if (enemy.focus == unit) task.value += 0.2f;
        }

        //am I flagging?
        if (unit.unitInfo.flagging) task.value += 0.1f;

        //am I outnumbered?
        task.value += RangeFinder.HowOutnumberedAmI(unit) / 10;

        unit.GetComponent<AI>().tasks.Add(task);
    }

    public override void DoTask(NPC unit, Unit targetUnit = null, Tile targetTile = null)
    {
        //find a square to move to that isn't next to an enemy.  
        if (unit.remainingMove > 0 && moved == false)
        {
            unit.FindSelectableTiles();
            List<Tile> tiles = RangeFinder.FindTilesNotNextToEnemy(unit, unit.selectableTiles, Factions.players);
            int roll = Random.Range(0, tiles.Count);
            if (tiles[roll] != null)
            {
                Initiative.queuedActions++;
                unit.MoveToTile(tiles[roll]);
                moved = true;
                return;
            }
        }

        //and defend
        if (unit.remainingActions > 0)
        {
            unit.defend.ExecuteAction(ActionCost.main);
            flagEndofTurn = true;
        }
    }
}
