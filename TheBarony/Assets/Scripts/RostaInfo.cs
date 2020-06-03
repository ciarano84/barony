using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RostaInfo : MonoBehaviour
{
    public List<UnitInfo> rosta = new List<UnitInfo>();
    public List<UnitInfo> squad = new List<UnitInfo>();

    private void Start()
    {
        //dummy squad data
        UnitInfo testPlayer = new UnitInfo();
        testPlayer.unitName = "TESTPLAYER";
        testPlayer.weapon1 = new Weapon();
        testPlayer.weapon1.imageFile = "Shortsword";
        squad.Add(testPlayer);

        UnitInfo testPlayer2 = new UnitInfo();
        testPlayer2.unitName = "TESTPLAYER2";
        squad.Add(testPlayer2);

        UnitInfo testPlayer3 = new UnitInfo();
        testPlayer3.unitName = "TESTPLAYER3";
        squad.Add(testPlayer3);

        UnitInfo testPlayer4 = new UnitInfo();
        testPlayer4.unitName = "TESTPLAYER4";
        squad.Add(testPlayer4);

        //dummy rosta data
        rosta.Add(testPlayer);
        rosta.Add(testPlayer2);
        rosta.Add(testPlayer3);
        rosta.Add(testPlayer4);


    }
}
