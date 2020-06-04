﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RostaManager : MonoBehaviour
{
    //this class is totally borked as it's based on having game objects. Need to update it to use player data. 
    
    static List<UnitInfo> rosta = new List<UnitInfo>();
    static int currentTroopShown = 1;

    public delegate void NewUnitShown();
    public static NewUnitShown newUnitShown;
    static Vector3 pedestalPosition;
    public GameObject pedestal;
    public GameObject hidden;

    enum Direction {left, right};

    public Text nameText;
    public Text classText;
    public Text breathText;
    public Text attackText;
    public Text defenceText;
    public Text damageText;
    public Text armourText;
    public Text speedText;

    private void Start()
    {
        pedestalPosition = pedestal.transform.position;
        RostaManager.newUnitShown += ShowStats;
    }

    // This brings in the rosta and is currently called by the proxy script. 
    public static void BringInRosta()
    {
        foreach (UnitInfo go in ProxyRosta.proxyRosta)
        {
            rosta.Add(go);
            //go.gameObject.SetActive(false);
            //go.gameObject.transform.Find("Visual").gameObject.transform.position = pedestalPosition;
        }
        ShowUnit(Direction.right);
    }

    public void ShowStats()
    {
        nameText.text = (rosta[currentTroopShown].unitName);
        classText.text = (rosta[currentTroopShown].className);
        breathText.text = (rosta[currentTroopShown].maxBreath.ToString());
        attackText.text = (rosta[currentTroopShown].attackModifier.ToString());
        defenceText.text = (rosta[currentTroopShown].defendModifier.ToString());
        damageText.text = (rosta[currentTroopShown].damageModifier.ToString());
        armourText.text = (rosta[currentTroopShown].Resiliance.ToString());
        speedText.text = (rosta[currentTroopShown].ToString());
    }

    public void OnRightButtonClick() { GetNextsUnit(); }
    public static void GetNextsUnit()
    {
        ShowUnit(Direction.right);
    }

    public void OnLeftButtonClick() { GetPreviousUnit(); }
    public static void GetPreviousUnit()
    {
        ShowUnit(Direction.left);
    }

    static void ShowUnit(Direction direction)
    {
        //rosta[currentTroopShown].gameObject.SetActive(false);

        if (direction == Direction.right)
        {
            currentTroopShown++;
            if (currentTroopShown >= rosta.Count)
            {
                currentTroopShown = 0;
            }
        }
        else
        {
            currentTroopShown--;
            if (currentTroopShown < 0)
            {
                currentTroopShown = (rosta.Count - 1);
            }
        }

        //rosta[currentTroopShown].gameObject.SetActive(true);

        //get the delegate to call subscribers. 
        newUnitShown();
    }
}
