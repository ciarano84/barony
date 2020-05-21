using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCharacter : TacticsMovement
{
    //public int moveSpeed;
    public int maxHealth;
    public int currentHealth;
    public int Resiliance;
    public int damageModifier;
    public int attackModifier;
    public Weapon weapon1;

    private void Start()
    {
        Init();
        weapon1 = this.gameObject.AddComponent<Weapon>();
        weapon1.owner = this;
    }
    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward);

        if (!turn)
        {
            return;
        }

        if (!moving)
        {
            //FindSelectableTiles();
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
                    //Work out if a TacticsMovement has been selected.
                    if (hit.collider.GetComponent<TacticsMovement>() != null)
                    {
                        if (weapon1.unitsInMeleeReach.ContainsKey(hit.collider.GetComponent<TacticsMovement>()))
                        {
                        //debug
                        Debug.Log("reached");

                            Tile tile;
                            weapon1.unitsInMeleeReach.TryGetValue(hit.collider.GetComponent<TacticsMovement>(), out tile);
                            weapon1.Attack(hit.collider.GetComponent<TacticsMovement>(), tile);
                        }
                    }
                    else if (hit.collider.tag == "tile")
                    {
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
