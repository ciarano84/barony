using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    
    //For now this will just be hitting or missing (no crits). 
    public static void AttackRoll(Unit attacker, Unit defender)
    {
        AbilityCheck check = new AbilityCheck();

        check.CheckAbility(attacker.attackModifier, defender.defendModifier);

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

    public static void DamageRoll(Unit attacker, Unit defender)
    {
        AbilityCheck check = new AbilityCheck();

        check.CheckAbility(attacker.damageModifier, defender.Resiliance);

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
