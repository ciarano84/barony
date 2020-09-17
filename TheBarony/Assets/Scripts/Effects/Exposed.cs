using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exposed : Effect
{
    public override void AddEffect(GameObject effectCauser, GameEvent gameEvent = GameEvent.DEFAULT)
    {
        owner = gameObject.GetComponent<Unit>();
        owner.effects.Add(this);

        OnEffectEnd += PrimeEndRemovalCheck;
        AttackManager.OnAttack += ExposeToAttack;
    }

    public override void RemovalCheck(Unit unit)
    { }

    public override void Remove()
    {
        UnSubscribe(owner);
        owner.effects.Remove(this);
        enabled = false;
    }

    public void UnSubscribe(Unit unit)
    {
        //Do all unsubscribes.  

        OnEffectEnd -= PrimeEndRemovalCheck;
        AttackManager.OnAttack -= ExposeToAttack;
        Unit.onKO -= UnSubscribe;
    }

    public override Sprite SetImage()
    {
        return GameAssets.i.ExposedIcon;
    }

    public void PrimeEndRemovalCheck(Unit unit, Effect effect)
    {
        if (unit == owner)
        {
            if (effect is Priming) Remove();
        }
    }

    public void ExposeToAttack(Unit attacker, Unit defender)
    {
        if (owner == defender) AttackManager.bonuses++;
    }
}
