using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class ActionUIManager : MonoBehaviour
{
    public static PlayerCharacter currentUnit;
    public Button endTurn;
    public Button Weapon1Attack;
    Weapon weapon1;

    private void Start()
    {
        endTurn.gameObject.SetActive(false);
    }

    public void UpdateActions(PlayerCharacter unit)
    {
        currentUnit = unit;
        GetEquipmentUI(unit.weapon1);

        if (unit.GetComponent<PlayerCharacter>() != null)
        {
            if (unit.remainingMove > 0 || unit.remainingActions > 0)
            {
                //need to make sure this gets turned off at some point. 
                endTurn.gameObject.SetActive(true);
            }
            else Clear();
        }
        else {
            //This is for NPC actions, so not really needed atm.
            Clear();
            return;
        }
    }

    //A default, starting action for testing purposes. 
    public void PlayerWeapon1()
    {
        currentUnit.weapon1.StartCoroutine("Attack");
    }

    public void PlayerEndsTurnEarly()
    {
        Initiative.EndTurn();
    }

    void GetEquipmentUI(Weapon weapon1)
    {
        //not sure what I need this for actually. 
    }

    public void Clear() {
        endTurn.gameObject.SetActive(false);
        //get rid of all Action UI. 
    }
}
