using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponData
{
    //As I add classes that inherit, we can get move variation. Crucially there should be a 1:1 relaionship between weapon scripts and WeaponData scripts.
    public int weaponAttack = 0;
    public int weaponDamage = 2;
    public int range;
    public string imageFile;

    public abstract void SetWeaponData();
    public abstract void CreateWeapon(Unit unit);
}

public class Weapon : MonoBehaviour
{
    public PlayerCharacter owner;
    public WeaponData weaponData;
    public int actionsPerAttack;

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
}
