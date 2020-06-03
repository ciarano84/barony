using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RostaInfo : MonoBehaviour
{
    public List<UnitInfo> rosta = new List<UnitInfo>();
    public List<UnitInfo> squad = new List<UnitInfo>();

    private void Start()
    {
        UnitInfo testPlayer = new UnitInfo();
        testPlayer.unitName = "TESTPLAYER";
        squad.Add(testPlayer);
    }
}
