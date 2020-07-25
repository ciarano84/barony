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
        RangedWeapon weapon = unit.GetComponent<RangedWeapon>();

        //check to see if an enemy is adjacent and then move. 
        if (unit.remainingMove > 0)
        {
            RangeFinder.FindAdjacentUnits(unit);
            foreach (Unit u in unit.adjacentUnits)
            {
                if (u.unitInfo.faction != unit.unitInfo.faction)
                {
                    List<Tile> tiles = RangeFinder.FindTilesWithLineOfSight(unit, unit.selectableTiles, target.GetComponent<TacticsMovement>());
                    tiles = RangeFinder.FindTilesNotNextToEnemy(unit, tiles, target.GetComponent<TacticsMovement>());
                    if (tiles.Count > 0)
                    {
                        Initiative.queuedActions++;
                        unit.MoveToTile(tiles[Random.Range(0, tiles.Count)]);
                        return;
                    }
                }
            }
        }

        if (weapon.rangedWeaponData.currentAmmo == 0)
        {
            if (weapon.rangedWeaponData.reloadSpeed == ActionCost.move && (unit.remainingMove == unit.unitInfo.currentMove))
            {
                weapon.Reload(ActionCost.move);
                return;
            }
            else if (unit.remainingActions > 0)
            {
                weapon.Reload(ActionCost.main);
                return;
            }
            else
            {
                if (unit.remainingMove > 0)
                {
                    Initiative.queuedActions++;
                    unit.MoveToTile(RangeFinder.FindTileFurthestFromOpponents(unit, unit.selectableTiles));
                    return;
                }
            }
        }

        if (unit.focus == null)
        {
            
        }
    }
}
