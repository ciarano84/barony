using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_orcBrute : AI
{
    public t_aggressiveMeleeAttack aggressiveMelee = new t_aggressiveMeleeAttack();
    Task run = new t_runAway();
    Task defaultTask = new DefaultTask();

    public override void SetTask()
    {
        aggressiveMelee.EvaluateCandidates(unit);
        defaultTask.EvaluateCandidates(unit);

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
