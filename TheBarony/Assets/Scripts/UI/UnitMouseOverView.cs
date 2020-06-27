using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitMouseOverView : MonoBehaviour
{
    float offset = 1f;

    public Text unitMouseOverViewName;
    public Slider unitMouseOverViewfirstBreathSlider;
    public GameObject canvass;
    public static Unit targetUnit;
    public Camera cam;

    static bool display = false;

    private void LateUpdate()
    {
        if (display)
        {
            canvass.SetActive(true);
            if (targetUnit != null)
            {
                transform.position = targetUnit.transform.position + new Vector3(0, offset);
                transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
                unitMouseOverViewfirstBreathSlider.value = targetUnit.unitInfo.currentBreath;
                unitMouseOverViewName.text = targetUnit.unitInfo.unitName;
                unitMouseOverViewfirstBreathSlider.maxValue = targetUnit.unitInfo.baseBreath;
            }
            else Hide();
        }
        else canvass.SetActive(false);
    }

    public static void Display(TacticsMovement unit)
    {
        display = true;
        targetUnit = unit;
    }

    public static void Hide()
    {
        display = false;
    }
}
