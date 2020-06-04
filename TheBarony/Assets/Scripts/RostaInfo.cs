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
        testPlayer.weaponData = new WeaponData();
        rosta.Add(testPlayer);
        squad.Add(testPlayer);

        UnitInfo testPlayer2 = new UnitInfo();
        testPlayer2.unitName = "TESTPLAYER2";
        testPlayer2.weaponData = new WeaponData();
        rosta.Add(testPlayer2);
        squad.Add(testPlayer2);

        UnitInfo testPlayer3 = new UnitInfo();
        testPlayer3.unitName = "TESTPLAYER3";
        testPlayer3.weaponData = new WeaponData();
        rosta.Add(testPlayer3);
        squad.Add(testPlayer3);

        UnitInfo testPlayer4 = new UnitInfo();
        testPlayer4.unitName = "TESTPLAYER4";
        testPlayer4.weaponData = new WeaponData();
        rosta.Add(testPlayer4);
        squad.Add(testPlayer4);
    }
}
