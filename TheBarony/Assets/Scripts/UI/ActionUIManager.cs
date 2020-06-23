using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class ActionUIManager : MonoBehaviour
{
    public static PlayerCharacter currentUnit;
    public Button endTurn;
    public Button ActionButton1;
    public Text ActionButtonText1;
    public Button ActionButton2;
    public Text ActionButtonText2;
    public Button ActionButton3;
    public Text ActionButtonText3;
    public Button ActionButton4;
    public Text ActionButtonText4;

    List<Action> actions = new List<Action>();

    //Weapon mainWeapon;

    static Texture2D attackCursor;

    private void Start()
    {
        endTurn.gameObject.SetActive(false);
        attackCursor = Resources.Load<Texture2D>("Sword_Cursor");
    }

    public void UpdateActions(PlayerCharacter unit)
    {
        Clear();
        currentUnit = unit;

        if (unit.GetComponent<PlayerCharacter>() != null)
        {
            if (unit.remainingMove > 0 || unit.remainingActions > 0)
            {
                //need to make sure this gets turned off at some point. 
                endTurn.gameObject.SetActive(true);

                //Ensure conditions for actions are met. 
                foreach (Action a in unit.actions)
                {
                    if (a.CheckAvailable())
                    {
                        actions.Add(a);
                    }
                }

                for (int count = 0; count < actions.Count; count++)
                {
                    switch (count)
                    {
                        case 0:
                            ActionButton1.gameObject.SetActive(true);
                            SetActionButtonText(actions[0], ActionButtonText1);
                            break;
                        case 1:
                            ActionButton2.gameObject.SetActive(true);
                            SetActionButtonText(actions[1], ActionButtonText2);
                            break;
                        case 2:
                            ActionButton3.gameObject.SetActive(true);
                            SetActionButtonText(actions[2], ActionButtonText3);
                            break;
                        case 3:
                            ActionButton4.gameObject.SetActive(true);
                            SetActionButtonText(actions[3], ActionButtonText4);
                            break;
                        default:
                            break;
                    }
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
        ActionButton1.gameObject.SetActive(false);
        ActionButton2.gameObject.SetActive(false);
        ActionButton3.gameObject.SetActive(false);
        ActionButton4.gameObject.SetActive(false);
    }

    public static void GetAttackCursor() {
        //Testing a change here. 
        Cursor.SetCursor(GameAssets.i.Sword_Cursor, Vector2.zero, CursorMode.Auto);
    }

    public static void SetStandardCursor() {
        Cursor.SetCursor(default, Vector2.zero, CursorMode.ForceSoftware);
    }

    void SetActionButtonText(Action action, Text text)
    {
        text.text = action.buttonText;
    }

    public void ActionButton1Press()
    {
        actions[0].ExecuteAction();
    }

    public void ActionButton2Press()
    {
        actions[1].ExecuteAction();
    }

    public void ActionButton3Press()
    {
        actions[2].ExecuteAction();
    }

    public void ActionButton4Press()
    {
        actions[3].ExecuteAction();
    }
}
