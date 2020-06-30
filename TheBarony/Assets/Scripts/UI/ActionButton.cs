using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    public Action action;
    public Image image;
    public string tooltipText;
    public Tooltip tooltip;
    public ActionCost actionCost;

    public void Awake()
    {
        tooltipText = "defaultActionButtonText";
    }

    public void TriggerAction()
    {
        action.ExecuteAction(actionCost);
    }

    public void ShowTooltip()
    {
        tooltip.ShowToolTip(tooltipText);
    }
    
    public void HideTootip()
    {
        tooltip.HideToolTip();
    }

    public void RemoveSelf()
    {
        tooltip.HideToolTip();
        Destroy(gameObject);
    }
}
