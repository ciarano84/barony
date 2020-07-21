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
            if (unitInfo.aspectData != null) body.GetComponent<SkinnedMeshRenderer>().sharedMesh = unitInfo.aspectData.GetVisual();
            SetSlots();
            unitInfo.mainWeaponData.EquipItem(GetComponent<Unit>());
            if (unitInfo.offHandData != null) unitInfo.offHandData.EquipItem(GetComponent<Unit>());
            if (unitInfo.armourData != null) unitInfo.armourData.EquipItem(GetComponent<Unit>());
            if (unitInfo.aspectData != null) unitInfo.aspectData.GetAspect(GetComponent<Unit>());
        }
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
                            if (RangeFinder.LineOfSight(this, UnitClickedOn) == true)
                            {
                                SetFocus(UnitClickedOn);
                                if (remainingActions > 0)
                                {
                                    remainingActions--;
                                }
                                Initiative.queuedActions++;
                                StartCoroutine(Initiative.CheckForTurnEnd());
                            }  
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
                    else if (hit.collider.tag == "Tile Select")
                    {
                        Tile t = hit.collider.transform.parent.GetComponent<Tile>();
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
