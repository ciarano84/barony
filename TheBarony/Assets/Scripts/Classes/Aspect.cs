using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AspectData 
{
    //This is the basic aspect (dnd class) that is inherited from. 

    public abstract void Level1(Unit unit);


    //level up()s

    public abstract void GetAspect(Unit unit);
}

public class Aspect : MonoBehaviour
{
    
}
