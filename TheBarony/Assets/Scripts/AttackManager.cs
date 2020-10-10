using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum Result { FAIL, PARTIAL, SUCCESS };
public enum DefenceType { SHIELD, BLOCK, DODGE };

public class AttackManager : MonoBehaviour
{
    //The values that can be manipulated by other classes. 
    public static int grazeDamage = -2;
    public static int damage = 0;
    public static int bonuses = 0;
    public static int attack = 0;
    public static int defence = 0;
    public static int resiliance = 0;
    public static int wounds = 0;
    public static int blockDice = 0;
    public static DefenceType defenceType;

    //These are the delegate handlers that class features and items can subscribe to. 
    public delegate void OnGrazeDelegate(Unit attacker, Unit defender);
    public static OnGrazeDelegate OnGraze;

    public delegate void OnWoundDelegate(Unit attacker, Unit defender);
    public static OnWoundDelegate OnWound;

    public delegate void OnAttackDelegate(Unit attacker, Unit defender);
    public static OnAttackDelegate OnAttack;

    //For now this will just be hitting or missing (no crits).
    //Crits and detailed attack data should probably be stored as variables on the attack manager, or even into seperate 'attackData' classes that are per attack. s
    public static Result AttackRoll(Unit attacker, Unit defender, int _bonuses = 0)
    {
        ResetValues();
        
        attack += attacker.unitInfo.currentAttack;
        defence += defender.unitInfo.currentDefence;
        bonuses += _bonuses;

        OnAttack(attacker, defender);

        defender.GetComponent<TacticsMovement>().Defend(attacker);

        //check conditions
        if (defender.unitInfo.flagging) { bonuses++; }

        //check focus
        if (attacker.focus != defender)
        {
            DamagePopUp.Create(attacker.gameObject.transform.position + new Vector3(0, defender.gameObject.GetComponent<TacticsMovement>().halfHeight), "Unfocused", false);
            bonuses--;
            attacker.focus = defender;
            attacker.focusSwitched = true;
        }
        if (defender.focus != attacker)
        {
            DamagePopUp.Create(attacker.gameObject.transform.position + new Vector3(0, defender.gameObject.GetComponent<TacticsMovement>().halfHeight), "Blindside", false);
            bonuses++;
        }

        //check for weight
        if (attacker.unitInfo.mainWeaponData.weight >= ItemData.Weight.medium)
        {
            if (attacker.unitInfo.mainWeaponData.weight >= ItemData.Weight.heavy) bonuses--;
            attacker.UpdateBreath(-1, true);
        }

        AbilityCheck.CheckAbility(attack, defence, bonuses);

        if (AbilityCheck.baseResult >= 0)
        {
            return Result.SUCCESS;
        }
        else if (AbilityCheck.baseResult >= -9)
        {
            return Result.PARTIAL;
        }
        else
        {
            return Result.FAIL;
        }
    }

    public static void DamageRoll(Unit attacker, Unit defender)
    {
        damage += attacker.unitInfo.currentDamage;
        resiliance = defender.unitInfo.currentToughness;

        //Blocking
        if (defenceType == DefenceType.BLOCK)
        {
            blockDice = -1;
            DamagePopUp.Create(defender.gameObject.transform.position + new Vector3(0, defender.gameObject.GetComponent<TacticsMovement>().halfHeight), "blocked", false);
        }
        if (defenceType == DefenceType.SHIELD)
        {
            blockDice = -2;
            DamagePopUp.Create(defender.gameObject.transform.position + new Vector3(0, defender.gameObject.GetComponent<TacticsMovement>().halfHeight), "shielded", false);
        }

        AbilityCheck.CheckAbility(damage, resiliance, blockDice);

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
            OnWound(attacker, defender); //Alert all that someone is wounded. 
            if (result > 9) wounds = 3;
            else if (result > 4) wounds = 2;
            else { wounds = 1; }
            defender.UpdateWounds(wounds);
        }
        if (defender.focus != attacker)
        {
            //defender.focus = attacker;
            defender.SetFocus(attacker);
        }
    }

    private static void ResetValues()
    {
        grazeDamage = -2;
        damage = 0;
        bonuses = 0;
        attack = 0;
        defence = 0;
        resiliance = 0;
        wounds = 0;
        blockDice = 0;
        defenceType = DefenceType.BLOCK;
    }
}
