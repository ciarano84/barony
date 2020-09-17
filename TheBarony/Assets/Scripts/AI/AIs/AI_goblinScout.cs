using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_goblinScout : AI
{
    Task simpleRangedAttack = new t_simpleRangedAttackTask();
    Task run = new t_runAway();
    Task defaultTask = new DefaultTask();

    public override void SetTask()
    {
        simpleRangedAttack.EvaluateCandidates(unit);
        defaultTask.EvaluateCandidates(unit);

        foreach (Unit opponent in Initiative.players)
        {
            if (opponent.focus == unit)
            {
                run.EvaluateCandidates(unit, -0.1f);
            }
        }
        RandomizeValues();

        unit.actualTargetTile = null;

        //Debug.Log(unit.name + " " + unit.gameObject.GetInstanceID());
        //Pick the Winner;
        float highestValue = -100f;
        foreach (Task t in tasks)
        {
            //Debug.Log(t.taskName + " " + t.value);
            if (t.value > highestValue)
            {
                task = t;
                highestValue = t.value;
            }
        }
    }
}
