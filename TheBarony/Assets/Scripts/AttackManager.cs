using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum Result { FAIL, PARTIAL, SUCCESS };
public enum DefenceType { SHIELD, BLOCK, DODGE, EXPOSED };

public class AttackManager : MonoBehaviour
{
    //The values that can be manipulated by other classes. 
    public static Unit attacker;
    public static Unit defender;
    public static int grazeDamage = -2;
    public static int damage = 0;
    public static int bonuses = 0;
    public static int attack = 0;
    public static int defence = 0;
    public static int resiliance = 0;
    public static int wounds = 0;
    public static int blockDice = 0;
    public static int woundValueAdjustment = 0;
    public static bool armourPierce = false;
    public static bool autocrit = false;

    //Debug
    public static bool attackRolled = false;

    public enum StruckAnimation { SHIELD, BLOCK, DODGE, GRAZE, WOUND, EVADE };
    public static StruckAnimation struckAnimation;
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
    public static Result AttackRoll(Unit _attacker, Unit _defender, int _bonuses = 0)
    {
        ResetValues();

        attacker = _attacker;
        defender = _defender;
        
        attack += attacker.unitInfo.currentAttack;
        bonuses += _bonuses;

        OnAttack(attacker, defender);

        DecideDefence(attacker, defender);

        //check conditions
        if (defender.unitInfo.flagging) { bonuses++; }

        //check focus
        if (attacker.focus != defender)
        {
            attacker.gameObject.GetComponent<UnitPopUpManager>().AddPopUpInfo("Astray");
            bonuses--;
            attacker.focus = defender;
            attacker.focusSwitched = true;
        } else if (defender.focus != attacker)
        {
            attacker.gameObject.GetComponent<UnitPopUpManager>().AddPopUpInfo("Blindsiding");
            bonuses++;
        }

        //check for weight
        if (attacker.unitInfo.mainWeaponData.weight >= ItemData.Weight.medium)
        {
            attacker.UpdateBreath(-1, true);
            if (attacker.unitInfo.mainWeaponData.weight > ItemData.Weight.medium) damage += (int)attacker.unitInfo.mainWeaponData.weight;
        }

        AbilityCheck.CheckAbility(attack, defence, bonuses);

        attackRolled = true;

        //Work out criticals
        CriticalManager.Reset();

        if (autocrit) AbilityCheck.crits = Mathf.Max(AbilityCheck.crits, 1);

        for (int count = 0; count < AbilityCheck.crits; count++)
        {
            CriticalManager.AddCritical(attacker.mainWeapon);
        }

        //Resolve all pre-damage criticals. 
        foreach (Critical c in CriticalManager.criticalChain)
        {
            if (c.AfterDamage() == false) c.CriticalEffect();    
        }

        //Work out the result
        if (AbilityCheck.baseResult >= 0)
        {
            CombatLog.UpdateCombatLog(attacker.name + " attacks " + defender.name + " and scores a success.");
            return Result.SUCCESS;
        }
        if (AbilityCheck.baseResult >= -9)
        {
            if (defenceType == DefenceType.DODGE) 
            {
                defender.GetComponent<TacticsMovement>().Dodge(Result.PARTIAL);
                defender.gameObject.GetComponent<UnitPopUpManager>().AddPopUpInfo("dodged");
            }
            if (defender.focus != attacker) defender.SetFocus(attacker);
            CombatLog.UpdateCombatLog(attacker.name + " attacks " + defender.name + " and scores a partial.");
            return Result.PARTIAL;
        }
        else
        {
            if (defenceType == DefenceType.DODGE)
            {
                defender.GetComponent<TacticsMovement>().Dodge(Result.FAIL);
                defender.gameObject.GetComponent<UnitPopUpManager>().AddPopUpInfo("evaded");
            }
            defender.gameObject.GetComponent<UnitPopUpManager>().AddPopUpInfo("miss");
            CombatLog.UpdateCombatLog(attacker.name + " attacks " + defender.name + " and scores a fail.");
            return Result.FAIL;
        }
    }

