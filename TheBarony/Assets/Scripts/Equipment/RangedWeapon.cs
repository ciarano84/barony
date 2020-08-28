using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedWeaponData : WeaponData
{
    public int missDistance = 20;
    public int maxAmmo;
    public int currentAmmo;
    public int rangedDamage;

    public ActionCost reloadSpeed;
}

public class RangedWeapon : Weapon
{
    public RangedWeaponData rangedWeaponData;
    Animator rootAnim;

    public override IEnumerator Attack(Target target)
    {
        int bonuses = 0;
        //owner.FindAdjacentUnits();
        RangeFinder.FindAdjacentUnits(owner);
        foreach (Unit unit in owner.adjacentUnits)
        {
            if (unit.unitInfo.faction != owner.unitInfo.faction)
            {
                bonuses--;
                break;
            }
        }

        owner.remainingActions--;

        //root animation nonsense so it doesn't fire to the side. 
        owner.FaceDirection(target.unitTargeted.gameObject.transform.position);
        rootAnim = owner.GetComponent<Animator>();
        rootAnim.SetTrigger("turnRight");
        
        yield return new WaitForSeconds(0.3f);

        //Create this animation.
        owner.unitAnim.SetTrigger("rangedAttack");

        bool hit = AttackManager.AttackRoll(owner, target.unitTargeted.GetComponent<Unit>(), bonuses);
        yield return new WaitForSeconds(owner.unitAnim.GetCurrentAnimatorStateInfo(0).length);
        GameObject missile = Instantiate(GameAssets.i.ArrowModel, owner.offHandSlot.position, owner.transform.rotation, owner.offHandSlot) as GameObject;

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
        rootAnim.SetTrigger("turnLeft");
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
                if (RangeFinder.LineOfSight(owner, unit))
                {
                    AddTarget(unit, owner.currentTile); ;
                }
            }
        }
    }

    public void Reload(ActionCost actionCost)
    {
        rangedWeaponData.currentAmmo = rangedWeaponData.maxAmmo;
        DamagePopUp.Create(transform.position + new Vector3(0, gameObject.GetComponent<TacticsMovement>().halfHeight), "Arrow nocked", false);
        if (actionCost == ActionCost.main)
        {
            owner.remainingActions -= 1; }
        else 
        { owner.remainingMove = 0; }
        Initiative.queuedActions++;
        Initiative.EndAction();
    }
}
