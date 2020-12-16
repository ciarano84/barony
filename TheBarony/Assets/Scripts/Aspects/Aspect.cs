using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AspectData 
{
    public string className;
    public UnitInfo unitInfo;
    public int tier = 1;

    //This is the basic aspect (dnd class) that is inherited from. 
    public abstract void SetAspectData(UnitInfo unitInfo);

    public abstract void Tier1();
    //level up()s

    public abstract void Tier2();

    public abstract void GetAspect(Unit unit);
    public abstract Mesh GetVisual();
}

public abstract class Aspect : MonoBehaviour
{
    public Unit owner;
    public AspectData aspectData;

    public abstract void GetAspectData();
}
