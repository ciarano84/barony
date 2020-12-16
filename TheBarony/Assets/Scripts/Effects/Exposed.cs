using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exposed : Effect
{
    DefenceType storedDefenceType;

    public override void AddEffect(GameObject effectCauser, GameEvent endCondition = GameEvent.DEFAULT)
    {
        owner = gameObject.GetComponent<Unit>();
        owner.effects.Add(this);
        storedDefenceType = owner.defenceType;
        owner.defenceType = DefenceType.EXPOSED;

        if (endCondition == GameEvent.TURNSTART)
        {
            Initiative.OnTurnStart += TurnStartRemovalCheck;
        }

        OnEffectEnd += PrimeEndRemovalCheck;
    }

    public override void RemovalCheck(Unit unit)
    { }

    public override void Remove()
    {
        Unsubscribe(owner);
        owner.defenceType = storedDefenceType;
        owner.effects.Remove(this);
        Destroy(this);
    }

    public void Unsubscribe(Unit unit)
    {
        //Do all unsubscribes.  
        OnEffectEnd -= PrimeEndRemovalCheck;
        Unit.onKO -= Unsubscribe;
        Initiative.OnTurnStart -= TurnStartRemovalCheck;
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

    public void TurnStartRemovalCheck(Unit unit)
    {
        if (unit == owner)
        {
            Remove();
        }
    }

    private void OnDestroy()
    {
        Unsubscribe(owner);
    }
}
