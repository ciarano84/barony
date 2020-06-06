using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RostaManager : MonoBehaviour
{
    //this class is totally borked as it's based on having game objects. Need to update it to use player data. 
    RostaInfo rosta;
    public GameObject pedestal;
    GameObject unitVisual;

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
        rosta = GameObject.Find("PlayerData" + "(Clone)").GetComponent<RostaInfo>();
        StartCoroutine(WaitAndShowStats());
    }

    IEnumerator WaitAndShowStats()
    {
        yield return new WaitForSeconds(0.1f);

        //Move the troop out the squad and into the rosta
        rosta.rosta.Add(rosta.squad[rosta.companyPosition]);
        //rosta.squad.Remove(rosta.squad[rosta.companyPosition]);
        rosta.currentUnitShown = rosta.rosta.Count-1;

        ShowStats();
        yield break;
    }

    public void ShowStats()
    {
        Destroy(unitVisual);
        unitVisual = Instantiate(Resources.Load(rosta.rosta[rosta.currentUnitShown].unitVisual)) as GameObject;
        unitVisual.transform.position = pedestal.transform.position;
        unitVisual.transform.Rotate(0, 90, 0);

        nameText.text = (rosta.rosta[rosta.currentUnitShown].unitName);
        classText.text = (rosta.rosta[rosta.currentUnitShown].className);
        breathText.text = (rosta.rosta[rosta.currentUnitShown].maxBreath.ToString());
        attackText.text = (rosta.rosta[rosta.currentUnitShown].attackModifier.ToString());
        defenceText.text = (rosta.rosta[rosta.currentUnitShown].defendModifier.ToString());
        damageText.text = (rosta.rosta[rosta.currentUnitShown].damageModifier.ToString());
        armourText.text = (rosta.rosta[rosta.currentUnitShown].Resiliance.ToString());
        speedText.text = (rosta.rosta[rosta.currentUnitShown].ToString());
    }

    public void OnRightButtonClick() { GetNextsUnit(); }
    public void GetNextsUnit()
    {
        ShowUnit(Direction.right);
    }

    public void OnLeftButtonClick() { GetPreviousUnit(); }
    public void GetPreviousUnit()
    {
        ShowUnit(Direction.left);
    }

    public void SelectUnit()
    {
        //Add unit to squad.
        rosta.squad[rosta.companyPosition] = rosta.rosta[rosta.currentUnitShown];

        //Remove it from rosta.
        rosta.rosta.Remove(rosta.rosta[rosta.currentUnitShown]);

        SceneManager.LoadScene("SquadView");
    }

    void ShowUnit(Direction direction)
    {
        if (direction == Direction.right)
        {
            rosta.currentUnitShown++;
            if (rosta.currentUnitShown >= rosta.rosta.Count)
            {
                rosta.currentUnitShown = 0;
            }
        }
        else
        {
            rosta.currentUnitShown--;
            if (rosta.currentUnitShown < 0)
            {
                rosta.currentUnitShown = (rosta.rosta.Count - 1);
            }
        }
        ShowStats();
    }
}
