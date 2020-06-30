using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedWeaponData : WeaponData
{
    public int missDistance = 20;
    public int maxAmmo;
    public int currentAmmo;
    public int rangedDamage;
}

public class RangedWeapon : Weapon
{
    public RangedWeaponData rangedWeaponData;

    public override IEnumerator Attack(Target target)
    {
        int bonuses = 0;
        owner.FindAdjacentUnits();
        foreach (Unit unit in owner.adjacentUnits)
        {
            if (unit.unitInfo.faction != owner.unitInfo.faction)
            {
                bonuses--;
                break;
            }
        }

        owner.remainingActions--;
        owner.FaceDirection(target.unitTargeted.gameObject.transform.position);
        yield return new WaitForSeconds(0.3f);

        //Create this animation.
        owner.unitAnim.SetTrigger("rangedAttack");

        bool hit = AttackManager.RangedAttackRoll(owner, target.unitTargeted.GetComponent<Unit>(), bonuses);
        yield return new WaitForSeconds(owner.unitAnim.GetCurrentAnimatorStateInfo(0).length);
        GameObject missile = Instantiate(Resources.Load("Arrow"), owner.transform.position, owner.transform.rotation, owner.gameObject.transform) as GameObject;

        if (hit)
        {
            //Hit goes here.
            missile.GetComponent<Missile>().target = target.unitTargeted.transform.position;
            missile.GetComponent<Missile>().Launch(true);
            yield return new WaitForSeconds(1.2f);
            int storedDamage = owner.unitInfo.currentDamage;
            owner.unitInfo.currentDamage = rangedWeaponData.rangedDamage;
            AttackManager.DamageRoll(owner, target.unitTargeted.GetComponent<Unit>());
            owner.unitInfo.currentDamage = storedDamage;
        }
        else
        {
            //miss goes here. 
            DamagePopUp.Create(transform.position + new Vector3(0, 2 * GetComponent<TacticsMovement>().halfHeight), "miss", false);
            missile.GetComponent<Missile>().target = target.unitTargeted.transform.position;
            missile.GetComponent<Missile>().Launch(false);
        }

        yield return new WaitForSeconds(1f);
        rangedWeaponData.currentAmmo--;
        Initiative.EndAction();

        yield break;
    }

    public override void GetTargets()
    {
        targets.Clear();
        if (rangedWeaponData.currentAmmo == 0) return;

        foreach (TacticsMovement unit in Initiative.order)
        {
            if (unit != owner.GetComponent<TacticsMovement>())
            {
                Vector3 rayOrigin = owner.gameObject.transform.position;
                Vector3 centre = -Vector3.up;

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

    public void Reload(bool asMainAction = false)
    {
        rangedWeaponData.currentAmmo = rangedWeaponData.maxAmmo;
        DamagePopUp.Create(transform.position + new Vector3(0, gameObject.GetComponent<TacticsMovement>().halfHeight), "Arrow nocked", false);
        if (asMainAction)
        { owner.remainingActions -= 1; }
        else 
        { owner.remainingMove = 0; }
        Initiative.EndAction();
    }
}
