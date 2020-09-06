using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class t_aggressiveMeleeAttack : Task
{
    bool dashed = false;
    
    public override void EvaluateCandidates(NPC unit)
    {
        if (unit.focus != null)
        {
            Task task = new t_aggressiveMeleeAttack();
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

                Task task = new t_aggressiveMeleeAttack();
                task.value = 1 / Vector3.Distance(unit.transform.position, target.transform.position);
                task.tile = t;
                task.target = target.GetComponent<Unit>();
                if (!RangeFinder.LineOfSight(unit, target.GetComponent<Unit>()))
                {
                    task.value -= 1;
                }
                unit.GetComponent<AI>().tasks.Add(task);
            }
        }
    }

    public override void DoTask(NPC unit, Unit targetUnit = null, Tile targetTile = null)
    {
        //Attack the target if possible. 
        if (unit.remainingActions > 0)
        {
            //unit.FindAdjacentUnits();
            RangeFinder.FindAdjacentUnits(unit);
            if (target != null)
            {
                if (unit.adjacentUnits.Contains(target))
                {
                    foreach (Weapon.Target t in unit.mainWeapon.targets)
                    {
                        if (t.unitTargeted == target)
                        {
                            Initiative.queuedActions += 1;
                            unit.mainWeapon.StartCoroutine("Attack", t);
                            flagEndofTurn = true;
                            return;
                        }
                    }
                }
            }
        }

        //Dash if that's needed
        Tile tileToAttackFrom = RangeFinder.FindTileNextToTarget(unit, target.GetComponent<TacticsMovement>());
        if (tileToAttackFrom == null)
        {
            if (unit.remainingActions > 0)
            {
                unit.dash.ExecuteAction(ActionCost.main);
                dashed = true;
            }
        }

        //Move closer to the target if needed. 
        if ((unit.remainingMove > 0 && unit.remainingActions > 0) || dashed == true)
        {
            unit.destination = target.gameObject;
            dashed = false;
        }
        else
        {
            flagEndofTurn = true;
        }
    }
}
