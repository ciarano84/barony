﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class UnitInfo
{
    public string unitName = "nobody";
    public string unitVisual = "EnemyVisual";
    public Factions faction = Factions.players;
    public AspectData aspectData;
    public Aspect aspect;
    public string className;
    public WeaponData weaponData;
    public ItemData offHandData;
    public ItemData armourData;

    //Base Stats
    public int baseBreath = 3;
    public int baseAttack = 1;
    public int baseDefence = -3;
    public int baseToughness = -3;
    public int baseStrength = 0;
    public int baseMove = 4;

    //Modified Stats
    public int wounds = 0;
    public int currentBreath;
    public int currentAttack;
    public int currentDefence;
    public int currentToughness;
    public int currentDamage;
    public int currentMove;
}

public class Unit : MonoBehaviour
{
    public UnitInfo unitInfo; 
    public Animator unitAnim;
    public Weapon currentWeapon;
    //how is this being populated?  
    public List<Action> actions = new List<Action>();

    //These should be chosen from "drugdge" "elite" "dangerous"
    public string fate;

    public delegate void OnKODelegate(Unit unit);
    public static OnKODelegate onKO;

    public void SetStats()
    {
        unitInfo.currentBreath = unitInfo.baseBreath;
        unitInfo.currentAttack = unitInfo.baseAttack;
        unitInfo.currentDefence = unitInfo.baseDefence;
        unitInfo.currentToughness = unitInfo.baseToughness;
        unitInfo.currentDamage = unitInfo.baseStrength;
        unitInfo.currentMove = unitInfo.baseMove;
    }

    //was protected and not sure why. 
    public virtual void InitUnit()
    {
    }

    public void UpdateBreath(int amount)
    {
        unitInfo.currentBreath += amount;
        DamagePopUp.Create(gameObject.transform.position + new Vector3(0, (gameObject.GetComponent<TacticsMovement>().halfHeight) + 0.5f), amount.ToString(), false);

        if (unitInfo.currentBreath <= 0)
        {
            StartCoroutine("KO");
        }
    }

    public void UpdateWounds(int amount)
    {
        unitInfo.wounds += amount;
        while (amount > 0)
        {
            unitInfo.currentBreath -= 5;
            unitInfo.wounds++;
            amount--;
            if ((unitInfo.currentBreath <= 0) || (unitInfo.wounds >= 3))
            {
                StartCoroutine("KO");
            }
        }
        string woundedText;
        switch (amount)
        {   case 1:
                woundedText = "wounded";
                break;
            case 2:
                woundedText = "severely wounded";
                break;
            case 3:
                woundedText = "executed";
                break;
            default:
                woundedText = "wounded";
                break;
        }

        DamagePopUp.Create(gameObject.transform.position + new Vector3(0, (gameObject.GetComponent<TacticsMovement>().halfHeight) + 0.5f), woundedText, true);
    }

    //This needs sorting out. 
    public IEnumerator KO()
    {
        Initiative.queuedActions++;
        unitInfo.currentBreath = 0;

        unitAnim.SetBool("dead", true);
        yield return new WaitForSeconds(unitAnim.GetCurrentAnimatorStateInfo(0).length);
        onKO(this);

        //Tell the initiative order to remove this unit. 
        Initiative.RemoveUnit(this);
    }
}