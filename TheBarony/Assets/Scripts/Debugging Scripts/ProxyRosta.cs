using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyRosta : MonoBehaviour
{
    //This whole script is JUST to give us a starting List of troops to work from. 

    string[] names = { "bob", "Sandy", "Rex", "Bill", "Helen", "Walter", "Elsa", "Reiner", "Daz", "Peter", "Lucy" };

    static public int numSelectors = 5;
    public static List<PlayerCharacter> proxyRosta = new List<PlayerCharacter>();
    public GameObject unit1;
    public GameObject unit2;
    public GameObject unit3;
    public GameObject unit4;
    public GameObject unit5;
    public GameObject unit6;
    void Start()
    {
        proxyRosta.Add(unit1.GetComponent<PlayerCharacter>());
        proxyRosta.Add(unit2.GetComponent<PlayerCharacter>());
        proxyRosta.Add(unit3.GetComponent<PlayerCharacter>());
        proxyRosta.Add(unit4.GetComponent<PlayerCharacter>());
        proxyRosta.Add(unit5.GetComponent<PlayerCharacter>());
        proxyRosta.Add(unit6.GetComponent<PlayerCharacter>());

        foreach (PlayerCharacter p in proxyRosta)
        {
            AssignStats(p);
        }

        RostaManager.BringInRosta();
    }

    void AssignStats(PlayerCharacter player)
    {
        player.unitName = (names[Random.Range(0, names.Length)] + names[Random.Range(0, names.Length)]);
        if (Random.Range(0, 10) > 6) player.className = ("Heavy");
        else player.className = ("Scout");
        player.maxBreath = 4 + Random.Range(0, 5);
        player.moveSpeed = 4 + Random.Range(0, 5);
        player.attackModifier = -1 + Random.Range(0, 3);
        player.defendModifier = -1 + Random.Range(0, 3);
        player.damageModifier = -1 + Random.Range(0, 3);
        player.Resiliance = -1 + Random.Range(0, 3);
    }
}
