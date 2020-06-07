using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeaponData : WeaponData
{
    public override void SetWeaponData()
    {
        imageFile = "Shortbow";
        range = 20;
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
                Vector3 rayOrigin = owner.transform.position;
                // Declare a raycast hit to store information about what our raycast has hit
                RaycastHit hit;
                if (Physics.Raycast(rayOrigin, owner.transform.forward, out hit, owner.weapon1.weaponData.range))
                { 
                    
                }
            }
        }
    }
}
