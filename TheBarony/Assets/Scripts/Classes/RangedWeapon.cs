using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeaponData : WeaponData
{
    public override void SetData()
    {
        imageFile = "Shortbow";
    }

    public override void EquipItem(Unit unit)
    {
        RangedWeapon weapon = unit.gameObject.AddComponent<RangedWeapon>();
        weapon.owner = unit.gameObject.GetComponent<PlayerCharacter>();
        unit.unitInfo.weaponData = this;
        unit.currentWeapon = weapon;
        weapon.rangeType = Weapon.Range.ranged;
        weapon.range = 200;
        weapon.actionsPerAttack = 1;
        weapon.missDistance = 20;
        unit.unitInfo.currentDamage = 3;
    }
}

public class RangedWeapon : Weapon
{
    public int missDistance = 20;

    public override IEnumerator Attack(Target target)
    {
        owner.remainingActions--;
        owner.FaceDirection(target.unitTargeted.gameObject.transform.position);
        yield return new WaitForSeconds(0.3f);

        //Create this animation.
        owner.unitAnim.SetTrigger("rangedAttack");

        bool hit = AttackManager.RangedAttackRoll(owner, target.unitTargeted.GetComponent<Unit>());
        yield return new WaitForSeconds(owner.unitAnim.GetCurrentAnimatorStateInfo(0).length);
        GameObject missile = Instantiate(Resources.Load("Arrow"), owner.transform.position, owner.transform.rotation, owner.gameObject.transform) as GameObject;

        if (hit)
        {
            //Hit goes here.
            missile.GetComponent<Missile>().target = target.unitTargeted.transform.position;
            missile.GetComponent<Missile>().Launch(true);
            yield return new WaitForSeconds(1.2f);
            AttackManager.DamageRoll(owner, target.unitTargeted.GetComponent<Unit>());
        }
        else
        {
            //miss goes here. 
            DamagePopUp.Create(transform.position + new Vector3(0, GetComponent<TacticsMovement>().halfHeight), "miss", false);
            Vector3 missTarget = new Vector3(1, 1, missDistance);
            missile.GetComponent<Missile>().target = target.unitTargeted.transform.position + missTarget;
            missile.GetComponent<Missile>().Launch(false);
        }

        yield return new WaitForSeconds(1f);
        Initiative.EndAction();

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
    }
}
