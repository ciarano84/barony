using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatScript : MonoBehaviour
{
    public EncounterManager encounterManager;
    
    public void Update()
    {
        //Cheat for player victory
        if (Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.P))
        {
            RostaInfo.currentEncounter.completionState = Encounter.CompletionState.VICTORY;
            StartCoroutine(encounterManager.EncounterEnd(Factions.players));
        }

        //Cheat for enemy victory
        if (Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.E))
        {
            encounterManager.playerSquad.Clear();
            RostaInfo.currentEncounter.completionState = Encounter.CompletionState.DEFEAT;
            StartCoroutine(encounterManager.EncounterEnd(Factions.enemies));
        }
    }
}
