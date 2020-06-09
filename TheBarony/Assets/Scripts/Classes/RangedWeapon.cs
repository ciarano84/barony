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
    int missDistance = 2;
    new public int actionsPerAttack = 1;

    public override IEnumerator Attack(Target target)
    {
        Debug.Log("Attack called");
        owner.moving = true;
        owner.FaceDirection(target.unitTargeted.gameObject.transform.position);
        yield return new WaitForSeconds(0.3f);

        //Create this animation.
        owner.unitAnim.SetTrigger("rangedAttack");
        yield return new WaitForSeconds(0.3f);

        bool hit = AttackManager.RangedAttackRoll(owner, target.unitTargeted.GetComponent<Unit>());
        yield return new WaitForSeconds(owner.unitAnim.GetCurrentAnimatorStateInfo(0).length);
        GameObject missile = Instantiate(Resources.Load("Arrow"), owner.transform.position, owner.transform.rotation, owner.gameObject.transform) as GameObject;

        if (hit)
        {
            //Hit goes here.
            Debug.Log("hit");
            missile.transform.LookAt(target.unitTargeted.transform);

        }
        else
        {
            //miss goes here. 
            Debug.Log("miss");
            Vector3 missPosition = new Vector3(0, missDistance, 0);
            missile.transform.LookAt(target.unitTargeted.transform.position + missPosition);
        }

        yield return new WaitForSeconds(2f);
        owner.remainingActions--;
        owner.moving = false;
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

        foreach (Target t in targets)
        {
            //Debug.Log(t.unitTargeted.gameObject);
        }
    }
}
