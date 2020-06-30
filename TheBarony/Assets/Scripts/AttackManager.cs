﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    //The values that can be manipulated by other classes. 
    public static int grazeDamage = -2;

    //These are the delegate handlers that class features and items can subscribe to. 
    public delegate void OnGrazeDelegate(Unit attacker, Unit defender);
    public static OnGrazeDelegate OnGraze;


    //For now this will just be hitting or missing (no crits).
    //Crits and detailed attack data should probably be stored as variables on the attack manager, or even into seperate 'attackData' classes that are per attack. s
    public static bool AttackRoll(Unit attacker, Unit defender, int bonuses = 0)
    {
        ResetValues();
        
        int attack = attacker.unitInfo.currentAttack;
        int defence = defender.unitInfo.currentDefence;

        //check conditions
        if (defender.unitInfo.flagging) { bonuses++; }

        //check for weight
        if (attacker.unitInfo.mainWeaponData.weight == ItemData.Weight.medium)
        {
            if (attacker.unitInfo.mainWeaponData.weight >= ItemData.Weight.heavy) bonuses--;
            attacker.UpdateBreath(-1, true);
        }

        AbilityCheck.CheckAbility(attack, defence, bonuses);

        if (AbilityCheck.baseResult >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void DamageRoll(Unit attacker, Unit defender)
    {
        int damage;
        int resiliance;
        int wounds;

        damage = attacker.unitInfo.currentDamage;
        resiliance = defender.unitInfo.currentToughness;

        AbilityCheck.CheckAbility(damage, resiliance);

        int result = AbilityCheck.baseResult;

        //assumes all are 'fated' for now. 
        if (result < -9)
        {
            DamagePopUp.Create(defender.gameObject.transform.position + new Vector3(0, defender.gameObject.GetComponent<TacticsMovement>().halfHeight), "shrugged", false);
            return;
        }
        else if (result < 1)
        {
            OnGraze(attacker, defender); //Alert all that someone is grazed. 
            defender.UpdateBreath(grazeDamage);
        } 
        else
        {
            if (result > 9) wounds = 1;
            else if (result > 4) wounds = 2;
            else { wounds = 3; }
            defender.UpdateWounds(wounds);
        }
    }

    private static void ResetValues()
    {
        grazeDamage = -2;
    }
}
