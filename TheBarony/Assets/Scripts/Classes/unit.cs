using System.Collections;
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
    public int maxBreath;
    public int currentBreath;
    public int Resiliance;
    public int damageModifier;
    public int attackModifier;
    public int defendModifier;
    public int wounds;
    public Factions faction = Factions.players;
    //public Animator unitAnim;
    public WeaponData weaponData;
    public string unitVisual = "PlayerVisual";
}

public class Unit : MonoBehaviour
{
    public UnitInfo unitInfo; // = new UnitInfo();
    public Animator unitAnim;
    public Weapon weapon1;

    //These should be chosen from "drugdge" "elite" "dangerous"
    public string fate;

    //was protected and not sure why. 
    public virtual void InitUnit()
    {
        unitInfo = new UnitInfo();
        unitInfo.weaponData.CreateWeapon(this);
    }

    public void UpdateBreath(int amount)
    {
        unitInfo.currentBreath += amount;
        if (unitInfo.currentBreath < 0)
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