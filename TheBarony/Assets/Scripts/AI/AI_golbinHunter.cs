using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_golbinHunter : AI
{
    Task sneakyAttack = new t_sneakyMeleeAttack();
    Task run = new t_runAway();
    Task defaultTask = new DefaultTask();

    public override void SetTask()
    {
        sneakyAttack.EvaluateCandidates(unit);
        defaultTask.EvaluateCandidates(unit);

        foreach (Unit opponent in Initiative.players)
        {
            if (opponent.focus == unit)
            {
                run.EvaluateCandidates(unit);
            }
        }
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
