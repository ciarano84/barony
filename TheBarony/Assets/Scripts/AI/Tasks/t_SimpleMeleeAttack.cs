﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class t_SimpleMeleeAttack : Task
{
    bool movedCloser = false;

    public override void EvaluateCandidates(NPC unit, float weighting = 0)
    {
        if (unit.focus != null)
        {
            Task task = new t_SimpleMeleeAttack();
            task.taskName = "simple melee";
            task.target = unit.focus;
            unit.GetComponent<AI>().tasks.Add(task);
        }
        else
        {
            foreach (Unit target in Initiative.players)
            {
                //check it can find a route. 
                unit.destination = target.gameObject;
                target.GetComponent<TacticsMovement>().GetCurrentTile();
                Tile t = unit.FindPath(target.GetComponent<TacticsMovement>().currentTile);
                unit.destination = null;
                if (t == null) return;

                Task task = new t_SimpleMeleeAttack();
                task.taskName = "simple melee";
                task.value = 1 / Vector3.Distance(unit.transform.position, target.transform.position);
                task.tile = t;
                task.target = target.GetComponent<Unit>();
                unit.GetComponent<AI>().tasks.Add(task);
            }
        }
    }

    public override void DoTask(NPC unit, Unit targetUnit = null, Tile targetTile = null)
    {
        //Attack the target if possible. 
        if (unit.remainingActions > 0)
        {
            RangeFinder.FindAdjacentUnits(unit);
            if (target != null)
            {
                if (unit.adjacentUnits.Contains(target))
                {
                    foreach (Weapon.Target t in unit.mainWeapon.targets)
                    {
                        if (t.unitTargeted == target)
                        {
                            Initiative.queuedActions++;
                            CombatLog.UpdateCombatLog(unit.name + " melee attacks " + target.name);
                            unit.mainWeapon.StartCoroutine("Attack", t);
                            flagEndofTurn = true;
                            return;
                        }
                    }
                }
            }
        }

        //Move closer to the target if needed. 
        if (unit.remainingMove > 0 && unit.remainingActions > 0)
        {
            if (!movedCloser)
            {
                unit.destination = target.gameObject;
                CombatLog.UpdateCombatLog(unit.name + " A* toward opposing faction.");
                movedCloser = true;
                return;
            }
        }

        //Defend if you've not reached the target
        if (unit.remainingActions > 0)
        {
            CombatLog.UpdateCombatLog(unit.name + " defends.");
            unit.defend.ExecuteAction(ActionCost.main);
            return;
        }

        flagEndofTurn = true;
    }
}
