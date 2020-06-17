using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : MonoBehaviour
{
    //effects should all attach to the UNIT gameobject. Then instantiate any visuals or other required game objects. 
    //responsibility for vetting what should have an effect should be on the causer of the effect. 
    //effects should be responsible for checking if they are still needed, and removing themselves appropriately. 
    
    public Unit owner;

    public abstract void AddEffect(GameObject effectCauser);

    public abstract void RemovalCheck(Unit unit);

    public abstract void Remove();
}
