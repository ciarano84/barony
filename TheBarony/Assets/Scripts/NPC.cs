using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : TacticsMovement
{
    public GameObject destination;
    public AI ai;

    private void Start()
    {
        InitUnit();
        InitTacticsMovement();
    }

    private void FixedUpdate()
    {
        //if (!turn)
        if (Initiative.currentUnit != this)
        {
            if (focus != null)
            {
                if (RangeFinder.LineOfSight(this, focus) == true)
                {
                    FaceDirection(focus.transform.position);
                }
            }
        }
        else 
        {
            if (!moving && Initiative.queuedActions < 1)
            {
                destination = null;
                ai.DoTurn();

                //This section DOES the A* move, but once the tile has been decided. 
                if (remainingMove >= 1 && turn && destination != null)
                {
                    CalculatePath();

                    //Failsafe, in case of there being no path. 
                    if (actualTargetTile == null)
                    {
                        ai.task = new DefaultTask();
                        Debug.LogWarning("unit defaulted on task as no route found.");
                        return;
                    }
                        
                    NPCMove();
                    actualTargetTile.target = true;
                }
            }
            else
            {
                Move();
            }
        }    
    }
    
    void CalculatePath()
    {
        Tile targetTile = GetTargetTile(destination);
        actualTargetTile = FindPath(targetTile);
    }

    public void EndNPCTurn()
    {
        destination = null;
        ai.tasks.Clear();
        ai.task = null;
        Initiative.ForceTurnEnd(true);
    }
}
