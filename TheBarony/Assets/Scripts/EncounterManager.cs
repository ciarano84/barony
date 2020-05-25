using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Factions {players, enemies, allies}

public class EncounterManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            foreach (TacticsMovement unit in Initiative.order)
            {
                Debug.Log(unit + " belonging to faction " + unit.faction);
            }
        }    
    }

    public static void CheckForFactionDeath()
    {
        bool playerVictory = true;
        //checks to see if one side has wiped out the other. 
        foreach (TacticsMovement unit in Initiative.order)
        {
            if (unit.faction == Factions.enemies)
            {
                playerVictory = false;
                break;
            }
        }

        if (playerVictory) 
        { 
            OnPlayerVictory();
            return;
        }

        foreach (TacticsMovement unit in Initiative.order)
        {
            if (unit.faction == Factions.players) return;
        }
        OnEnemyVictory();
    }

    static void CheckForOtherWinCondition()
    { 
        //have other win conditions added as methods, then do a delegate in the start to decide which is going to get called.     
    }

    static void OnPlayerVictory()
    {
        Debug.Log("Player victory");
    }

    static void OnEnemyVictory()
    {
        Debug.Log("Enemy victory");
    }
}
