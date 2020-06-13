﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;

[System.Serializable]
public class UnitInfo
{
    public string unitName = "nobody";
    public int move = 5;
    public string className;
    public int maxBreath = 3;
    public int currentBreath = 3;
    public int Resiliance = -3;
    public int damageModifier = 0;
    public int attackModifier = 1;
    public int defendModifier = -3;
    public int wounds = 0;
    public Factions faction = Factions.players;
    //public Animator unitAnim;
    public WeaponData weaponData;
    public string unitVisual = "EnemyVisual";
}

public class Unit : MonoBehaviour
{
    public UnitInfo unitInfo; 
    public Animator unitAnim;
    public Weapon weapon1;

    //These should be chosen from "drugdge" "elite" "dangerous"
    public string fate;

    private void Start()
    {
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
            unitInfo.currentBreath = 0;
            KO();
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

        DamagePopUp.Create(gameObject.transform.position + new Vector3(0, (gameObject.GetComponent<TacticsMovement>().halfHeight) + 0.5f), "Wounded", true);
    }

    //This needs sorting out. 
    public IEnumerator KO()
    {
        Initiative.queuedActions++;
        unitInfo.currentBreath = 0;

        unitAnim.SetBool("dead", true);
        yield return new WaitForSeconds(unitAnim.GetCurrentAnimatorStateInfo(0).length);

        //Tell the initiative order to remove this unit. 
        Initiative.RemoveUnit(this);
    }
}