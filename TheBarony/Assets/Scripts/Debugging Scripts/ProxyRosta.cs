using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyRosta : MonoBehaviour
{
    //This whole script is JUST to give us a starting List of troops to work from. 

    string[] names = { "bob", "Sandy", "Rex", "Bill", "Helen", "Walter", "Elsa", "Reiner", "Daz", "Peter", "Lucy" };

    static public int numSelectors = 5;
    public static List<UnitInfo> proxyRosta = new List<UnitInfo>();
    public UnitInfo unit1;
    public UnitInfo unit2;
    public UnitInfo unit3;
    public UnitInfo unit4;
    public UnitInfo unit5;
    public UnitInfo unit6;
    void Start()
    {
        proxyRosta.Add(new UnitInfo());
        proxyRosta.Add(new UnitInfo());
        proxyRosta.Add(new UnitInfo());
        proxyRosta.Add(new UnitInfo());
        proxyRosta.Add(new UnitInfo());
        proxyRosta.Add(new UnitInfo());

        foreach (UnitInfo p in proxyRosta)
        {
            AssignStats(p);
        }

        RostaManager.BringInRosta();
    }

    void AssignStats(UnitInfo player)
    {
        player.unitName = (names[Random.Range(0, names.Length)] + names[Random.Range(0, names.Length)]);
        if (Random.Range(0, 10) > 6) player.className = ("Heavy");
        else player.className = ("Scout");
        player.maxBreath = 4 + Random.Range(0, 5);
        player.attackModifier = -1 + Random.Range(0, 3);
        player.defendModifier = -1 + Random.Range(0, 3);
        player.damageModifier = -1 + Random.Range(0, 3);
        player.Resiliance = -1 + Random.Range(0, 3);
    }
}
