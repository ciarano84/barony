using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActionUIManager : MonoBehaviour
{
    public static Unit currentUnit;
    public Text unitName;
    public Image weaponImage;
    public Slider firstBreathSlider;
    public Slider flaggingBreathSlider;
    public GameObject statusEffects;
    public GameObject effectIcon;

    public GameObject mainActionButton;
    public GameObject moveActionButton;
    public GameObject actionButtonPrefab;
    public GameObject customMainActions;
    public GameObject customMoveActions;

    public GameObject tooltip;

    public GameObject focusSwitch;
    public GameObject focusActiveIndicator;
    public Button endTurn;
    public Button focusButton;
    public Button defenceToggle;
    public Image defenceToggleIcon;
    static bool focusBeingSelected = false;

    bool moveAvailable;
    bool mainAvailable;

    //List<Action> actions = new List<Action>();
    List<Action> moveActions = new List<Action>();
    List<Action> mainActions = new List<Action>();

    static Texture2D attackCursor;

    private void Start()
    {
        endTurn.gameObject.SetActive(false);
        focusButton.gameObject.SetActive(false);
        tooltip.SetActive(false);
    }

    public void UpdateActions(TacticsMovement unit)
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

        foreach (Effect effect in unit.effects) AddEffectIcon(effect, statusEffects.transform);

        if (unitTactics.remainingMove >= unit.unitInfo.currentMove) moveAvailable = true;
        else moveAvailable = false;
        if (unitTactics.remainingActions >= 1) mainAvailable = true;
        else mainAvailable = false;

        if (moveAvailable) moveActionButton.SetActive(true);
        else moveActionButton.SetActive(false);

        if (mainAvailable) mainActionButton.SetActive(true);
        else mainActionButton.SetActive(false);

        focusBeingSelected = false;
        if (!currentUnit.focusSwitched)
        {
            focusSwitch.gameObject.SetActive(true);
            focusActiveIndicator.SetActive(false);
        }
        else focusSwitch.gameObject.SetActive(false);

        if (unit != null && unit.unitInfo.faction == Factions.players)
        {
            if (unit.remainingMove > 0 || unit.remainingActions > 0)
            {
                endTurn.gameObject.SetActive(true);
                focusButton.gameObject.SetActive(true);
                defenceToggle.gameObject.SetActive(true);
                DefenceTypeSet(true);

                //Ensure conditions for actions are met. 
                foreach (Action a in unit.actions)
                {
                    if (a.CheckAvailable())
                    {
                        if (moveAvailable)
                        {
                            if (a.actionCost == ActionCost.move)
                            {
                                moveActions.Add(a);
                            }
                        }
                        if (mainAvailable)
                        {
                            if (a.actionCost == ActionCost.main || a.actionCost == ActionCost.move)
                            {
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
        else
        {
            //This is for NPC actions, so not really needed atm.
            Clear();
            defenceToggle.gameObject.SetActive(false);
            return;
        }
    }

    public void PlayerEndsTurnEarly()
    {
        if (Initiative.queuedActions > 0) return;
        Initiative.EndTurn();
    }

    public void Clear()
    {
        endTurn.gameObject.SetActive(false);
        focusButton.gameObject.SetActive(false);
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
        foreach (Transform t in statusEffects.transform)
        {
            Destroy(t.gameObject);
        }
    }

    public static void SetCursor()
    {
        if (TacticsMovement.mousedOverUnit != null)
        {
            UnitMouseOverView.Display(TacticsMovement.mousedOverUnit.GetComponent<TacticsMovement>());
            if (focusBeingSelected)
            {
                if (RangeFinder.LineOfSight(currentUnit, TacticsMovement.mousedOverUnit) == true)
                {
                    Cursor.SetCursor(GameAssets.i.Eye_Cursor, Vector2.zero, CursorMode.Auto);
                }
            }
            else if ((Initiative.currentUnit.remainingActions > 0) && (!Initiative.currentUnit.moving))
            {
                foreach (Weapon.Target target in Initiative.currentUnit.GetComponent<TacticsMovement>().mainWeapon.targets)
                {

                    if (target.unitTargeted == TacticsMovement.mousedOverUnit)
                    {
                        Cursor.SetCursor(GameAssets.i.Sword_Cursor, Vector2.zero, CursorMode.Auto);
                    }
                }
            }
            else
            {
                Cursor.SetCursor(default, Vector2.zero, CursorMode.ForceSoftware);
            }
        }
        else 
        { 
            UnitMouseOverView.Hide();
            Cursor.SetCursor(default, Vector2.zero, CursorMode.ForceSoftware);
        }
    }

    public void AddActionButton(List<Action> list, int count, GameObject _parent, ActionCost _actionCost)
    {
        ActionButton actionButton = Instantiate(actionButtonPrefab).GetComponent<ActionButton>();
        actionButton.gameObject.transform.SetParent(_parent.transform, false);
        actionButton.tooltip = tooltip.GetComponent<Tooltip>();
        actionButton.actionCost = _actionCost;
        actionButton.tooltipText = list[count].buttonText;
        actionButton.action = list[count];
        actionButton.image.sprite = list[count].SetImage();
    }

    public void FocusButton()
    {
        if (!focusBeingSelected)
        {
            //turn it red.
            focusActiveIndicator.SetActive(true);
            //turn the mouseover icon to the eye.
            currentUnit.canFocusSwitch = true;
        }

        if (!focusBeingSelected) focusBeingSelected = true;
    }

    //the POINT of this (no idea if it will work) is to always de-activate the focus select button if something else in the UI is clicked on. 
    //not sure if it's even getting used. 
    void CheckMouse()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (focusBeingSelected)
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    FocusButton();
                }
            }
        }
    }

    public void AddEffectIcon(Effect effect, Transform parent)
    {
        EffectIcon icon = Instantiate(effectIcon).GetComponent<EffectIcon>();
        icon.image.sprite = effect.SetImage();
        icon.gameObject.transform.SetParent(parent.transform, false);
    }

    public void DefenceTypeSet(bool defaultSetting = false)
    {
        if (!defaultSetting) currentUnit.dodge = !currentUnit.dodge;
        if (currentUnit.dodge) defenceToggleIcon.sprite = GameAssets.i.DodgeToggle;
        if (!currentUnit.dodge) defenceToggleIcon.sprite = GameAssets.i.BlockToggle;
    }
}
