using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;

[System.Serializable]
public class UnitInfo
{
    public string unitName = "nobody";
    public string className;
    public int maxBreath;
    public int currentBreath;
    public int Resiliance;
    public int damageModifier;
    public int attackModifier;
    public int defendModifier;
    public int wounds;
    public Factions faction;
    public Animator unitAnim;
    public Weapon weapon1;
}

public class Unit : MonoBehaviour
{
    public UnitInfo unitInfo = new UnitInfo();
    public Animator unitAnim;

    //These should be chosen from "drugdge" "elite" "dangerous"
    public string fate;

    protected void InitUnit()
    {
        unitInfo.weapon1 = gameObject.AddComponent<Weapon>();
        PlayerCharacter weaponOwner = gameObject.GetComponent<PlayerCharacter>();
        unitInfo.weapon1.owner = weaponOwner; 
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

        //Tell the initiative order to go
        Initiative.RemoveUnit(this);
    }
}