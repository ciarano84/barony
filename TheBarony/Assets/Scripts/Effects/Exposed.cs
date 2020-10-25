using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exposed : Effect
{
    DefenceType storedDefenceType;

    public override void AddEffect(GameObject effectCauser, GameEvent gameEvent = GameEvent.DEFAULT)
    {
        owner = gameObject.GetComponent<Unit>();
        owner.effects.Add(this);
        storedDefenceType = owner.defenceType;
        owner.defenceType = DefenceType.EXPOSED;

        OnEffectEnd += PrimeEndRemovalCheck;
    }

    public override void RemovalCheck(Unit unit)
    { }

    public override void Remove()
    {
        UnSubscribe(owner);
        owner.defenceType = storedDefenceType;
        owner.effects.Remove(this);
        Destroy(this);
    }

    public void UnSubscribe(Unit unit)
    {
        //Do all unsubscribes.  
        OnEffectEnd -= PrimeEndRemovalCheck;
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
}
