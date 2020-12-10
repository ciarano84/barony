using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_orcDefender : AI
{
    public t_SimpleMeleeAttack simpleMelee = new t_SimpleMeleeAttack();
    public t_defend defend = new t_defend();
    Task defaultTask = new DefaultTask();

    public override void SetTask()
    {
        simpleMelee.EvaluateCandidates(unit);
        defend.EvaluateCandidates(unit, -0.4f);
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
