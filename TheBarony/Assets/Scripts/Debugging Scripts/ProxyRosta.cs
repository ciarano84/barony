using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyRosta : MonoBehaviour
{
    //This script is JUST to give us a starting List of troops to work from. 

    List<PlayerCharacter> proxyRosta = new List<PlayerCharacter>();
    public int rostaTotal;
    string[] names = { "bob", "Sandy", "Rex", "Bill", "Helen", "Walter", "Elsa", "Reiner", "Daz", "Peter", "Lucy" };

    void Start()
    {
        for (int i = rostaTotal; i > 0; i--)
        {
            CreatePlayerInstance();
        }
    }

    PlayerCharacter CreatePlayerInstance()
    {
        PlayerCharacter player = new PlayerCharacter();
        player.name = (names[Random.Range(0, names.Length)] + names[Random.Range(0, names.Length)]);
        if (Random.Range(0, 10) > 6) player.className = ("Heavy");
        else player.className = ("Scout");
        player.maxBreath = 4 + Random.Range(0, 5);
        player.moveSpeed = 4 + Random.Range(0, 5);
        player.attackModifier = -1 + Random.Range(0, 3);
        player.defendModifier = -1 + Random.Range(0, 3);
        player.damageModifier = -1 + Random.Range(0, 3);
        player.Resiliance = -1 + Random.Range(0, 3);

        return player;
    }
}
