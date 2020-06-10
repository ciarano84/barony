using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCharacter : TacticsMovement
{
    //public int moveSpeed;
    GameObject visual;

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
            unitInfo.weaponData.SetWeaponData();
            unitInfo.unitVisual = "EnemyVisual";
        }
        //This next section is just for when we start direct in a combat.  
        if (this.unitInfo.weaponData == null)
        {
            unitInfo.weaponData = new MeleeWeaponData();
            unitInfo.damageModifier = 2;
            unitInfo.weaponData.SetWeaponData();
        }

        //Get the visual. This, at present, will only affect player characters (this script). There's nothing in place for enemies. 
        visual = Instantiate(Resources.Load(unitInfo.unitVisual), transform) as GameObject;
        visual.transform.position = transform.position;

        unitInfo.weaponData.CreateWeapon(this);
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position, transform.forward);

        if (!turn)
        {
            return;
        }

        if (!moving && Initiative.queuedActions < 1)
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

                        if (remainingActions > 0)
                        {
                            foreach (Weapon.Target target in weapon1.targets)
                            {
                                if (target.unitTargeted == UnitClickedOn)
                                {
                                    Initiative.queuedActions += unitInfo.weaponData.actionsPerAttack;
                                    weapon1.StartCoroutine("Attack", target);
                                    return;
                                }
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
                            return;
                        }
                    }
                }
            }
        }
    }
}
