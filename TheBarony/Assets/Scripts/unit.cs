﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Fate { Drudge, Elite, Fated };

[System.Serializable]
public class UnitInfo
{
    public string unitName = "goblin";
    public string unitVisual = "EnemyVisual";
    public Factions faction = Factions.enemies;
    public AspectData aspectData;
    public Aspect aspect;
    public WeaponData mainWeaponData;
    public ItemData offHandData;
    public ItemData armourData;
    public ItemData accessory1;
    public ItemData accessory2;
    public Fate fate = Fate.Fated;

    //Base Stats
    public int baseBreath;
    public int firstBreath = 3;
    public int flaggingBreath = 0;
    public int baseAttack = 1;
    public int baseDefence = -3;
    public int baseToughness = -3;
    public int baseStrength = 0;
    public int baseMove = 4;
    public int baseArmour = 0;

    //Modified Stats
    public int wounds = 0;
    public int currentBreath;
    public int currentAttack;
    public int currentDefence;
    public int currentToughness;
    public int currentDamage;
    public int currentMove;
    public int currentArmour = 0;

    //conditions 
    public bool flagging;

    public UnitInfo()
    {
        baseBreath = firstBreath + flaggingBreath;
    }
}

public class Unit : MonoBehaviour
{
    public UnitInfo unitInfo;
    public Animator unitAnim;
    public GameObject rig;
    public Weapon mainWeapon;
    public GameObject body;

    //slots
    public Transform mainHandSlot;
    public Transform offHandSlot;

    //tracks if they have used a focus switch in their turn. 
    public bool focusSwitched = false;
    public bool canFocusSwitch = false;
    bool focusSinceStart;

    public bool aimingBow;

    public List<Action> actions = new List<Action>();
    public List<Effect> effects = new List<Effect>();

    public List<Unit> adjacentUnits = new List<Unit>();

    public delegate void OnKODelegate(Unit unit);
    public static OnKODelegate onKO;

    public delegate void OnSetFocusDelegate(Unit unit);
    public static OnSetFocusDelegate onSetFocus;

    //focus
    public Unit focus;
    public static Unit mousedOverUnit;

    //Actions
    public Action dash;
    public Action defend;
    public Action prime;

    //defence - unts are assumed to be blocking if they are not dodging. By default they are blocking. 
    //public bool dodge;
    public DefenceType defenceType = DefenceType.BLOCK;

    public void SetStats()
    {
        unitInfo.currentBreath = unitInfo.baseBreath;
        unitInfo.currentAttack = unitInfo.baseAttack;
        unitInfo.currentDefence = unitInfo.baseDefence;
        unitInfo.currentToughness = unitInfo.baseToughness;
        unitInfo.currentDamage = unitInfo.baseStrength;
        unitInfo.currentMove = unitInfo.baseMove;
        unitInfo.currentArmour = unitInfo.baseArmour;
    }

    public void SetActions()
    {
        //Dash 
        Dash da = new Dash();
        da.SetActionButtonData(this);
        actions.Add(da);
        dash = da;

        //Defend
        Defend df = new Defend();
        df.SetActionButtonData(this);
        actions.Add(df);
        defend = df;

        //Prime
        Prime pr = new Prime();
        pr.SetActionButtonData(this);
        actions.Add(pr);
        prime = pr;
    }

    public void SetSlots()
    {
        mainHandSlot = rig.GetComponent<UnitBody>().mainHandSlot;
        offHandSlot = rig.GetComponent<UnitBody>().offHandSlot;
    }

    public virtual void InitUnit()
    {
    }

    public void UpdateBreath(int amount, bool _wear = false)
    {
        if (amount < 0)
        {
            if (unitInfo.currentBreath == 0)
            {
                if (_wear)
                {
                    return;
                }
                else
                {
                    //DamagePopUp.Create(gameObject.transform.position + new Vector3(0, (gameObject.GetComponent<TacticsMovement>().halfHeight) + 0.5f), "Knocked out", false);
                    GetComponent<UnitPopUpManager>().AddPopUpInfo("Knocked out");

                    Debug.Log("KO called from UpdateBreath.");
                    StartCoroutine("KO");
                }
            }
        }

        unitInfo.currentBreath += amount;
        //DamagePopUp.Create(gameObject.transform.position + new Vector3(0, (gameObject.GetComponent<TacticsMovement>().halfHeight) + 0.5f), amount.ToString(), false);
        GetComponent<UnitPopUpManager>().AddPopUpInfo("breath " + amount.ToString());

        //check for flagging
        if (unitInfo.currentBreath <= unitInfo.flaggingBreath)
        {
            if (!unitInfo.flagging)
            {
                unitInfo.flagging = true;
                //DamagePopUp.Create(gameObject.transform.position + new Vector3(0, (gameObject.GetComponent<TacticsMovement>().halfHeight) + 0.5f), "flagging", false);
                GetComponent<UnitPopUpManager>().AddPopUpInfo("flagging");
            }
        }

        //ensure breath doesn't go beneath 0 and trigger KO
        if (unitInfo.currentBreath <= 0)
        {
            unitInfo.currentBreath = 0;
        }
    }

    public void UpdateWounds(int amount, int woundValueAdjustment = 0)
    {
        //Sort the popup message.
        string woundedText;
        switch (amount)
        {
            case 1:
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
        GetComponent<UnitPopUpManager>().AddPopUpInfo(woundedText, true);

        while (amount > 0)
        {
            unitInfo.currentBreath -= (5 + woundValueAdjustment);
            unitInfo.wounds++;
            amount--;
            if ((unitInfo.currentBreath <= 0) || (unitInfo.wounds >= 3))
            {
                unitInfo.currentBreath = 0;

                Debug.Log("KO called from UpdateWounds.");
                StartCoroutine("KO");
            }
        }
    }

    //This needs sorting out. 
    public IEnumerator KO()
    {
        Initiative.queuedActions++;
        CombatLog.UpdateCombatLog(name + " KOed.");
        onKO(this);

        unitAnim.SetBool("dead", true);
        yield return new WaitForSeconds(unitAnim.GetCurrentAnimatorStateInfo(0).length + 2);

        gameObject.GetComponent<TacticsMovement>().currentTile.occupant = null;
        //Tell the initiative order to remove this unit. 
        Initiative.RemoveUnit(this);
    }

    public void AutoSetFocus()
    {
        if (focus == null)
        {
            float maxDistance = Mathf.Infinity;
            foreach (TacticsMovement t in Initiative.order)
            {
                if (t.unitInfo.faction != this.unitInfo.faction)
                {
                    if (RangeFinder.LineOfSight(this, t))
                    {
                        //this finds the nearest
                        if (Vector3.Distance(transform.position, t.transform.position) < maxDistance)
                        {
                            maxDistance = Vector3.Distance(transform.position, t.transform.position);
                            SetFocus(t);
                        }
                    }
                }
            }
        }
    }

    public void SetFocus(Unit unit)
    {
        focus = unit;
        focusSwitched = true;
        canFocusSwitch = false;
        focusSinceStart = false;
        GetComponent<TacticsMovement>().FaceDirection(unit.transform.position);
        onSetFocus(this);
    }

    public void CheckFocus(bool removalCheck)
    {
        if (focus != null)
        {
            if (removalCheck)
            {
                if (!RangeFinder.LineOfSight(this, focus))
                {
                    if (focusSinceStart)
                    {
                        focus = null;
                        focusSinceStart = false;
                    }
                }
            }
            else
            {
                focusSinceStart = true;
            }
        }
    }
}


