using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIElement : MonoBehaviour
{
    public string tooltipText;
    Tooltip tooltip;
    ActionUIManager UIManager;

    public void Awake()
    {
        UIManager = GameObject.Find("Main Action Panel").GetComponent<ActionUIManager>();
        tooltip = UIManager.tooltip.GetComponent<Tooltip>();
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
