using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestData : AspectData
{
    public override void SetAspectData(UnitInfo unit)
    {
        className = "Priest";
        unitInfo = unit;
        unit.mainWeaponData = new ShortswordData();
        unit.offHandData = new BlankItemData();
        unit.armourData = new BlankItemData();
        unit.accessory1 = new BlankItemData();
        unit.accessory2 = new BlankItemData();
    }

    public override void Level1()
    {

    }

    public override void GetAspect(Unit unit)
    {
        Priest priestAspect = unit.gameObject.AddComponent<Priest>();
        priestAspect.owner = unit;
    }

    public override GameObject GetVisual() { return GameAssets.i.PriestVisual; }
}

public class Priest : Aspect
{
    
    private void Start()
    {
        TacticsMovement.OnEnterSquare += Bless;
        Initiative.OnEncounterStart += Bless;
        Unit.onKO += UnSubscribe;
    }
    
    public void Bless(Unit mover)
    {
        foreach (Unit unit in Initiative.order)
        {
            if (RangeFinder.LineOfSight(owner, unit))
            {
                if (unit.unitInfo.faction == Factions.players && (unit.gameObject != gameObject))
                {
                    //check there are no effects of the same name. 
                    if (unit.gameObject.GetComponent<Bless>() == null)
                    {
                        Bless bless = unit.gameObject.AddComponent<Bless>();
                        bless.AddEffect(gameObject);
                    }
                    else if (!unit.gameObject.GetComponent<Bless>().enabled)
                    {
                        unit.gameObject.GetComponent<Bless>().enabled = true;
                        unit.gameObject.GetComponent<Bless>().AddEffect(gameObject);
                    }
                }
            }
        }
    }

    public void UnSubscribe(Unit unit)
    {
        if (unit == gameObject.GetComponent<Unit>())
        {
            TacticsMovement.OnEnterSquare -= Bless;
            Initiative.OnEncounterStart -= Bless;
            Unit.onKO -= UnSubscribe;
        }
    }

    public override void GetAspectData()
    {
        aspectData = new PriestData();
    }
}
