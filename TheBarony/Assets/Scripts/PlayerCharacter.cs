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

        //this should just read 'unitInfo.weaponData.CreateWeapon(this);' but has the rest to catch the proxy npcs. 
        if (unitInfo.unitName == "goblin")
        {
            unitInfo = new UnitInfo();
            unitInfo.mainWeaponData = new ShortswordData();
            unitInfo.mainWeaponData.SetData(unitInfo);
            unitInfo.faction = Factions.enemies;
            unitInfo.mainWeaponData.EquipItem(GetComponent<Unit>());
            Instantiate(GameAssets.i.EnemyVisual, transform);
        }
        //This next section is just for when we start direct in a combat.  
        if (this.unitInfo.mainWeaponData == null)
        {
            unitInfo.mainWeaponData = new ShortswordData();

            unitInfo.mainWeaponData.EquipItem(GetComponent<Unit>());
        }

        SetStats();
        unitInfo.mainWeaponData.EquipItem(GetComponent<Unit>());
        if (unitInfo.offHandData != null) unitInfo.offHandData.EquipItem(GetComponent<Unit>());
        if (unitInfo.armourData != null) unitInfo.armourData.EquipItem(GetComponent<Unit>());
        if (unitInfo.aspectData != null) unitInfo.aspectData.GetAspect(GetComponent<Unit>());
        if (unitInfo.aspectData != null) Instantiate(unitInfo.aspectData.GetVisual(), transform);
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
