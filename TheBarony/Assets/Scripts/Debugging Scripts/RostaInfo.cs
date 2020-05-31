using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RostaInfo : MonoBehaviour
{
    public List<PlayerCharacter> rosta = new List<PlayerCharacter>();
    public List<PlayerCharacter> squad = new List<PlayerCharacter>();

    private void Start()
    {
        PlayerCharacter testPlayer = new PlayerCharacter();
        testPlayer.unitInfo.unitName = "TESTPLAYER";
        squad.Add(testPlayer);
    }
}
