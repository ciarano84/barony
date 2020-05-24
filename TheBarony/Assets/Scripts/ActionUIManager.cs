using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class ActionUIManager : MonoBehaviour
{
    public Button endTurn;
    public Button Weapon1Attack;
    Weapon weapon1;

    static Texture2D attackCursor;

    private void Start()
    {
        Initiative.onAwaitPlayerInput += UpdateActions;
        endTurn.gameObject.SetActive(false);
        attackCursor = Resources.Load<Texture2D>("Sword_Cursor");
    }

    void UpdateActions()
    {
        TacticsMovement currentUnit = Initiative.currentUnit;

        if (currentUnit.GetComponent<PlayerCharacter>() != null)
        {
            if (currentUnit.remainingMove > 0 || currentUnit.remainingActions > 0)
            {
                //need to make sure this gets turned off at some point. 
                endTurn.gameObject.SetActive(true);
            }
            else Clear();
        }
        else {
            //This is for NPC actions, so not really needed atm.
            Clear();
            return;
        }
    }

    //Called from the End turn button. 
    public void PlayerEndsTurnEarly()
    {
        Initiative.currentUnit.remainingMove = 0;
        Initiative.currentUnit.remainingActions = 0;
        Initiative.CheckForTurnEnd();
    }

    public void Clear() {
        endTurn.gameObject.SetActive(false);
        //get rid of all Action UI. 
    }

    public static void GetAttackCursor() {
        Cursor.SetCursor(attackCursor, Vector2.zero, CursorMode.Auto);
    }

    public static void SetStandardCursor() {
        Cursor.SetCursor(default, Vector2.zero, CursorMode.ForceSoftware);
    }
}
