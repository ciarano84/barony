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
             ai.DoTurn();

            //This section DOES the move, but once the tile has been decided. 
            if (remainingMove > 0 && turn)
            {
                CalculatePath();
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
        FindPath(targetTile);
    }

    //This currently only works for finding PCs, but should be expanded to get other destinations. 
    void FindNearestDestination()
    {
        List<GameObject> destinations = new List<GameObject>();

        //Add all the players to the list of destinations. 
        foreach (TacticsMovement c in Initiative.order)
        {
            if (c.unitInfo.faction == Factions.players)
            {
                destinations.Add(c.gameObject);
            }
        }

        //Here is where you need to add anything to filter or weight that list. 

        GameObject nearest = null;
        float distance = Mathf.Infinity;

        foreach (GameObject go in destinations)
        {
            float d = Vector3.Distance(transform.position, go.transform.position);

            if (d < distance)
            {
                distance = d;
                nearest = go;
            }
        }

        destination = nearest;
    }
}
