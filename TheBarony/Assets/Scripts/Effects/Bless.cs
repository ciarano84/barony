using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bless : Effect
{
    GameObject BlessVisual = null;
    Unit blessingUnit;
    
    //The onus should be on the causer of the effect to check that this is a viable target (and doesn't have a similar effect on them). 
    public override void AddEffect(GameObject effectCauser)
    {
        owner = gameObject.GetComponent<Unit>();
        owner.effects.Add(this);
        if (BlessVisual == null)
        {
            blessingUnit = effectCauser.GetComponent<Unit>();
            BlessVisual = Instantiate(GameAssets.i.Bless, owner.transform);
        }
        else
        {
            BlessVisual.SetActive(true);
        }
        owner.unitInfo.currentAttack += 2;
        owner.unitInfo.currentDefence += 2;
        TacticsMovement.OnEnterSquare += RemovalCheck;
        Unit.onKO += DeathRemovalCheck;
    }

    public void DeathRemovalCheck(Unit unit)
    {
        if ((unit == gameObject.GetComponent<Unit>()) || unit == blessingUnit)
        {
            Remove();
        }
    }

    public override void RemovalCheck(Unit unit = null)
    {
        if (blessingUnit != null)
        {
            if (RangeFinder.LineOfSight(owner, blessingUnit))
            {
                return;
            }
            else { Remove(); }
        }
        else { Remove(); }
    }

    public void UnSubscribe(Unit unit)
    {
        TacticsMovement.OnEnterSquare -= RemovalCheck;
        Unit.onKO -= DeathRemovalCheck;
        Unit.onKO -= UnSubscribe;
    }

    public override void Remove()
    {
        UnSubscribe(owner);
        owner.effects.Remove(this);
        owner.unitInfo.currentAttack -= 2;
        owner.unitInfo.currentDefence -= 2;
        BlessVisual.SetActive(false);
        enabled = false;
    }

    public override Sprite SetImage()
    {
        return GameAssets.i.BlessIcon;
    }
}
