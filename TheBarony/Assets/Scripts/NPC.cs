﻿using System.Collections;
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
        if (!turn)
        {
            if (focus != null)
            {
                if (RangeFinder.LineOfSight(this, focus) == true)
                {
                    FaceDirection(focus.transform.position);
                }
            }
            return;
        }

        if (!moving && Initiative.queuedActions < 1)
        {
            //this next line could proper fuck everything. 
            destination = null;
            ai.DoTurn();

            //This section DOES the A* move, but once the tile has been decided. 
            if (remainingMove > 0 && turn && destination != null)
            {
                CalculatePath();
                NPCMove();
                actualTargetTile.target = true;
            }
        }
        else
        {
            Move();
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
        Initiative.EndTurn();
    }
}
