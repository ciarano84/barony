using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityChecker : MonoBehaviour
{
    //involves no fumbles yet. 

    public static int CheckAbility(int activeAbility, int difficulty)
    {
        int result;

        int firstDie = Random.Range(1, 11);
        int secondDie = Random.Range(1, 11);

        if ((firstDie + secondDie) + activeAbility >= difficulty)
        {
            result = 0;
            if (secondDie == 10)
            { result += 1; }
            if (firstDie == 10)
            { result += 1; }
        }
        else result = -1;
        return result;
    }

    //Write the overload for bonuses. negative = rolling penalty die, positive = rolling bonus dice. 
    public static int CheckAbility(int activeAbility, int difficulty, int bonuses)
    {
        int result = 0;
        int bonusResult = 0;
        int bonusCrits = 0;

        int firstDie = Random.Range(1, 10);
        int secondDie = Random.Range(1, 10);

        while (bonuses > 0)
        {
            bonusResult += Random.Range(1, 5);
            if (bonusResult == 5)
            { bonusCrits++; }
            bonuses--;
        }

        while (bonuses < 0)
        {
            bonusResult -= Random.Range(1, 5);
            bonuses++;
        }

        if ((firstDie + secondDie) + activeAbility >= difficulty)
        {
            if (secondDie == 10)
            { result += 1; }
            if (firstDie == 10)
            { result += 1; }
            result += bonusCrits;
        }
        else result = -1;

        //Debug data
        //Debug.Log("First Dice is " + firstDie);
        //Debug.Log("Second Dice is " + secondDie);
        //wDebug.Log("result (including + 2) is " + (result));

        return result;
    }
}
