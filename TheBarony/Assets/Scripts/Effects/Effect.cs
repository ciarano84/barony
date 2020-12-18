using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameEvent { DEFAULT, TURNSTART, TURNEND, WOUNDED, HIT, ATTACKING, MOVED, FOCUSSET };

public abstract class Effect : MonoBehaviour
{
    //effects should all attach to the UNIT gameobject. Then instantiate any visuals or other required game objects. 
    //responsibility for vetting what should have an effect should be on the causer of the effect. 
    //effects should be responsible for checking if they are still needed, and removing themselves appropriately. 
    
    public delegate void OnEffectEndDelegate(Unit affectedUnit, Effect effect);
    public static OnEffectEndDelegate OnEffectEnd;

    public Unit owner;
    public GameEvent endCondition;

    public abstract void AddEffect(GameObject effectCauser, GameEvent endCondition = GameEvent.DEFAULT);

    public abstract void RemovalCheck(Unit unit);

    public abstract void Remove();

    public abstract Sprite SetImage();
}
