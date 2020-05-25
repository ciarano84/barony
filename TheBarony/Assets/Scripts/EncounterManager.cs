using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Factions {players, enemies, allies}

public class EncounterManager : MonoBehaviour
{
    public GameObject encounterEndPanel;
    static GameObject staticEncounterPanel;

    public Text encounterEndtext;
    static Text staticEncounterEndtext;

    private void Start()
    {
        staticEncounterPanel = encounterEndPanel;
        staticEncounterEndtext = encounterEndtext;
    }

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
            EncounterEnd(Factions.players);
            return;
        }

        foreach (TacticsMovement unit in Initiative.order)
        {
            if (unit.faction == Factions.players) return;
        }
        EncounterEnd(Factions.enemies);
    }

    static void CheckForOtherWinCondition()
    { 
        //have other win conditions added as methods, then do a delegate in the start to decide which is going to get called.     
    }

    static void EncounterEnd(Factions faction)
    {
        Initiative.queuedActions++;
        staticEncounterPanel.SetActive(true);
        staticEncounterEndtext.text = (faction + " are victorious!");
    }
}
