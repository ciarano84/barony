using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class ActionUIManager : MonoBehaviour
{
    public static PlayerCharacter currentUnit;
    public Button endTurn;
    public Button Weapon1Attack;

    Queue<Action> actions = new Queue<Action>();

    Weapon currentWeapon;

    static Texture2D attackCursor;

    private void Start()
    {
        endTurn.gameObject.SetActive(false);
        attackCursor = Resources.Load<Texture2D>("Sword_Cursor");
    }

    public void UpdateActions(PlayerCharacter unit)
    {
        currentUnit = unit;

        if (unit.GetComponent<PlayerCharacter>() != null)
        {
            if (unit.remainingMove > 0 || unit.remainingActions > 0)
            {
                //need to make sure this gets turned off at some point. 
                endTurn.gameObject.SetActive(true);

                //load in all the actions and make buttons for them. 
                actions = unit.actions;
                foreach (Action action in actions)
                {
                    SetActionButton(action);
                }
            }
            else Clear();
        }
        else {
            //This is for NPC actions, so not really needed atm.
            Clear();
            return;
        }
    }

    public void PlayerEndsTurnEarly()
    {
        if (Initiative.queuedActions > 0) return;
        Initiative.EndTurn();
    }

    public void Clear() {
        endTurn.gameObject.SetActive(false);
        //get rid of all Action UI. 
        actions.Clear();
    }

    public static void GetAttackCursor() {
        //Testing a change here. 
        Cursor.SetCursor(GameAssets.i.Sword_Cursor, Vector2.zero, CursorMode.Auto);
    }

    public static void SetStandardCursor() {
        Cursor.SetCursor(default, Vector2.zero, CursorMode.ForceSoftware);
    }

    void SetActionButton(Action action)
    {
        //Create the action button. 
    }
}
