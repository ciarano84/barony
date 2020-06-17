using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AspectData 
{
    public string className;
    public UnitInfo unitInfo;

    //This is the basic aspect (dnd class) that is inherited from. 
    public abstract void SetAspectData(UnitInfo unitInfo);

    public abstract void Level1();
    //level up()s

    public abstract void GetAspect(Unit unit);
    public abstract GameObject GetVisual();
}

public class Aspect : MonoBehaviour
{
    public Unit owner;
}
