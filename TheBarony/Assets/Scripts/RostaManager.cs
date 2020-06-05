using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RostaManager : MonoBehaviour
{
    //this class is totally borked as it's based on having game objects. Need to update it to use player data. 
    RostaInfo rosta;
    static int currentTroopShown = 1;
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
        rosta = GameObject.Find("PlayerData").GetComponent<RostaInfo>();
        StartCoroutine(WaitAndShowStats());
    }

    IEnumerator WaitAndShowStats()
    {
        yield return new WaitForSeconds(0.1f);
        ShowStats();
        yield break;
    }

    public void ShowStats()
    {
        Destroy(unitVisual);
        unitVisual = Instantiate(Resources.Load(rosta.rosta[currentTroopShown].unitVisual)) as GameObject;
        unitVisual.transform.position = pedestal.transform.position;
        unitVisual.transform.Rotate(0, 90, 0);

        nameText.text = (rosta.rosta[currentTroopShown].unitName);
        classText.text = (rosta.rosta[currentTroopShown].className);
        breathText.text = (rosta.rosta[currentTroopShown].maxBreath.ToString());
        attackText.text = (rosta.rosta[currentTroopShown].attackModifier.ToString());
        defenceText.text = (rosta.rosta[currentTroopShown].defendModifier.ToString());
        damageText.text = (rosta.rosta[currentTroopShown].damageModifier.ToString());
        armourText.text = (rosta.rosta[currentTroopShown].Resiliance.ToString());
        speedText.text = (rosta.rosta[currentTroopShown].ToString());
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

    void ShowUnit(Direction direction)
    {
        if (direction == Direction.right)
        {
            currentTroopShown++;
            if (currentTroopShown >= rosta.rosta.Count)
            {
                currentTroopShown = 0;
            }
        }
        else
        {
            currentTroopShown--;
            if (currentTroopShown < 0)
            {
                currentTroopShown = (rosta.rosta.Count - 1);
            }
        }
        ShowStats();
    }
}
