using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponData : ItemData
{
    public int actionsPerAttack;
    public int range;
    public int weaponAttack;
    public int weaponDamage;

    public enum Range { melee, ranged };
    public Range rangeType;
}

public class Weapon : Item
{
    public WeaponData weaponData;
    public Target currentTarget;

    public class Target
    {
        public TacticsMovement unitTargeted;
        public Tile tileToAttackFrom;
    }

    public List<Target> targets = new List<Target>();

    public virtual IEnumerator Attack(Target target)
    {
        //to be overidden
        yield break;
    }

    public virtual void GetTargets()
    {
        //Purely to get overidden. 
    }

    public virtual void AddTarget(TacticsMovement unit, Tile tileToAttackFrom)
    {
        Target target = new Target
        {
            unitTargeted = unit
        };
        targets.Add(target);
    }

    public override void GetItemData()
    {
    }

    public virtual void AttackEvent() { }

    public virtual Critical CoreCritical() { return new c_DamageUp(); }

    public void EndAction()
    {
        Initiative.EndAction();
    }
}
