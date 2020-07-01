using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyRosta : MonoBehaviour
{
    //This whole script is JUST to give us a starting List of troops to work from. 

    string[] names = { "bob", "Sandy", "Rex", "Bill", "Helen", "Walter", "Elsa", "Reiner", "Daz", "Peter", "Lucy" };

    public int numberOfStartingUnits = 8;

    RostaInfo rosta;
    Inventory inventory;
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
            //assign the squad first
            for (int n = 4; n > 0; n--)
            {
                UnitInfo unit = new UnitInfo();
                AssignStats(unit);
                rosta.squad.Add(unit);
            }
            //assign the rest of the rosta after. 
            for (int n = (numberOfStartingUnits-4); n > 0; n--)
            {
                UnitInfo unit = new UnitInfo();
                AssignStats(unit);
                rosta.rosta.Add(unit);
            }
            //Build the intial inventory
            inventory = playerData.GetComponent<Inventory>();
            InitializeInventory();
        }
    }

    void AssignStats(UnitInfo player)
    {
        player.unitName = (names[Random.Range(0, names.Length)] + " " + names[Random.Range(0, names.Length)]);
        player.firstBreath = 4 + Random.Range(0, 4);
        player.flaggingBreath = player.firstBreath;
        player.baseBreath = player.firstBreath + player.flaggingBreath;
        player.baseAttack = -1 + Random.Range(0, 4);
        player.baseDefence = -1 + Random.Range(0, 4);
        player.baseStrength = -1 + Random.Range(0, 4);
        player.baseToughness = -1 + Random.Range(0, 4);
        player.baseMove = 3 + Random.Range(0, 4);
        player.faction = Factions.players;

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
        if (player.mainWeaponData != null) player.mainWeaponData.SetData(player);
        if (player.offHandData != null) player.offHandData.SetData(player);
        if (player.armourData != null) player.armourData.SetData(player);
        if (player.accessory1 != null) player.accessory1.SetData(player);
        if (player.accessory2 != null) player.accessory2.SetData(player);

        player.aspectData.Level1();
    }

    void InitializeInventory()
    {
        inventory.UpdateEntry(new ShortswordData(), 1, true);

        inventory.UpdateEntry(new ShieldData(), 5, false);

        inventory.UpdateEntry(new LeatherArmourData(), 1, false);

        inventory.UpdateEntry(new MaceData(), 1, false);

        inventory.UpdateEntry(new GreataxeData(), 1, false);
    }
}
