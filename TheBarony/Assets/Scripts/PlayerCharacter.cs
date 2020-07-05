using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCharacter : TacticsMovement
{
    private void Start()
    {
        InitUnit();
        InitTacticsMovement();
    }

    public override void InitUnit()
    {
        if (unitInfo.faction == Factions.players)
        {
            SetStats();
            unitInfo.mainWeaponData.EquipItem(GetComponent<Unit>());
            if (unitInfo.offHandData != null) unitInfo.offHandData.EquipItem(GetComponent<Unit>());
            if (unitInfo.armourData != null) unitInfo.armourData.EquipItem(GetComponent<Unit>());
            if (unitInfo.aspectData != null) unitInfo.aspectData.GetAspect(GetComponent<Unit>());
            if (unitInfo.aspectData != null) Instantiate(unitInfo.aspectData.GetVisual(), transform);
        }
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

                        if (canFocusSwitch)
                        {
                            //ActionUI should show the focus icon.
                            SetFocus(UnitClickedOn);
                            if (remainingActions > 0)
                            {
                                remainingActions--;
                            } 
                            //i have a bad feeling about this line. 
                            Initiative.queuedActions++;
                            StartCoroutine(Initiative.CheckForTurnEnd());
                        }
                        else if (remainingActions > 0)
                        {
                            foreach (Weapon.Target target in mainWeapon.targets)
                            {
                                if (target.unitTargeted == UnitClickedOn)
                                {
                                    Initiative.queuedActions += mainWeapon.weaponData.actionsPerAttack;
                                    mainWeapon.StartCoroutine("Attack", target);
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
