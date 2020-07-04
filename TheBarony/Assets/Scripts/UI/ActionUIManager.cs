using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class ActionUIManager : MonoBehaviour
{
    public static PlayerCharacter currentUnit;
    public Text unitName;
    public Image weaponImage;
    public Slider firstBreathSlider;
    public Slider flaggingBreathSlider;

    public GameObject mainActionButton;
    public GameObject moveActionButton;
    public GameObject actionButtonPrefab;
    public GameObject customMainActions;
    public GameObject customMoveActions;

    public GameObject tooltip;

    public FocusRing TurnFocus;
    public FocusRing OutOfTurnFocus;
    public Button endTurn;

    bool moveAvailable;
    bool mainAvailable;

    //List<Action> actions = new List<Action>();
    List<Action> moveActions = new List<Action>();
    List<Action> mainActions = new List<Action>();

    static Texture2D attackCursor;

    private void Start()
    {
        endTurn.gameObject.SetActive(false);
        tooltip.SetActive(false);
    }

    public void UpdateActions(PlayerCharacter unit)
    {
        Clear();
        
        //set all the info out for the selected unit
        currentUnit = unit;
        unitName.text = unit.unitInfo.unitName;
        weaponImage.sprite = unit.unitInfo.mainWeaponData.SetImage();
        firstBreathSlider.maxValue = unit.unitInfo.firstBreath;
        firstBreathSlider.value = unit.unitInfo.currentBreath - unit.unitInfo.flaggingBreath;
        flaggingBreathSlider.maxValue = unit.unitInfo.flaggingBreath;
        flaggingBreathSlider.value = unit.unitInfo.currentBreath;
        TacticsMovement unitTactics = unit.GetComponent<TacticsMovement>();
        if (currentUnit.focus != null)
        { 
            TurnFocus.SetFocus(currentUnit, currentUnit.focus.GetComponent<TacticsMovement>());
            if (currentUnit.focus.focus != null) 
            { OutOfTurnFocus.SetFocus(currentUnit.focus.GetComponent<TacticsMovement>(), currentUnit.focus.focus.GetComponent<TacticsMovement>()); }
        }
        else { TurnFocus.NoFocus(); OutOfTurnFocus.NoFocus(); }

        if (unitTactics.remainingMove >= unit.unitInfo.currentMove) moveAvailable = true;
        else moveAvailable = false;
        if (unitTactics.remainingActions >= 1) mainAvailable = true;
        else mainAvailable = false;

        if (moveAvailable) moveActionButton.SetActive(true);
        else moveActionButton.SetActive(false);

        if (mainAvailable) mainActionButton.SetActive(true);
        else mainActionButton.SetActive(false);

        if (unit.GetComponent<PlayerCharacter>() != null)
        {
            if (unit.remainingMove > 0 || unit.remainingActions > 0)
            {
                endTurn.gameObject.SetActive(true);

                //Ensure conditions for actions are met. 
                foreach (Action a in unit.actions)
                {
                    if (a.CheckAvailable())
                    {
                        if (moveAvailable)
                        {
                            if (a.actionCost == ActionCost.move)
                            {
                                Debug.Log("adding to moveActions");
                                moveActions.Add(a);
                            }
                        }
                        if (mainAvailable)
                        {
                            if (a.actionCost == ActionCost.main || a.actionCost == ActionCost.move)
                            {
                                Debug.Log("adding to mainActions");
                                mainActions.Add(a);
                            }
                        }
                    }
                }

                for (int count = 0; count < moveActions.Count; count++)
                {
                    AddActionButton(moveActions, count, customMoveActions, ActionCost.move);
                }

                for (int count = 0; count < mainActions.Count; count++)
                {
                    AddActionButton(mainActions, count, customMainActions, ActionCost.main);
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
        moveActions.Clear();
        mainActions.Clear();
        foreach (Transform t in customMainActions.transform)
        {
            t.GetComponent<ActionButton>().RemoveSelf();
        }
        foreach (Transform t in customMoveActions.transform)
        {
            t.GetComponent<ActionButton>().RemoveSelf();
        }
    }

    public static void GetAttackCursor() {
        //Testing a change here. 
        Cursor.SetCursor(GameAssets.i.Sword_Cursor, Vector2.zero, CursorMode.Auto);
    }

    public static void SetStandardCursor() {
        Cursor.SetCursor(default, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void AddActionButton(List<Action> list, int count, GameObject _parent, ActionCost _actionCost)
    {
        ActionButton actionButton = Instantiate(actionButtonPrefab).GetComponent<ActionButton>();
        actionButton.gameObject.transform.SetParent(_parent.transform, false);
        actionButton.tooltip = tooltip.GetComponent<Tooltip>();
        //I don't think I need this line.
        //actions[count].SetActionButtonData(unit);
        actionButton.actionCost = _actionCost;
        actionButton.tooltipText = list[count].buttonText;
        actionButton.action = list[count];
        actionButton.image.sprite = list[count].SetImage();
    }
}
