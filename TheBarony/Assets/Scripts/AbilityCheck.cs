using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCheck //: MonoBehaviour
{
    //The basic outcome of the roll, with 0 meaning equalling the opposition, and a success. Negative means a failure and under. 
    public int baseResult = 0;
    public int crits = 0;
    public int fumbles;

    //This returns -1 for a failure. 0 for a success. and 1 for each critical beyond that. 
    //Because the last permater is set in the method declaration, it has a default and is optional. 
    public void CheckAbility(int activeModifier, int passiveModifier, int bonuses = 0)
    {
        //Work out active roll
        int activeRoll = BasicRoll(activeModifier);

        while (bonuses > 0)
        {
            int bonusRoll = Random.Range(1, 5);
            if (bonusRoll == 5)
            { crits++; }
            bonuses--;
        }

        while (bonuses < 0)
        {
            baseResult -= Random.Range(1, 5);
            bonuses++;
        }

        //Work out passive check:
        int passiveRoll = BasicRoll(passiveModifier);

        baseResult = activeRoll - passiveRoll;
    }

    int BasicRoll(int activeAbility)
    {
        int activeResult;
        int firstActiveDie = Random.Range(1, 10);
        if (firstActiveDie == 10) crits++;
        int secondActiveDie = Random.Range(1, 10);
        if (secondActiveDie == 10) crits++;
        activeResult = firstActiveDie + secondActiveDie;

        return activeResult;
    }
}
