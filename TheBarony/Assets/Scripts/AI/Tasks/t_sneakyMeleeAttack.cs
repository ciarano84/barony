using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class t_sneakyMeleeAttack : Task
{
    bool inFlankingPosition = false;
    bool firstMoveDone = false;
    
    public override void EvaluateCandidates(NPC unit, float weighting = 0)
    {
        foreach (Unit u in Initiative.players)
        {
            //check it can find a route. 

            unit.destination = u.gameObject;
            u.GetComponent<TacticsMovement>().GetCurrentTile();
            Tile t = unit.FindPath(u.GetComponent<TacticsMovement>().currentTile);
            unit.destination = null;
            attacked = false;
            if (t == null) return;

            Task task = new t_sneakyMeleeAttack();
            task.value = 1 / Vector3.Distance(unit.transform.position, u.transform.position);
            task.tile = t;
            task.target = u.GetComponent<Unit>();
            
            if (!RangeFinder.LineOfSight(unit, u.GetComponent<Unit>())) task.value -= 1;
            //Debug
            if (unit.focus == u) task.value += 10f;
            //if (unit.focus == u) task.value += 0.4f;
            if (u.focus == unit) task.value -= 0.3f;
            if (RangeFinder.FindFlankingTile(unit, unit.selectableTiles, u.GetComponent<TacticsMovement>()) != null) task.value += 0.2f;

            unit.GetComponent<AI>().tasks.Add(task);
        }
    }

    public override void DoTask(NPC unit, Unit targetUnit = null, Tile targetTile = null)
    {
        //Check to see if the target has been removed. If so, end turn. 
        if (target == null && attacked == true)
        {
            flagEndofTurn = true;
            return;
        }

        //if a flank is available, move to it. 
        if (!inFlankingPosition && !firstMoveDone)
        {
            unit.FindSelectableTiles();
            Tile flankingTile = RangeFinder.FindFlankingTile(unit, unit.selectableTiles, target.GetComponent<TacticsMovement>());
            if (flankingTile != null && unit.remainingMove > 0)
            {
                inFlankingPosition = true;
                Initiative.queuedActions++;
                unit.MoveToTile(flankingTile);
                firstMoveDone = true;
                return;
            }
        }

        //Move as close as possible if main action is available and you're not next to the target. 
        RangeFinder.FindAdjacentUnits(unit);
        if (!unit.adjacentUnits.Contains(target) && !firstMoveDone)
        {
            if (unit.remainingActions > 0 && unit.remainingMove > 0)
            {
                unit.destination = target.gameObject;
                firstMoveDone = true;
                return;
            }
        }

        //Attack if in the right position
        if (unit.remainingActions > 0)
        {
            RangeFinder.FindAdjacentUnits(unit);

            if (unit.adjacentUnits.Contains(target))
            {
                foreach (Weapon.Target t in unit.mainWeapon.targets)
                {
                    if (t.unitTargeted == target)
                    {
                        Initiative.queuedActions += 1;
                        unit.mainWeapon.StartCoroutine("Attack", t);
                        attacked = true;
                        return;
                    }
                }
            }
        }

        //Move away if you have move left
        if (unit.remainingMove >= 1)
        {
            unit.FindSelectableTiles();

            //A Hack to solve the bug 'characters teleport rather than move, and can share spaces'
            TacticsMovement targetT = target.GetComponent<TacticsMovement>();
            targetT.GetCurrentTile();
            if (unit.selectableTiles.Contains(targetT.currentTile))
            {
                unit.selectableTiles.Remove(targetT.currentTile);
            }

            //Debug
            //TacticsMovement targetT = target.GetComponent<TacticsMovement>();
            //targetT.GetCurrentTile();
            //if (unit.selectableTiles.Contains(targetT.currentTile))
            //{
            //    Debug.LogError("enemy's tile has shown up in selectable tile list");
            //    unit.selectableTiles.Remove(targetT.currentTile);
            //}

            List<Tile> preferedTiles = RangeFinder.FindTilesNotNextToEnemy(unit, unit.selectableTiles, Factions.players);
            if (preferedTiles.Count > 0)
            {
                Initiative.queuedActions++;
                unit.MoveToTile(preferedTiles[Random.Range(0, preferedTiles.Count)]);
                flagEndofTurn = true;
                return;
            }
        }

        //Defend if you've not reached the target
        if (unit.remainingActions > 0)
        {
            unit.defend.ExecuteAction(ActionCost.main);
            return;
        }

        flagEndofTurn = true;
    }
}
