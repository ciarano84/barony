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
        player.baseBreath = 4 + Random.Range(0, 5);
        player.baseAttack = -1 + Random.Range(0, 4);
        player.baseDefence = -1 + Random.Range(0, 4);
        player.baseStrength = -1 + Random.Range(0, 4);
        player.baseToughness = -1 + Random.Range(0, 4);

        int classroll = Random.Range(0, 10);
        switch (classroll)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                player.aspectData = new DefenderData();
                break;
            case 4:
            case 5:
            case 6:
            case 7:
                player.aspectData = new ScoutData();
                break;
            case 8:
            case 9:
                player.aspectData = new PriestData();
                break;
        }
        player.aspectData.SetAspectData(player);
        player.aspectData.Level1();
    }
}
