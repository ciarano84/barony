using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defending : Effect
{
    //The onus should be on the causer of the effect to check that this is a viable target (and doesn't have a similar effect on them). 
    public override void AddEffect(GameObject effectCauser, GameEvent gameEvent = GameEvent.DEFAULT)
    {
        owner = gameObject.GetComponent<Unit>();
        owner.effects.Add(this);
        owner.unitInfo.currentDefence += 4;
        Initiative.OnTurnStart += RemovalCheck;
        Unit.onKO += DeathRemovalCheck;
    }

    public void DeathRemovalCheck(Unit unit)
    {
        if (unit == owner) Remove();
    }

    public override void RemovalCheck(Unit unit = null)
    {
        if (unit == owner) Remove();
    }

    public void UnSubscribe(Unit unit)
    {
        Initiative.OnTurnStart -= RemovalCheck;
        Unit.onKO -= DeathRemovalCheck;
        Unit.onKO -= UnSubscribe;
    }

    public override void Remove()
    {
        UnSubscribe(owner);
        owner.effects.Remove(this);
        owner.unitInfo.currentDefence -= 4;
        enabled = false;
    }

    public override Sprite SetImage()
    {
        return GameAssets.i.Defending;
    }
}
