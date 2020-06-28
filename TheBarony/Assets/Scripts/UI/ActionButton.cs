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

    public void Awake()
    {
        tooltipText = "defaultActionButtonText";
    }

    public void TriggerAction()
    {
        action.ExecuteAction();
    }

    public void ShowTooltip()
    {
        Debug.Log("OnMouseEnter recognised");
        tooltip.ShowToolTip(tooltipText);
    }
    
    public void HideTootip()
    {
        tooltip.HideToolTip();
    }
}
