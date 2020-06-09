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

    public override void InitUnit()
    {
        //this should just read 'unitInfo.weaponData.CreateWeapon(this);' but has the rest to catch the proxy npcs. 
        if (unitInfo.unitName == "nobody")
        {
            unitInfo = new UnitInfo();
            unitInfo.weaponData = new MeleeWeaponData();
            unitInfo.faction = Factions.enemies;
            unitInfo.damageModifier = 2;
        }
        //This next section is just for when we start direct in a combat.  
        if (this.unitInfo.weaponData == null)
        {
            unitInfo.weaponData = new MeleeWeaponData();
            unitInfo.damageModifier = 2;
        }
        
        unitInfo.weaponData.CreateWeapon(this);
    }

    private void FixedUpdate()
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
            Debug.Log("mousebutton up");
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
                                Initiative.queuedActions += weapon1.actionsPerAttack;
                                weapon1.StartCoroutine("Attack", target);
                                //weapon1.StartCoroutine(MeleeAttack(target));
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
}
