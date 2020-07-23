using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    NPC unit;

    public Unit targetUnit;
    public t_SimpleMeleeAttack basicTask = new t_SimpleMeleeAttack();
    public List<Task> tasks = new List<Task>();
    public Task task;

    private void Awake()
    {
        unit = this.gameObject.GetComponent<NPC>();
    }

    //Go through each task type. 
    //Evaluate all the places that could be carried out. 
    //Add random weighting to them.  
    //Pick the winner. 
    //Defer to that task script to DO the action. 

    public void DoTurn()
    {
        if (task == null)
        {
            Debug.LogWarning("task is null");
        }
        
        task.DoTask(unit);
    }

    void RandomizeValues()
    {
        foreach (Task t in tasks)
        {
            t.value += UnityEngine.Random.Range(0, 6) / 10;
        }
    }

    public void SetTask()
    {
        basicTask.EvaluateCandidates(unit);
        RandomizeValues();
        unit.actualTargetTile = null;

        //Pick the Winner;
        float highestValue = -100f;
        foreach (Task t in tasks)
        {
            if (t.value > highestValue)
            {
                task = t;
                highestValue = t.value;
            }
        }
    }
}

public abstract class Task
{
    public float value;
    public Tile tile;
    public Unit target;

    public abstract void EvaluateCandidates(NPC unit);
    
    public abstract void DoTask(NPC unit, Unit targetUnit = null, Tile targetTile = null);
}

public class t_SimpleMeleeAttack : Task
{
    public override void EvaluateCandidates(NPC unit)
    {
        if (unit.focus != null)
        {
            Task task = new t_SimpleMeleeAttack();
            task.target = unit.focus;
            unit.GetComponent<AI>().tasks.Add(task);
        }
        else 
        {
            List<GameObject> enemies = new List<GameObject>();
            foreach (TacticsMovement c in Initiative.order)
            {
                if (c.unitInfo.faction == Factions.players)
                {
                    enemies.Add(c.gameObject);
                }
            }

            foreach (GameObject enemy in enemies)
            {
                //check it can find a route. 
                unit.destination = enemy;
                enemy.GetComponent<TacticsMovement>().GetCurrentTile();
                Tile t = unit.FindPath(enemy.GetComponent<TacticsMovement>().currentTile);
                if (t == null) return;
                unit.destination = null;

                Task task = new t_SimpleMeleeAttack();
                task.value = 1 / Vector3.Distance(unit.transform.position, enemy.transform.position);
                task.tile = t;
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
        unit.destination = target.gameObject;

        //after move is set, the attack is carried out, when possible. 
        if (unit.remainingActions > 0)
        {
            unit.FindAdjacentUnits();
            if (target != null)
            {
                if (unit.adjacentUnits.Contains(target))
                //if (unit.adjacentUnits.Contains(unit.GetComponent<AI>().targetUnit))
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
        }
        unit.GetCurrentTile();
        if (unit.currentTile == unit.actualTargetTile)
        {
            unit.EndNPCTurn();
        }
    }
}
