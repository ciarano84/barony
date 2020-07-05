using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeFinder
{
    //range is included here if ever I need to specifiy a range limit. 
    public static bool LineOfSight(Unit origin, Unit target, float RangeLimit = 0)
    {
        if (Physics.Raycast(origin.transform.position, (target.gameObject.transform.position - origin.transform.position), out RaycastHit hit))
        {
            if (target == hit.collider.gameObject.GetComponent<TacticsMovement>())
            {
                return true;
            }
        }
        return false;
    }
}
