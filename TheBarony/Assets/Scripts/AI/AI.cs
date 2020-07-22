using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    NPC unit;
    public Unit targetUnit;
    public t_SimpleMeleeAttack basicTask = new t_SimpleMeleeAttack();

    private void Awake()
    {
        unit = this.gameObject.GetComponent<NPC>();
    }

    //Go through each task type. 
    //Evaluate all the places that could be carried out. 
    //Add random weighting to them.  
    //Pick the winner. 
    //Defer to that task script to DO the action. 

    //As a proxy for now, let's shortcut to just kicking off the simpleMelee Task.
    public void DoTurn()
    {
        basicTask.DoTask(unit);
    }
}

public abstract class Task
{
    public float value;

    public abstract void DoTask(NPC unit);
}

public class t_SimpleMeleeAttack : Task
{
    public override void DoTask(NPC unit)
    {
        if (unit.destination == null)
        {
            if (unit.focus != null)
            {
                unit.GetComponent<AI>().targetUnit = unit.focus;
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

                unit.GetComponent<AI>().targetUnit = RangeFinder.FindNearestDestination(unit.gameObject, enemies).GetComponent<Unit>();
            }
            unit.destination = unit.GetComponent<AI>().targetUnit.gameObject;
            return;
        }

        //after move is set, the attack is carried out, when possible. 
        if (unit.remainingActions > 0)
        {
            unit.FindAdjacentUnits();
            if (unit.adjacentUnits.Contains(unit.GetComponent<AI>().targetUnit))
            {
                foreach (Weapon.Target t in unit.mainWeapon.targets)
                {
                    if (t.unitTargeted == unit.GetComponent<AI>().targetUnit)
                    {
                        Initiative.queuedActions += 1;
                        unit.mainWeapon.StartCoroutine("Attack", t);
                        return;
                    }
                }
            }
        }
        unit.GetCurrentTile();
        if (unit.currentTile == unit.actualTargetTile)
        {
            unit.destination = null;
            Initiative.EndTurn();
        }
    }
}
