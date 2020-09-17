using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priming : Effect
{
    bool turnStartFlag;
    enum PrimeAttackType { AIMED, MIGHTY };
    PrimeAttackType primeAttackType;

    public override void AddEffect(GameObject effectCauser, GameEvent gameEvent = GameEvent.DEFAULT)
    {
        owner = gameObject.GetComponent<Unit>();
        owner.effects.Add(this);
        if (owner.mainWeapon.weaponData.rangeType == WeaponData.Range.ranged) primeAttackType = PrimeAttackType.AIMED;
        else if (owner.mainWeapon.weaponData.rangeType == WeaponData.Range.melee && owner.mainWeapon.weaponData.weight >= ItemData.Weight.medium) primeAttackType = PrimeAttackType.MIGHTY;

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

        AttackManager.OnAttack += BoostAttack;
        AttackManager.OnWound += WoundRemovalCheck;
        TacticsMovement.OnEnterSquare += RemovalCheck;
        Unit.onKO += DeathRemovalCheck;
    }

    public void DeathRemovalCheck(Unit unit)
    {
        if (unit == owner) Remove();
    }

    //This is only triggered by something moving. 
    public override void RemovalCheck(Unit unit = null)
    {
        if (unit == owner)
        {
            if (primeAttackType == PrimeAttackType.AIMED) Remove();
        }
    }

    public void UnSubscribe(Unit unit)
    {
        //Do all unsubscribes. 
        //one of which, for future reference, will be on weapon change. 

        AttackManager.OnAttack -= BoostAttack;
        AttackManager.OnWound -= WoundRemovalCheck;
        TacticsMovement.OnEnterSquare -= RemovalCheck;
        Unit.onKO -= DeathRemovalCheck;
        Unit.onKO -= UnSubscribe;
        turnStartFlag = false;
    }

    public override void Remove()
    {
        UnSubscribe(owner);
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
                DamagePopUp.Create(gameObject.transform.position + new Vector3(0, (gameObject.GetComponent<TacticsMovement>().halfHeight) + 0.5f), "Aimed Shot", false);
            }
            else if (primeAttackType == PrimeAttackType.MIGHTY)
            {
                AttackManager.damage += ((int)owner.mainWeapon.weaponData.weight) * 2;
                DamagePopUp.Create(gameObject.transform.position + new Vector3(0, (gameObject.GetComponent<TacticsMovement>().halfHeight) + 0.5f), "Mighty Attack", false);
            }
        }
        Remove();
    }

    void WoundRemovalCheck(Unit attacker, Unit defender)
    {
        if (defender == owner) Remove();
    }
}
