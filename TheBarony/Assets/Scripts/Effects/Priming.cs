using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Priming : Effect
{
    bool startCheckingForMainActionUsed;
    enum PrimeAttackType { AIMED, MIGHTY };
    PrimeAttackType primeAttackType;

    public override void AddEffect(GameObject effectCauser, GameEvent endCondition = GameEvent.DEFAULT)
    {
        owner = gameObject.GetComponent<Unit>();
        owner.effects.Add(this);

        //add exposed effect
        if (owner.GetComponent<Exposed>() == null)
        {
            Exposed exposed = owner.gameObject.AddComponent<Exposed>();
            exposed.AddEffect(owner.gameObject);
        }
        else if (!owner.GetComponent<Exposed>().enabled)
        {
            owner.GetComponent<Exposed>().enabled = true;
            owner.GetComponent<Exposed>().AddEffect(owner.gameObject);
        }

        if (owner.mainWeapon.weaponData.rangeType == WeaponData.Range.ranged) owner.aimingBow = true;
        owner.GetComponent<TacticsMovement>().FaceDirection(owner.focus.transform.position);

        if (owner.mainWeapon.weaponData.rangeType == WeaponData.Range.ranged)
        {
            primeAttackType = PrimeAttackType.AIMED;
            owner.unitAnim.SetBool("PrimeAimed", true);
        }
        else if (owner.mainWeapon.weaponData.rangeType == WeaponData.Range.melee && owner.mainWeapon.weaponData.weight >= ItemData.Weight.medium)
        {
            primeAttackType = PrimeAttackType.MIGHTY;
            owner.unitAnim.SetBool("PrimeMighty", true);
        }

        AttackManager.OnAttack += BoostAttack;
        AttackManager.OnGraze += GrazeRemovalCheck;
        AttackManager.OnWound += GrazeRemovalCheck;
        TacticsMovement.OnEnterSquare += RemovalCheck;
        TacticsMovement.OnDodge += DodgeRemovalCheck;
        Initiative.OnActionTaken += MainActionTakenRemovalCheck;
        Unit.onSetFocus += FocusSetRemovalCheck;
        Unit.onKO += DeathRemovalCheck;
    }

    public void DeathRemovalCheck(Unit unit)
    {
        if (unit == owner || unit == owner.focus)
        {
            Remove();
        }   
    }

    public void FocusSetRemovalCheck(Unit unit)
    {
        if (unit == owner)
        {
            Remove();
        }
    }

    //This is only triggered by something moving. 
    public override void RemovalCheck(Unit unit = null)
    {
        if (unit == owner)
        {
            if (primeAttackType == PrimeAttackType.AIMED)
            {
                Remove();
                owner.GetComponent<TacticsMovement>().FaceDirection(owner.focus.transform.position);
            }
        }
    }

    public void MainActionTakenRemovalCheck(Unit unit = null)
    {
        if (unit == owner)
        {
            if (unit.GetComponent<TacticsMovement>().remainingActions < 1)
            {
                if (!startCheckingForMainActionUsed) startCheckingForMainActionUsed = true;
                else
                {
                    Remove();
                } 
            }    
        }
    }

    public void UnSubscribe(Unit unit)
    {
        //Do all unsubscribes. 
        //one of which, for future reference, will be on weapon change. 

        AttackManager.OnAttack -= BoostAttack;
        AttackManager.OnGraze -= GrazeRemovalCheck;
        AttackManager.OnWound -= GrazeRemovalCheck;
        TacticsMovement.OnEnterSquare -= RemovalCheck;
        Unit.onKO -= DeathRemovalCheck;
        Unit.onKO -= UnSubscribe;
        Initiative.OnActionTaken -= MainActionTakenRemovalCheck;
        Unit.onSetFocus -= FocusSetRemovalCheck;
        startCheckingForMainActionUsed = false;
    }

    public override void Remove()
    {
        UnSubscribe(owner);
        owner.unitAnim.SetBool("PrimeMighty", false);
        owner.unitAnim.SetBool("PrimeAimed", false);
        owner.aimingBow = false;
        OnEffectEnd(owner, this);
        owner.effects.Remove(this);
        enabled = false;
    }

    public override Sprite SetImage()
    {
        return GameAssets.i.PrimingIcon;
    }

    void BoostAttack(Unit attacker, Unit defender)
    {
        if (attacker == owner)
        {
            //put the benefit to the attack in here. 
            if (primeAttackType == PrimeAttackType.AIMED)
            {
                AttackManager.bonuses += 1;
                gameObject.GetComponent<UnitPopUpManager>().AddPopUpInfo("aimed shot");
            }
            else if (primeAttackType == PrimeAttackType.MIGHTY)
            {
                AttackManager.damage += ((int)owner.mainWeapon.weaponData.weight) * 2;
                gameObject.GetComponent<UnitPopUpManager>().AddPopUpInfo("mighty attack");
            }
            Remove();
        }
    }

    void GrazeRemovalCheck(Unit attacker, Unit defender)
    {
        if (defender == owner)
        {
            owner.aimingBow = false;
            owner.GetComponent<TacticsMovement>().FaceDirection(owner.focus.transform.position);
            Remove();
        }      
    }

    void DodgeRemovalCheck(Unit _defender, Result _result)
    {   
        if (_defender == this)
        {
            if (_result == Result.PARTIAL)
            {
                Remove();
            }
        }
    }

    private void OnDestroy()
    {
        UnSubscribe(gameObject.GetComponent<Unit>());
    }
}
