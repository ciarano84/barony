using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class ActionUIManager : MonoBehaviour
{
    public static TacticsMovement currentUnit;
    public Button endTurn;
    public Button Weapon1Attack;
    Weapon weapon1;

    private void Start()
    {
        endTurn.gameObject.SetActive(false);
    }

    public void UpdateActions(TacticsMovement unit)
    {
        GetEquipment();

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
    public void PlayerWeapon1(TacticsMovement unit)
    {
        weapon1.Attack();
    }

    public void PlayerEndsTurnEarly()
    {
        Initiative.EndTurn();
    }

    void GetEquipment()
    {
        weapon1 = currentUnit.weapon1;
    }

    public void Clear() {
        endTurn.gameObject.SetActive(false);
        //get rid of all Action UI. 
    }
}
