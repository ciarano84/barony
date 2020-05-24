using System.Collections;
using System.Collections.Generic;
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
    
    //These should be chosen from "drugdge" "elite" "dangerous"
    public string fate;
    public Weapon weapon1;
    public Animator unitAnim;

    public void UpdateBreath(int amount)
    {
        currentBreath += amount;
        if (currentBreath < 0)
        {
            currentBreath = 0;
            StartCoroutine("KO");
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

    public IEnumerator KO()
    {
        //Tell the initiative order to hang on. 
        Initiative.action = true;

        currentBreath = 0;
        unitAnim.SetBool("dead", true);
        yield return new WaitForSeconds(unitAnim.GetCurrentAnimatorStateInfo(0).length/* + unitAnim.GetCurrentAnimatorStateInfo(0).normalizedTime*/);

        //Tell the initiative order to go
        Initiative.action = false;
        Initiative.RemoveUnit(this.GetComponent<TacticsMovement>());
    }
}
