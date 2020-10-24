using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : UIElement
{
    public Action action;
    public Image image;
    public ActionCost actionCost;

    public void TriggerAction()
    {
        action.ExecuteAction(actionCost);
    }
}
