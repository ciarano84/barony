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
        int attack = attacker.unitInfo.attackModifier + attacker.unitInfo.weaponData.weaponAttack;
        int defence = defender.unitInfo.defendModifier;

        AbilityCheck.CheckAbility(attack, defence);

        if (AbilityCheck.baseResult >= 0)
        //hit goes here. 
        {
            DamageRoll(attacker, defender);
        }
        else
        //miss goes here. 
        {
            DamagePopUp.Create(defender.gameObject.transform.position + new Vector3(0, defender.gameObject.GetComponent<TacticsMovement>().halfHeight), "miss", false);
        }
    }

    //This is the ranged variation
    public static bool RangedAttackRoll(Unit attacker, Unit defender)
    {
        AbilityCheck check = new AbilityCheck();

        int attack = attacker.unitInfo.attackModifier + attacker.unitInfo.weaponData.weaponAttack;
        int defence = defender.unitInfo.defendModifier;

        AbilityCheck.CheckAbility(attack, defence);

        if (AbilityCheck.baseResult >= 0)
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
        int wounds;

        if (attacker.unitInfo.weaponData.rangeType == WeaponData.Range.ranged) { damage = attacker.unitInfo.weaponData.weaponDamage; }
        else { damage = attacker.unitInfo.damageModifier + attacker.unitInfo.weaponData.weaponDamage; }

        int resiliance = defender.unitInfo.Resiliance;

        AbilityCheck.CheckAbility(damage, resiliance);

        int result = AbilityCheck.baseResult;

        //assumes all are 'fated' for now. 
        if (result < -9)
        {
            DamagePopUp.Create(defender.gameObject.transform.position + new Vector3(0, defender.gameObject.GetComponent<TacticsMovement>().halfHeight), "harmless", false);
            return;
        }
        else if (result < 1) defender.UpdateBreath(-2);
        else
        {
            if (result > 9) wounds = 1;
            else if (result > 4) wounds = 2;
            else { wounds = 3; }
            defender.UpdateWounds(wounds);
        }
    }
}
