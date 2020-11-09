using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCheck
{
    //The basic outcome of the roll, with 0 meaning equalling the opposition, and a success. Negative means a failure and under. 
    public static int baseResult = 0;
    public static int crits = 0;
    public static int fumbles;

    //This returns -1 for a failure. 0 for a success. and 1 for each critical beyond that. 
    //Because the last permater is set in the method declaration, it has a default and is optional. 
    public static void CheckAbility(int activeModifier, int passiveModifier, int bonuses = 0)
    {
        Reset();

        //if (!AttackManager.attackRolled) Debug.Log("Ability Roll with " + bonuses + " bonuses!");

        //Work out active roll
        int activeRoll = BasicRoll(activeModifier);

        while (bonuses > 0)
        {
            int bonusRoll = Random.Range(1, 6);
            baseResult += bonusRoll;
            if (bonusRoll == 5) { crits++; }
            //if (!AttackManager.attackRolled) Debug.Log("bonus rolled, scoring a " + bonusRoll);
            bonuses--;
        }

        while (bonuses < 0)
        {
            int penaltyRoll = Random.Range(1, 6);
            baseResult -= penaltyRoll;
            //if (!AttackManager.attackRolled) Debug.Log("penalty rolled, scoring a " + penaltyRoll);
            bonuses++;
        }

        //Work out passive check:
        int passiveRoll = BasicRoll(passiveModifier, false);

        baseResult += (activeRoll - passiveRoll);
    }

    static int BasicRoll(int stat, bool active = true)
    {
        //Doesn't appear to include fumbles atm. 
        int firstActiveDie = Random.Range(1, 11);
        if (firstActiveDie == 10 && active) crits++;
        //if (!AttackManager.attackRolled) if (active) Debug.Log("first active roll is " + firstActiveDie);
        int secondActiveDie = Random.Range(1, 11);
        if (secondActiveDie == 10 && active) crits++;
        //if (!AttackManager.attackRolled) if (active) Debug.Log("second active roll is " + secondActiveDie);
        stat += (firstActiveDie + secondActiveDie);

        return stat;
    }

    static void Reset()
    {
        baseResult = 0;
        crits = 0;
        fumbles = 0;
    }

    public static int IncrementAndLoopNumber(int _number, int _max)
    {
        _number++;
        if (_number == _max) _number = 0;
        return _number;
    }
}
