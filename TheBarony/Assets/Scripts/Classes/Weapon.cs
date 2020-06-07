using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData
{
    //As I add classes that inherit, we can get move variation. Crucially there should be a 1:1 relaionship between weapon scripts and WeaponData scripts.
    public int attackModifier = 0;
    public int damageModifier = 2;
    public string imageFile;

    public virtual void CreateWeapon(Unit unit)
    {
        //To be overidden. 
    }
}

public class Weapon : MonoBehaviour
{
    public PlayerCharacter owner;
    public WeaponData weaponData;

    public class Target
    {
        public TacticsMovement unitTargeted;
        public Tile tileToAttackFrom;
    }

    public List<Target> targets = new List<Target>();

    private void Start()
    {
        weaponData.attackModifier += owner.unitInfo.attackModifier;
        weaponData.damageModifier += owner.unitInfo.damageModifier;
    }

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
