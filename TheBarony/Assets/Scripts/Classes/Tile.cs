using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    public bool diagonal = false;

    public List<Tile> adjacencyList = new List<Tile>();
    public List<Tile> diagonalAdjacencyList = new List<Tile>();

    //Required for Breadth first search
    public bool visited = false;
    public Tile parent = null;
    public float distance = 0;

    //Debug
    public bool foundABarrier = false;

    //diagonals
    Vector3 forwardAndLeft = new Vector3(-1, 0, 1);
    Vector3 forwardAndRight = new Vector3(1, 0, 1);
    Vector3 backAndLeft = new Vector3(-1, 0, -1);
    Vector3 backAndRight = new Vector3(1, 0, -1);

    void Update()
    {
        if (current)
        {
            GetComponent<Renderer>().material.color = Color.magenta;
        }
        else if (target)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else if (selectable)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
    }

    public void Reset()
    {
        adjacencyList.Clear();
        diagonalAdjacencyList.Clear();

        current = false;
        target = false;
        selectable = false;
        visited = false;
        diagonal = false;
        parent = null;
        distance = 0;
    }

    public void FindNeighbours(float jumpHeight)
    {
        Reset();

        CheckTile(Vector3.forward, jumpHeight);
        CheckTile(-Vector3.forward, jumpHeight);
        CheckTile(Vector3.right, jumpHeight);
        CheckTile(-Vector3.right, jumpHeight);

        //Diagonals
        CheckTile(forwardAndLeft, jumpHeight, true);
        CheckTile(forwardAndRight, jumpHeight, true);
        CheckTile(backAndLeft, jumpHeight, true);
        CheckTile(backAndRight, jumpHeight, true);
    }

    public void CheckTile(Vector3 direction, float jumpHeight) {

        //Barrier check overlapbox
        Vector3 barrierCheckRange = new Vector3(0.25f, 0.25f, 0.25f);
        Collider[] barrierCheckColliders = Physics.OverlapBox(transform.position + (direction/2), barrierCheckRange);

        foreach (Collider boundary in barrierCheckColliders)
        {
            if (boundary.gameObject.tag == "barrier")
            {
                foundABarrier = true;
                return;
            }
        }

        Vector3 halfExtents = new Vector3(0.25f, (1 + jumpHeight) / 2f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider item in colliders) 
        {   
            Tile tile = item.GetComponent<Tile>();
            if (tile != null && tile.walkable)
            {
                RaycastHit hit;
                if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1))
                {
                    adjacencyList.Add(tile);
                }
            }
        }
    }

    public void CheckTile(Vector3 direction, float jumpHeight, bool diagonal)
    {
        //Barrier check overlapbox
        Vector3 barrierCheckRange = new Vector3(0.25f, 0.25f, 0.25f);
        Collider[] barrierCheckColliders = Physics.OverlapBox(transform.position + (direction / 2), barrierCheckRange);

        foreach (Collider boundary in barrierCheckColliders)
        {
            if (boundary.gameObject.tag == "barrier")
            {
                foundABarrier = true;
                return;
            }
        }

        Vector3 halfExtents = new Vector3(0.25f, (1 + jumpHeight) / 2f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            if (tile != null && tile.walkable)
            {
                RaycastHit hit;
                if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1))
                {
                    diagonalAdjacencyList.Add(tile);
                }
            }
        }
    }
}
