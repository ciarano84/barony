﻿using System.Collections;
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

    public Button endTurn;

    List<Action> actions = new List<Action>();

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
        if (unitTactics.remainingMove >= unit.unitInfo.currentMove)
        { moveActionButton.SetActive(true); }
        else { moveActionButton.SetActive(false); }
        if (unitTactics.remainingActions >= 1)
        { mainActionButton.SetActive(true); }
        else { mainActionButton.SetActive(false); }

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
                        actions.Add(a);
                    }
                }

                for (int count = 0; count < actions.Count; count++)
                {
                    ActionButton actionButton = Instantiate(actionButtonPrefab).GetComponent<ActionButton>();
                    actionButton.gameObject.transform.SetParent(customMoveActions.transform, false);
                    actionButton.tooltip = tooltip.GetComponent<Tooltip>();
                    actions[count].SetActionButtonData(unit);
                    actionButton.tooltipText = actions[count].buttonText;
                    actionButton.action = actions[count];
                    actionButton.image.sprite = actions[count].SetImage();

                    //we will need to work out how and when to put things in EITHER or BOTH of main and move here. 
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
}
