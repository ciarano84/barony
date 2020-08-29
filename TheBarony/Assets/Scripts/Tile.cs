﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    public bool diagonal = false;
    public bool difficultTerrain = false;
    public GameObject selectPlane;

    public Neighbour[] neighbours = new Neighbour[8];
    public Unit occupant;

    public List<Tile> adjacencyList = new List<Tile>();
    public List<Tile> diagonalAdjacencyList = new List<Tile>();

    //will need to add scripts to barriers, and list of Barriers, in order to interact with them later on. 
    public int barrierCount = 0;

    //Required for Breadth first search
    public bool visited = false;
    public Tile parent = null;
    public float distance = 0;

    //Required for A*
    public float f = 0;
    public float g = 0;
    public float h = 0;

    //diagonals
    Vector3 forwardAndLeft = new Vector3(-1, 0, 1);
    Vector3 forwardAndRight = new Vector3(1, 0, 1);
    Vector3 backAndLeft = new Vector3(-1, 0, -1);
    Vector3 backAndRight = new Vector3(1, 0, -1);

    Collider[] collidersTemp = new Collider[32]; 

    private void Start()
    {
        //Go through each direction and add the neighbour
        for (int point = 0; point < 8; point++)
        {
            neighbours[point] = GetNeighbour(point);
        }
    }

    private Neighbour GetNeighbour(int position)
    {
        Vector3 direction = new Vector3();
        switch (position)
        {
            case 0: direction = Vector3.forward; break;
            case 1: direction = forwardAndRight; break;
            case 2: direction = Vector3.right; break;
            case 3: direction = backAndRight; break;
            case 4: direction = Vector3.back; break;
            case 5: direction = backAndLeft; break;
            case 6: direction = Vector3.left; break;
            case 7: direction = forwardAndLeft; break;
        }

        int colliderCount;
        bool barrier = false;

        //Barrier check overlapbox
        Vector3 barrierCheckRange = new Vector3(0.25f, 2f, 0.25f);
        colliderCount = Physics.OverlapBoxNonAlloc(transform.position + (direction / 2), barrierCheckRange, collidersTemp);
        for (int i = 0; i < colliderCount; i++)
        {
            if (collidersTemp[i].gameObject.tag == "barrier")
            {
                barrierCount++;
                barrier = true;
            }
        }

        Vector3 halfExtents = new Vector3(0.25f, 5f, 0.25f);
        colliderCount = Physics.OverlapBoxNonAlloc(transform.position + direction, halfExtents, collidersTemp);
        for (int i = 0; i < colliderCount; i++)
        {
            if (collidersTemp[i].gameObject.tag == "tile")
            {
                Neighbour neighbour = new Neighbour();
                neighbour.tile = collidersTemp[i].gameObject.GetComponent<Tile>();
                if (barrier) neighbour.cover = Cover.FULL;
                neighbour.height = neighbour.tile.gameObject.transform.position.y - transform.position.y;
                return neighbour;
            }
        }
        return null;
    }


    void Update()
    {
        if (selectable || target)
        {
            selectPlane.SetActive(true);
        }
        else
        {
            selectPlane.SetActive(false);
        }
    }

    public void Reset()
    {
        adjacencyList.Clear();
        diagonalAdjacencyList.Clear();
        barrierCount = 0;

        current = false;
        target = false;
        selectable = false;
        visited = false;
        diagonal = false;
        parent = null;
        distance = 0;

        f = g = h = 0;
    }

    public void FindNeighbours(float jumpHeight = 0, Tile targetTile = null)
    {
        Reset();

        CheckTile(Vector3.forward, jumpHeight, targetTile, false);
        CheckTile(-Vector3.forward, jumpHeight, targetTile, false);
        CheckTile(Vector3.right, jumpHeight, targetTile, false);
        CheckTile(-Vector3.right, jumpHeight, targetTile, false);

        //Diagonals
        CheckTile(forwardAndLeft, jumpHeight, targetTile, true);
        CheckTile(forwardAndRight, jumpHeight, targetTile, true);
        CheckTile(backAndLeft, jumpHeight, targetTile, true);
        CheckTile(backAndRight, jumpHeight, targetTile, true);
    }

    public void CheckTile(Vector3 direction, float jumpHeight = 0, Tile targetTile = null, bool diagonal = false) {

        //Barrier check overlapbox
        Vector3 barrierCheckRange = new Vector3(0.25f, 0.25f, 0.25f);
        Collider[] barrierCheckColliders = Physics.OverlapBox(transform.position + (direction/2), barrierCheckRange);

        foreach (Collider boundary in barrierCheckColliders)
        {
            if (boundary.gameObject.tag == "barrier")
            {
                barrierCount++;
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
                if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1) || (tile == targetTile))
                {
                    if (!diagonal) adjacencyList.Add(tile);
                    else diagonalAdjacencyList.Add(tile);
                }
            }
        }
    }
}

public enum Cover { NONE, PARTIAL, FULL };

[System.Serializable]
public class Neighbour
{
    public Tile tile;
    public float height;
    public Cover cover = Cover.NONE;
}
