using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCharacter : TacticsMovement
{
    //public int moveSpeed;

    private void Start()
    {
        InitTacticsMovement();
        InitUnit();
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
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    //Work out if a TacticsMovement has been selected.
                    if (hit.collider.GetComponent<TacticsMovement>() != null)
                    {
                        TacticsMovement UnitClickedOn = hit.collider.GetComponent<TacticsMovement>();
                        foreach (Weapon.Target target in weapon1.targets)
                        {
                            if (target.unitTargeted == UnitClickedOn)
                            {
                                Initiative.queuedActions += 2;
                                weapon1.StartCoroutine("MeleeAttack", target);
                            }
                        }
                    }
                    else if (hit.collider.tag == "tile")
                    {
                        Tile t = hit.collider.GetComponent<Tile>();
                        if (t.selectable)
                        {
                            Initiative.queuedActions++;
                            MoveToTile(t);
                        }
                    }
                }
            }
        }
    }
    /*
    public PlayerCharacter ShallowCopy()
    {
        return (PlayerCharacter)this.MemberwiseClone();
    }
    */
}
