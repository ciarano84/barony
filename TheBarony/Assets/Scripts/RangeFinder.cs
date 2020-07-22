using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeFinder
{
    //This is for checking for line of sight
    public static bool LineOfSight(Unit origin, Unit target, float RangeLimit = 0)
    {
        Vector3 POV = origin.transform.position + new Vector3(0, origin.GetComponent<TacticsMovement>().halfHeight);

        if (Physics.Raycast(POV, (target.gameObject.transform.position + new Vector3(0, target.GetComponent<TacticsMovement>().halfHeight) - POV), out RaycastHit hit))
        {
            if (target == hit.collider.gameObject.GetComponent<TacticsMovement>())
            {
                return true;
            }
        }
        return false;
    }

    //This is for finding the nearest... something. 
    public static GameObject FindNearestDestination(GameObject origin, List<GameObject> candidates)
    {
        GameObject nearest = null;
        float distance = Mathf.Infinity;

        foreach (GameObject go in candidates)
        {
            float d = Vector3.Distance(origin.transform.position, go.transform.position);

            if (d < distance)
            {
                distance = d;
                nearest = go;
            }
        }

        return nearest;
    }
}
