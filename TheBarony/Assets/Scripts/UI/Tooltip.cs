using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public Camera cam;
    
    Text toolTipText;
    RectTransform backgroundRectTransform;

    private void Awake()
    {
        toolTipText = transform.Find("TooltipText").GetComponent<Text>();
        backgroundRectTransform = transform.Find("TooltipBackground").GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, cam, out localPoint);
        transform.localPosition = localPoint;
    }

    public void ShowToolTip(string _toolTipText)
    {
        gameObject.SetActive(true);
        toolTipText.text = _toolTipText;
        float textPadding = 4f;
        Vector2 backgroundSize = new Vector2(toolTipText.preferredWidth + (textPadding *2), toolTipText.preferredHeight + (textPadding * 2));
        backgroundRectTransform.sizeDelta = backgroundSize;
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
    }
}
