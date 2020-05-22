using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int maxBreath;
    public int currentBreath;
    public int Resiliance;
    public int damageModifier;
    public int attackModifier;
    public int defendModifier;
    public int wounds;
    
    //These should be chosen from "drugdge" "elite" "dangerous"
    public string fate;
    public Weapon weapon1;

    void BeginTurn()
    { 
    
    }

    public void UpdateBreath(int amount)
    {
        currentBreath += amount;
        if (currentBreath < 0)
        {
            currentBreath = 0;
            KO();
        }
    }

    public void UpdateWounds(int amount)
    {
        wounds += amount;
        while (amount > 0)
        {
            currentBreath -= 5;
            wounds++;
            amount--;
            if ((currentBreath <= 0) || (wounds >= 3))
            {
                KO();
            }
        }
    }

    //Debugging
    public void KO()
    {
        currentBreath = 0;
        GetComponent<Renderer>().material.color = Color.black;
    }
}
