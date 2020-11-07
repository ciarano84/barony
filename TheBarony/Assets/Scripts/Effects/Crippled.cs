using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crippled : Effect
{
    public override void AddEffect(GameObject effectCauser, GameEvent gameEvent = GameEvent.DEFAULT)
    {
        owner = gameObject.GetComponent<Unit>();
        owner.effects.Add(this);
        owner.unitInfo.currentMove = (int)Math.Floor(owner.unitInfo.currentMove / 2f);
    }

    public override void RemovalCheck(Unit unit)
    { }

    public override void Remove()
    { }

    public void UnSubscribe(Unit unit)
    { }

    public override Sprite SetImage()
    {
        return GameAssets.i.CrippledIcon;
    }
}
