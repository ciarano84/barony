using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

//public enum CriticalType { DAMAGEUP, BLEED, BATTER, PIERCE, CRUSH, FLURRY, GOUGE, CRIPPLE, WEAKEN, SUNDER, STUN, TRIP }

public class CriticalManager
{
    public static List<Critical> criticalChain = new List<Critical>();
    static int tier = 0;

    public static void AddCritical(Weapon _weapon)
    {
        if (tier == 0) criticalChain.Add(_weapon.CoreCritical());
        else criticalChain.Add(new c_DamageUp());
        tier = criticalChain.Count +1;
    }

    public static void Reset()
    {
        criticalChain.Clear();
        tier = 0;
    }
}

public abstract class Critical
{
    //This dictates if the critical effect takes place before the damage (false) or after (true).
    public abstract bool AfterDamage();

    public abstract string Name();

    public abstract void CriticalEffect();
}

public class c_DamageUp : Critical
{
    public override string Name() { return "DamageUp"; }

    public override bool AfterDamage() { return false; }

    public override void CriticalEffect()
    {
        Debug.Log("damage up critical takes effect");
        AttackManager.damage += 2;
    }
}

public class c_Cripple : Critical
{
    public override string Name() { return "Cripple"; }

    public override bool AfterDamage() { return true; }

    public override void CriticalEffect()
    {
        if (AttackManager.wounds < 1) return;

        Debug.Log("crippled critical takes effect");
        
        Unit attacker = AttackManager.attacker;
        Unit defender = AttackManager.defender;
        
        DamagePopUp.Create(defender.transform.position + new Vector3(0, (defender.gameObject.GetComponent<TacticsMovement>().halfHeight) + 0.5f), "Crippled", false);

        if (defender.GetComponent<Crippled>() == null)
        {
            Crippled crippled = defender.gameObject.AddComponent<Crippled>();
            crippled.AddEffect(attacker.gameObject);
        }
        else if (!defender.GetComponent<Crippled>().enabled)
        {
            defender.GetComponent<Crippled>().enabled = true;
            defender.GetComponent<Crippled>().AddEffect(defender.gameObject);
        }
    }
}

public class c_Gouge : Critical
{
    public override string Name() { return "Gouge"; }

    public override bool AfterDamage() { return false; }

    public override void CriticalEffect()
    {
        Debug.Log("gouge critical takes effect");
        AttackManager.woundValueAdjustment += 2;
    }
}

public class c_Crush : Critical
{
    public override string Name() { return "Crush"; }

    public override bool AfterDamage() { return false; }

    public override void CriticalEffect()
    {
        Debug.Log("Crush critical takes effect");
        AttackManager.grazeDamage -= 1;
    }
}

public class c_ArmourPierce : Critical
{
    public override string Name() { return "Armour Piercing"; }

    public override bool AfterDamage() { return false; }

    public override void CriticalEffect()
    {
        Debug.Log("Armour piercing critical takes effect");
        AttackManager.armourPierce = true;
    }
}
