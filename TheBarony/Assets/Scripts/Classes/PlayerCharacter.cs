using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : TacticsMovement
{
    //public int moveSpeed;
    public int health;
    public int damage;

    private void Start()
    {
        Init();
    }
    private void Update()
    {
        if (!moving)
        {
            FindSelectableTiles();
            CheckMouse();
        }
        else
        {
            Move();
        }
    }

    void CheckMouse()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "tile")
                {
                    Debug.Log("tile");
                    Tile t = hit.collider.GetComponent<Tile>();
                    if (t.selectable)
                    {
                        MoveToTile(t); 
                    }
                }
            }
        }
    }
}
