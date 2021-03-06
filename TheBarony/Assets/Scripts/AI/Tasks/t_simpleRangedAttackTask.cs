﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class t_simpleRangedAttackTask : Task
{
    public override void EvaluateCandidates(NPC unit, float weighting = 0)
    {
        Debug.Log("evaluating candidates.");
        if (unit.focus != null)
        {
            Debug.Log("goblin has a focus.");
            Task task = new t_simpleRangedAttackTask();
            task.taskName = "simple ranged";
            task.target = unit.focus;
            unit.GetComponent<AI>().tasks.Add(task);
        }
        else
        {
            foreach (Unit enemy in Initiative.players)
            {
                Task task = new t_simpleRangedAttackTask();
                task.taskName = "simple ranged";
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
        RangedWeapon weapon = unit.GetComponent<RangedWeapon>();

        //Check to see if you've killed the enemy. 
        if (target == null)
        {
            flagEndofTurn = true;
            return;
        }

        //check to see if an enemy is adjacent and then move. 
        if (unit.remainingMove > 0)
        {
            RangeFinder.FindAdjacentUnits(unit);
            foreach (Unit u in unit.adjacentUnits)
            {
                if (u.unitInfo.faction != unit.unitInfo.faction)
                {
                    List<Tile> tiles = RangeFinder.FindTilesWithLineOfSight(unit, unit.selectableTiles, target.GetComponent<TacticsMovement>());
                    tiles = RangeFinder.FindTilesNotNextToEnemy(unit, tiles, Factions.players);
                    if (tiles.Count > 0)
                    {
                        Initiative.queuedActions++;
                        CombatLog.UpdateCombatLog(unit.name + " moves away to avoid ranged hindrance.");
                        unit.MoveToTile(tiles[Random.Range(0, tiles.Count)]);
                        return;
                    }
                }
            }
        }

        //Reload if required.
        if (weapon.rangedWeaponData.currentAmmo == 0)
        {
            if (weapon.rangedWeaponData.reloadSpeed == ActionCost.move && (unit.remainingMove == unit.unitInfo.currentMove))
            {
                weapon.Reload(ActionCost.move);
                CombatLog.UpdateCombatLog(unit.name + " reloads as a move action.");
                flagEndofTurn = true;
                return;
            }
            else if (unit.remainingActions > 0)
            {
                weapon.Reload(ActionCost.main);
                CombatLog.UpdateCombatLog(unit.name + " reloads as a main action.");
                flagEndofTurn = true;
                return;
            }
            else
            {
                if (unit.remainingMove > 0)
                {
                    Initiative.queuedActions++;
                    CombatLog.UpdateCombatLog(unit.name + " moves as far from opposing faction as possible.");
                    unit.MoveToTile(RangeFinder.FindTileFurthestFromOpponents(unit, unit.selectableTiles));
                    return;
                }
            }
        }

        //If you've not got line of sight, get it.
        if (!RangeFinder.LineOfSight(unit, target) && unit.remainingMove >= 1)
        {
            //Find a tile to move to that has LoS, ideally one that isn't next to an enemy.
            unit.FindSelectableTiles();
            List<Tile> tiles = RangeFinder.FindTilesWithLineOfSight(unit, unit.selectableTiles, target.GetComponent<TacticsMovement>());
            if (tiles.Count > 0)
            {
                List<Tile> preferedTiles = RangeFinder.FindTilesNotNextToEnemy(unit, tiles, Factions.players);
                Initiative.queuedActions++;
                CombatLog.UpdateCombatLog(unit.name + " moves to get line of sight.");
                if (preferedTiles.Count > 0) unit.MoveToTile(tiles[Random.Range(0, preferedTiles.Count)]);
                else unit.MoveToTile(tiles[Random.Range(0, tiles.Count)]);
            }
            else
            {
                //if you can't get to a place you can see from, A* toward the target. 
                unit.destination = target.gameObject;
                CombatLog.UpdateCombatLog(unit.name + " A* toward opposing faction.");
            }
            return;
        }

        //If you've not got focus, get it. 
        if (unit.focus == null)
        {
            if (RangeFinder.LineOfSight(unit, target))
            {
                unit.SetFocus(target);
                flagEndofTurn = true;
                CombatLog.UpdateCombatLog(unit.name + " focuses on opposing faction.");
                return;
            }
        }

        //shoot
        if (RangeFinder.LineOfSight(unit, target) && unit.remainingActions > 0 && weapon.rangedWeaponData.currentAmmo > 0 && target == unit.focus)
        {
            Debug.Log("goes to attack");
            foreach (Weapon.Target t in unit.mainWeapon.targets)
            {
                if (t.unitTargeted == target)
                {
                    Initiative.queuedActions += 1;
                    unit.mainWeapon.StartCoroutine("Attack", t);
                    CombatLog.UpdateCombatLog(unit.name + " shoots at " + target.name);
                    flagEndofTurn = true;
                    return;
                }
            }
        }

        //and defend if able.
        if (unit.remainingActions > 0)
        {
            unit.defend.ExecuteAction(ActionCost.main);
            CombatLog.UpdateCombatLog(unit.name + " defends");
            flagEndofTurn = true;
            return;
        }

        flagEndofTurn = true;
    }
}
