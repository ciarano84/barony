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
    public Renderer renderer;
    public GameObject selectPlane;

    public List<Tile> adjacencyList = new List<Tile>();
    public List<Tile> diagonalAdjacencyList = new List<Tile>();

    //will need to add scripts to barriers, and list of Barriers, in order to interact with them later on. 
    public int barrierCount = 0;

    //Required for Breadth first search
    public bool visited = false;
    public Tile parent = null;
    public float distance = 0;

    //diagonals
    Vector3 forwardAndLeft = new Vector3(-1, 0, 1);
    Vector3 forwardAndRight = new Vector3(1, 0, 1);
    Vector3 backAndLeft = new Vector3(-1, 0, -1);
    Vector3 backAndRight = new Vector3(1, 0, -1);

    private void Start()
    {
        if (renderer == null)
        {
            renderer = GetComponent<Renderer>();
        }
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


        /*if (current)
        {
            renderer.material.color = Color.magenta;
        }
        else if (target)
        {
            renderer.material.color = Color.green;
        }
        else if (selectable)
        {
            renderer.material.color = Color.red;
        }
        else
        {
            renderer.material.color = Color.white;
        }*/
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

    public void CheckTile(Vector3 direction, float jumpHeight, bool diagonal = false) {

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
                if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1))
                {
                    if (!diagonal) adjacencyList.Add(tile);
                    else diagonalAdjacencyList.Add(tile);
                }
            }
        }
    }
}
