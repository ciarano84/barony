using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roaring : Effect
{
    //The onus should be on the causer of the effect to check that this is a viable target (and doesn't have a similar effect on them). 
    public override void AddEffect(GameObject effectCauser, GameEvent gameEvent = GameEvent.TURNSTART)
    {
        owner = gameObject.GetComponent<Unit>();
        owner.effects.Add(this);
        AttackManager.OnAttack += AddPenalty;
        Initiative.OnTurnStart += RemovalCheck;
    }

    public override void RemovalCheck(Unit unit = null)
    {
        if (unit == owner) Remove();
    }

    public void AddPenalty(Unit attacker, Unit defender)
    {
        if (defender == owner) AttackManager.bonuses--;
    }

    public void UnSubscribe(Unit unit)
    {
        Initiative.OnTurnStart -= RemovalCheck;
    }

    public override void Remove()
    {
        UnSubscribe(owner);
        owner.effects.Remove(this);
        Destroy(this);
    }

    public override Sprite SetImage()
    {
        return GameAssets.i.RoarIcon;
    }

    public void OnDestroy()
    {
        UnSubscribe(owner);
    }
}
