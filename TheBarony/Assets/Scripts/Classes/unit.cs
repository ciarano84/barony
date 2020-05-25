using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;

public class Unit : MonoBehaviour
{
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

    //These should be chosen from "drugdge" "elite" "dangerous"
    public string fate;

    protected void InitUnit()
    {
        weapon1 = gameObject.AddComponent<Weapon>();
        PlayerCharacter weaponOwner = gameObject.GetComponent<PlayerCharacter>();
        weapon1.owner = weaponOwner; 
    }

    public void UpdateBreath(int amount)
    {
        currentBreath += amount;
        if (currentBreath < 0)
        {
            currentBreath = 0;
            KO();
        }
    }

    public void UpdateWounds(int amount)
    {
        wounds += amount;
        while (amount > 0)
        {
            currentBreath -= 5;
            wounds++;
            amount--;
            if ((currentBreath <= 0) || (wounds >= 3))
            {
                StartCoroutine("KO");
            }
        }
    }

    //This needs sorting out. 
    public IEnumerator KO()
    {
        Initiative.queuedActions++;
        currentBreath = 0;

        unitAnim.SetBool("dead", true);
        yield return new WaitForSeconds(unitAnim.GetCurrentAnimatorStateInfo(0).length);

        //Tell the initiative order to go
        Initiative.RemoveUnit(this);
    }
}