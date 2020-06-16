using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderData : AspectData
{
    public override void SetAspectData()
    {
        className = "Defender";
    }

    public override void Level1(Unit unit)
    {    
    }

    public override void GetAspect(Unit unit)
    {
        Defender defenderAspect = unit.gameObject.AddComponent<Defender>();
        defenderAspect.owner = unit;
    }

    public override GameObject GetVisual() { return GameAssets.i.DefenderVisual; }

}

public class Defender : Aspect
{
    private void Start()
    {
        AttackManager.OnGraze += SoakDamage;
    }

    public void SoakDamage(Unit attacker, Unit defender)
    {
        if (defender == owner)
        {
            AttackManager.grazeDamage = -1;
        }
    }
}


