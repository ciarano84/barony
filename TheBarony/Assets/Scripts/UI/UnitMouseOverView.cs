﻿using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitMouseOverView : MonoBehaviour
{
    float offset = 2f;

    public Text unitMouseOverViewName;
    public Slider unitMouseOverViewfirstBreathSlider;
    public Slider unitMouseOverViewFlaggingBreathSlider;
    public GameObject canvass;
    public GameObject effectHolder;
    public static Unit targetUnit;
    public CinemachineBrain cam;
    //public Camera cam;

    ActionUIManager actionUIManager;

    static bool display = false;
    static bool effectsSet;

    private void Start()
    {
        actionUIManager = GameObject.Find("Main Action Panel").GetComponent<ActionUIManager>();
    }

    private void LateUpdate()
    {
        if (display)
        {
            canvass.SetActive(true);
            if (targetUnit != null)
            {
                transform.position = targetUnit.transform.position + new Vector3(0, offset);
                transform.rotation = Quaternion.LookRotation(transform.position - cam.ActiveVirtualCamera.VirtualCameraGameObject.transform.position);
                unitMouseOverViewName.text = targetUnit.unitInfo.unitName;

                if (!effectsSet) SetEffects();

                unitMouseOverViewfirstBreathSlider.maxValue = targetUnit.unitInfo.firstBreath;
                unitMouseOverViewfirstBreathSlider.value = targetUnit.unitInfo.currentBreath - targetUnit.unitInfo.flaggingBreath;

                unitMouseOverViewFlaggingBreathSlider.maxValue = targetUnit.unitInfo.flaggingBreath;
                unitMouseOverViewFlaggingBreathSlider.value = targetUnit.unitInfo.currentBreath;
            }
            else
            {
                Hide();
                effectsSet = false;
            }   
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
        effectsSet = false;
    }

    public void SetEffects()
    {
        foreach (Transform child in effectHolder.gameObject.transform) Destroy(child.gameObject);
        foreach (Effect effect in targetUnit.effects) actionUIManager.AddEffectIcon(effect, effectHolder.transform);
        effectsSet = true;
    }
}
