using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderData : AspectData
{
    public override void Level1(Unit unit)
    {
        
    }

    public override void GetAspect(Unit unit)
    {
        Defender defenderAspect = unit.gameObject.AddComponent<Defender>();
        defenderAspect.owner = unit;
    }
}

public class Defender : Aspect
{
    public Unit owner;

    private void Start()
    {
        AttackManager.OnGraze += SoakDamage;
    }

    public void SoakDamage(Unit attacker, Unit defender)
    {
        Debug.Log("SoakDamageCalled");
        if (defender == owner)
        {
            AttackManager.grazeDamage = -1;
        }
    }
}


