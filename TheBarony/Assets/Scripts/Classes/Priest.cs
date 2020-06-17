using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestData : AspectData
{
    public override void SetAspectData()
    {
        className = "Priest";
    }

    public override void Level1(Unit unit)
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
    List<Unit> blessedUnits = new List<Unit>();

    private void Start()
    {
        TacticsMovement.OnEnterSquare += Bless;
    }

    public void Bless(Unit mover)
    {

        if (mover.gameObject != this.gameObject)
        {
            foreach (Unit unit in Initiative.order)
            {
                Vector3 rayOrigin = owner.gameObject.transform.position;

                //Debug
                Debug.DrawRay(transform.position, (unit.gameObject.transform.position - transform.position), Color.green, 1000f);

                // Declare a raycast hit to store information about what our raycast has hit
                if (Physics.Raycast(rayOrigin, (unit.gameObject.transform.position - transform.position), out RaycastHit hit))
                {
                    if (unit == hit.collider.gameObject.GetComponent<TacticsMovement>())
                    {
                        if (unit.unitInfo.faction == Factions.players)
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
        } 
    }
}
