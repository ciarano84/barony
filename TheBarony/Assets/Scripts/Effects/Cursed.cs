using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursed : Effect
{
    public Unit cursingUnit;
    
    //The onus should be on the causer of the effect to check that this is a viable target (and doesn't have a similar effect on them). 
    public override void AddEffect(GameObject effectCauser, GameEvent gameEvent = GameEvent.FOCUSSET)
    {
        owner = gameObject.GetComponent<Unit>();
        owner.effects.Add(this);
        Unit.onSetFocus += FocusChangeRemovalCheck;
        Unit.onKO += RemovalCheck;
        AttackManager.OnAttack += ApplyCurse;
    }

    public override void RemovalCheck(Unit unit)
    {
        if (unit == cursingUnit) Remove();
    }

    public void FocusChangeRemovalCheck(Unit unit)
    {
        if (unit == cursingUnit)
        {
            if (unit.focus != owner) Remove();
        }   
    }

    public void ApplyCurse(Unit attacker, Unit defender)
    {
        if (defender == owner) AttackManager.bonuses++;
    }

    public void UnSubscribe(Unit unit)
    {
        Unit.onSetFocus -= RemovalCheck;
        Unit.onKO -= RemovalCheck;
        AttackManager.OnAttack -= ApplyCurse;
    }

    public override void Remove()
    {
        UnSubscribe(owner);
        owner.effects.Remove(this);
        Destroy(this);
    }

    public override Sprite SetImage()
    {
        return GameAssets.i.CurseIcon;
    }

    public void OnDestroy()
    {
        UnSubscribe(owner);
    }
}
