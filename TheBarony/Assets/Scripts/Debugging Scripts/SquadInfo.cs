using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitInfo
    {
        public int maxBreath;
        public string unitName;
        public int Resiliance;
        public int wounds;
        public Factions faction;
    }

public class SquadInfo : MonoBehaviour
{
    public List<UnitInfo> squadInfo = new List<UnitInfo>();

    void AddUnit(int maxBreath, string unitName, int resliance, int wounds, Factions faction)
    {
        UnitInfo unit = new UnitInfo();
        unit.maxBreath = maxBreath;


        squadInfo.Add(unit);
    }

    public UnitInfo DeployUnit(int numberInSquad)
    {
        return squadInfo[numberInSquad];
    }


    
    public int maxBreath;
    public string unitName;
    public int Resiliance;
    public int wounds;
    public Factions faction;
        //original.fate = target.fate;
        //original.currentBreath = target.currentBreath;
        //original.className = target.className;
        //original.attackModifier = target.attackModifier;
        //original.defendModifier = target.defendModifier;
}
