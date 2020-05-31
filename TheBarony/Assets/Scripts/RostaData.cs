using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RostaData
{
    public static List<GameObject> rosta;
    public static List<Unit> squad = new List<Unit>();
    public static string testString = "rostaDataTestString";
    /*
    public static void CopyUnitStats(Unit original, Unit target)
    {
        original.maxBreath = target.maxBreath;
        original.unitName = target.unitName;
        original.Resiliance = target.Resiliance;
        original.wounds = target.wounds;
        original.faction = target.faction;
        original.fate = target.fate;
        original.currentBreath = target.currentBreath;
        original.className = target.className;
        original.attackModifier = target.attackModifier;
        original.defendModifier = target.defendModifier;
    }*/
}
