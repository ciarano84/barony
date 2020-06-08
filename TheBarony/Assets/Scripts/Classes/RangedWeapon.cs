using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeaponData : WeaponData
{
    public override void SetWeaponData()
    {
        imageFile = "Shortbow";
        range = 200;
    }

    public override void CreateWeapon(Unit unit)
    {
        RangedWeapon weapon = unit.gameObject.AddComponent<RangedWeapon>();
        weapon.owner = unit.gameObject.GetComponent<PlayerCharacter>();
        weapon.weaponData = this;
        unit.weapon1 = weapon;
    }
}

public class RangedWeapon : Weapon
{
    public override IEnumerator Attack(Target target)
    {
        yield break;
    }

    public override void GetTargets()
    {
        targets.Clear();
        foreach (TacticsMovement unit in Initiative.order)
        {
            if (unit != owner.GetComponent<TacticsMovement>())
            {
                Vector3 rayOrigin = owner.gameObject.transform.position;
                Vector3 centre = -Vector3.up;

                //Debug
                Debug.DrawRay(transform.position, (unit.gameObject.transform.position - transform.position), Color.green, 1000f);

                // Declare a raycast hit to store information about what our raycast has hit
                if (Physics.Raycast(rayOrigin, (unit.gameObject.transform.position - transform.position), out RaycastHit hit))
                {
                    if (unit == hit.collider.gameObject.GetComponent<TacticsMovement>())
                    {
                        AddTarget(unit, owner.currentTile); ;
                    }
                }
            }
        }

        foreach (Target t in targets)
        {
            Debug.Log(t.unitTargeted.gameObject);
        }
    }
}