    public static void DamageRoll(TacticsMovement attacker, Unit defender, Result attackResult)
    {
        damage += attacker.unitInfo.currentDamage;
        resiliance += defender.unitInfo.currentToughness;
        if (!armourPierce) resiliance += defender.unitInfo.currentArmour; 

        //Blocking
        if (defenceType == DefenceType.BLOCK)
        {
            blockDice = -1;
            defender.gameObject.GetComponent<UnitPopUpManager>().AddPopUpInfo("blocked");
        }
        if (defenceType == DefenceType.SHIELD)
        {
            blockDice = -2;
            defender.gameObject.GetComponent<UnitPopUpManager>().AddPopUpInfo("shielded");
        }

        AbilityCheck.CheckAbility(damage, resiliance, blockDice);
        int result = AbilityCheck.baseResult;
        //assumes all are 'fated' for now. 
        if (result < -9)
        {
            defender.gameObject.GetComponent<UnitPopUpManager>().AddPopUpInfo("shrugged");
            return;
        }
        else if (result < 1)
        {
            OnGraze(attacker, defender); //Alert all that someone is grazed. 
            defender.UpdateBreath(grazeDamage);
            struckAnimation = StruckAnimation.GRAZE;
        }
        else
        {
            OnWound(attacker, defender); //Alert all that someone is wounded. 
            if (result > 9) wounds = 3;
            else if (result > 4) wounds = 2;
            else { wounds = 1; }
            defender.UpdateWounds(wounds, woundValueAdjustment);
            struckAnimation = StruckAnimation.WOUND;
        }

        //Resolve all post-damage criticals. 
        foreach (Critical c in CriticalManager.criticalChain)
        {
            if (c.AfterDamage() == true) c.CriticalEffect();  
        }

        switch (struckAnimation)
        {
            case StruckAnimation.SHIELD:
                defender.unitAnim.SetTrigger("shield");
                break;
            case StruckAnimation.BLOCK:
                defender.unitAnim.SetTrigger("block");
                break;
            case StruckAnimation.GRAZE:
                defender.unitAnim.SetTrigger("graze");
                break;
            case StruckAnimation.WOUND:
                defender.unitAnim.SetTrigger("wound");
                break;
            default:
                break;
        }

        if (defender.focus != attacker)
        {
            defender.SetFocus(attacker);
        }
    }


    //This needs to set the defence and the defence type. 
    static void DecideDefence(Unit attacker, Unit defender)
    {
        defence = defender.unitInfo.currentDefence;

        if (defender.defenceType == DefenceType.BLOCK)
        {
            if (defender.GetComponent<Shield>() != null)
            {
                defenceType = DefenceType.SHIELD;
                struckAnimation = StruckAnimation.SHIELD;
                ShieldData data = (ShieldData)defender.GetComponent<Shield>().itemData;
                defence += data.shieldModifier;
                return;
            }
            else
            {
                if (defender.GetComponent<MeleeWeapon>() != null)
                {
                    if (defender.GetComponent<MeleeWeapon>().weaponData.weight > ItemData.Weight.light)
                    {
                        if (attacker.mainWeapon.weaponData.rangeType != WeaponData.Range.ranged)
                        {
                            defenceType = DefenceType.BLOCK;
                            struckAnimation = StruckAnimation.BLOCK;
                            //block anim.
                            return;
                        }
                    }
                }
            }
        }
        if (defender.defenceType != DefenceType.EXPOSED)
        {
            //is there a square to dodge to? 
            Tile dodgeTile = RangeFinder.FindTileToDodgeTo(defender.GetComponent<TacticsMovement>(), attacker, RangeFinder.FindDirection(defender.gameObject.transform, attacker.gameObject.transform));
            if (dodgeTile != null)
            {
                float heightOffset = dodgeTile.transform.position.y - defender.GetComponent<TacticsMovement>().currentTile.transform.position.y;
                defender.GetComponent<TacticsMovement>().dodgeTarget = new Vector3(dodgeTile.gameObject.transform.position.x, defender.transform.position.y + heightOffset, dodgeTile.gameObject.transform.position.z);
                defenceType = DefenceType.DODGE;
                return;
            }
        }
        //This catchall currently treats everything that doesn't fit the above as exposed. 
        bonuses++;
        defenceType = DefenceType.EXPOSED;
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
        struckAnimation = StruckAnimation.DODGE;
        armourPierce = false;
        autocrit = false;

        //Debug
        attackRolled = false;
    }
}
