using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public NPC unit;

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

        if (task.flagEndofTurn == true)
        {
            task.flagEndofTurn = false;
            unit.EndNPCTurn();
            return;
        }
        task.DoTask(unit);
    }

    public void RandomizeValues()
    {
        foreach (Task t in tasks)
        {
            t.value += UnityEngine.Random.Range(0, 6) / 10;
        }
    }

    public virtual void SetTask()
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
    public bool flagEndofTurn = false;
    public bool attacked = false;

    public abstract void EvaluateCandidates(NPC unit);
    
    public abstract void DoTask(NPC unit, Unit targetUnit = null, Tile targetTile = null);
}

public class DefaultTask : Task
{
    public override void EvaluateCandidates(NPC unit)
    {
        Task task = new DefaultTask();
        task.value = -10;
        unit.GetComponent<AI>().tasks.Add(task);
    }

    public override void DoTask(NPC unit, Unit targetUnit = null, Tile targetTile = null)
    {
        Debug.LogWarning(unit.name + " has nothing to do this turn");
        flagEndofTurn = true;
    }
}