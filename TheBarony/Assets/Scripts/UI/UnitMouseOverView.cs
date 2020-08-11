using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitMouseOverView : MonoBehaviour
{
    float offset = 1f;

    public Text unitMouseOverViewName;
    public Slider unitMouseOverViewfirstBreathSlider;
    public Slider unitMouseOverViewFlaggingBreathSlider;
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
                unitMouseOverViewName.text = targetUnit.unitInfo.unitName;

                unitMouseOverViewfirstBreathSlider.maxValue = targetUnit.unitInfo.firstBreath;
                unitMouseOverViewfirstBreathSlider.value = targetUnit.unitInfo.currentBreath - targetUnit.unitInfo.flaggingBreath;
                //unitMouseOverViewfirstBreathSlider.value = targetUnit.unitInfo.currentBreath - targetUnit.unitInfo.flaggingBreath;

                unitMouseOverViewFlaggingBreathSlider.maxValue = targetUnit.unitInfo.flaggingBreath;
                unitMouseOverViewFlaggingBreathSlider.value = targetUnit.unitInfo.currentBreath;
                //unitMouseOverViewFlaggingBreathSlider.value = targetUnit.unitInfo.currentBreath;
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
