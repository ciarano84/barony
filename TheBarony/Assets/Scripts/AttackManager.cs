using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    //For now this will just be hitting or missing (no crits).
    //Crits and detailed attack data should probably be stored as variables on the attack manager, or even into seperate 'attackData' classes that are per attack. 
    
    //This is the default and generally used for melee. 
    public static void AttackRoll(Unit attacker, Unit defender)
    {
        AbilityCheck check = new AbilityCheck();

        int attack = attacker.unitInfo.attackModifier + attacker.unitInfo.weaponData.weaponAttack;
        int defence = defender.unitInfo.defendModifier;

        check.CheckAbility(attack, defence);

        if (check.baseResult >= 0)
        //hit goes here. 
        {
            DamageRoll(attacker, defender);
        }
        else
        //miss goes here. 
        {

        }
    }

    //This is the ranged variation
    public static bool RangedAttackRoll(Unit attacker, Unit defender)
    {
        AbilityCheck check = new AbilityCheck();

        int attack = attacker.unitInfo.attackModifier + attacker.unitInfo.weaponData.weaponAttack;
        int defence = defender.unitInfo.defendModifier;

        check.CheckAbility(attack, defence);

        if (check.baseResult >= 0)
        //hit goes here. 
        {
            return true;
        }
        else
        //miss goes here. 
        {
            return false;
        }
    }

    public static void DamageRoll(Unit attacker, Unit defender)
    {
        AbilityCheck check = new AbilityCheck();

        int damage;

        if (attacker.unitInfo.weaponData.rangeType == WeaponData.Range.ranged) { damage = attacker.unitInfo.weaponData.weaponDamage; }
        else {damage = attacker.unitInfo.damageModifier + attacker.unitInfo.weaponData.weaponDamage; }

        int resiliance = defender.unitInfo.Resiliance;

        check.CheckAbility(damage, resiliance);

        int result = check.baseResult;

        //assumes all are 'fated' for now. 
        if (result < -9) return;
        else if (result < 1) defender.UpdateBreath(-2);
        else
        {
            if (result > 9) defender.UpdateWounds(3);
            else if (result > 4) defender.UpdateWounds(2);
            else { defender.UpdateWounds(1); }
        }
    }
}
