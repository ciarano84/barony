using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProphetData : AspectData
{
    public override void SetAspectData(UnitInfo unit)
    {
        className = "Prophet";
        unitInfo = unit;
        unit.mainWeaponData = new DaggerData();
        unit.offHandData = new BlankItemData();
        unit.armourData = new BlankItemData();
        unit.accessory1 = new BlankItemData();
        unit.accessory2 = new BlankItemData();
    }

    public override void Tier1()
    {
        //some kind of morale boost in here. 
        //do the effect where if they focus on you and have LoS they take damage each turn. 
    }

    public override void Tier2()
    {
        //set tier
        tier = 2;

        //health up
        unitInfo.baseBreath += 1;
        unitInfo.firstBreath += 1;
        if (unitInfo.fate != Fate.Drudge)
        {
            unitInfo.flaggingBreath += 1;
            unitInfo.baseBreath += 1;
        }
    }

    public override void GetAspect(Unit unit)
    {
        Prophet prophetAspect = unit.gameObject.AddComponent<Prophet>();
        prophetAspect.owner = unit;

        if (tier >= 2) prophetAspect.SetCurseAction();
    }

    public override Mesh GetVisual() { return GameAssets.i.PriestRobesMesh; }
}

public class Prophet : Aspect
{
    private void Start()
    {
        Initiative.OnTurnStart += EvilEye;
    }

    public void UnSubscribe(Unit unit)
    {
        if (unit == gameObject.GetComponent<Unit>())
        {
        }
    }

    public override void GetAspectData()
    {
        aspectData = new ProphetData();
    }

    private void OnDestroy()
    {
        UnSubscribe(gameObject.GetComponent<Unit>());
    }

    //Tier 1

    void EvilEye(Unit unit)
    {
        if (unit.focus == owner)
        {
            if (RangeFinder.LineOfSight(unit, owner))
            {
                unit.gameObject.GetComponent<UnitPopUpManager>().AddPopUpInfo("Evil Eye");
                unit.UpdateBreath(-1, true);
            }
        }
    }

    //Tier 2

    public void SetCurseAction()
    {
        Debug.Log("SetCurse called");
        Curse c = new Curse();
        c.SetActionButtonData(owner);
        owner.actions.Add(c);
    }
}
