using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_goblinScout : AI
{
    Task goblinTask = new t_simpleRangedAttackTask();

    public override void SetTask()
    {
        goblinTask.EvaluateCandidates(unit);
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
