using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeaponData : WeaponData
{
    public override void SetData(UnitInfo unitInfo)
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
        weapon.maxAmmo = 1;
        weapon.currentAmmo = 1;
    }
}

public class RangedWeapon : Weapon
{
    public int missDistance = 20;
    public int maxAmmo;
    public int currentAmmo;

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
            DamagePopUp.Create(transform.position + new Vector3(0, 2 * GetComponent<TacticsMovement>().halfHeight), "miss", false);
            missile.GetComponent<Missile>().target = target.unitTargeted.transform.position;
            missile.GetComponent<Missile>().Launch(false);
        }

        yield return new WaitForSeconds(1f);
        currentAmmo--;
        Initiative.EndAction();

        yield break;
    }

    public override void GetTargets()
    {
        targets.Clear();
        if (currentAmmo == 0) return;

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
        currentAmmo = maxAmmo;
        DamagePopUp.Create(transform.position + new Vector3(0, gameObject.GetComponent<TacticsMovement>().halfHeight), "Arrow nocked", false);
        if (asMainAction)
        { owner.remainingActions -= 1; }
        else 
        { owner.remainingMove = 0; }
        Initiative.EndAction();
    }
}
