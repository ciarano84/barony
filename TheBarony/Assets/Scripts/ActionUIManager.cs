using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class ActionUIManager : MonoBehaviour
{
    public static TacticsMovement currentUnit;
    public Button endTurn;

    private void Start()
    {
        endTurn.gameObject.SetActive(false);
    }

    public static void UpdateActions(TacticsMovement unit)
    {
        if (unit.GetComponent<PlayerCharacter>() != null)
        {
            if (unit.remainingMove > 0 || unit.remainingActions > 0)
            {
                //need to make sure this gets turned off at some point. 
                endTurn.gameObject.SetActive(true);
            }
        }
        else {
            //This is for NPC actions, so not really needed atm.
            Clear();
            return;
        }
    }

    public void PlayerEndsTurnEarly()
    {
        Initiative.EndTurn();
    }

    public static void Clear() {
        endTurn.gameObject.SetActive(false);
        //get rid of all Action UI. 
    }
}
