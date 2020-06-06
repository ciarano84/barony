using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyRosta : MonoBehaviour
{
    //This whole script is JUST to give us a starting List of troops to work from. 

    string[] names = { "bob", "Sandy", "Rex", "Bill", "Helen", "Walter", "Elsa", "Reiner", "Daz", "Peter", "Lucy" };

    public int numberOfStartingUnits = 8;

    RostaInfo rosta;
    public GameObject playerData;
    public GameObject companySelectManager;

    void Awake()
    {
        if (GameObject.Find("PlayerData"+"(Clone)") != null)
        {
            rosta = GameObject.Find("PlayerData" + "(Clone)").GetComponent<RostaInfo>();
        }
        else 
        {
            rosta = Instantiate(playerData).GetComponent<RostaInfo>();
            for (int n = 4; n > 0; n--)
            {
                UnitInfo unit = new UnitInfo();
                AssignStats(unit);
                rosta.squad.Add(unit);
            }
            for (int n = (numberOfStartingUnits-4); n > 0; n--)
            {
                UnitInfo unit = new UnitInfo();
                AssignStats(unit);
                rosta.rosta.Add(unit);
            }
        }
    }

    void AssignStats(UnitInfo player)
    {
        player.unitName = (names[Random.Range(0, names.Length)] + " " + names[Random.Range(0, names.Length)]);
        if (Random.Range(0, 10) > 6)
        {
            player.className = ("Heavy");
            player.unitVisual = "PlayerVisual";
        }
        else
        {
            player.className = ("Scout");
            player.unitVisual = "PlayerVisualScout";
        }
        player.maxBreath = 4 + Random.Range(0, 5);
        player.attackModifier = -1 + Random.Range(0, 3);
        player.defendModifier = -1 + Random.Range(0, 3);
        player.damageModifier = -1 + Random.Range(0, 3);
        player.Resiliance = -1 + Random.Range(0, 3);
    }
}
