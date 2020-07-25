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

    public static void FindAdjacentUnits(Unit unit)
    {
        unit.adjacentUnits.Clear();

        float halfHeight = unit.GetComponent<TacticsMovement>().halfHeight;

        //diagonals
        Vector3 forwardAndLeft = new Vector3(-1, 0, 1);
        Vector3 forwardAndRight = new Vector3(1, 0, 1);
        Vector3 backAndLeft = new Vector3(-1, 0, -1);
        Vector3 backAndRight = new Vector3(1, 0, -1);

        CheckForUnit(unit, Vector3.forward, halfHeight);
        CheckForUnit(unit, -Vector3.forward, halfHeight);
        CheckForUnit(unit, Vector3.right, halfHeight);
        CheckForUnit(unit, -Vector3.right, halfHeight);

        //Diagonals
        CheckForUnit(unit, forwardAndLeft, halfHeight);
        CheckForUnit(unit, forwardAndRight, halfHeight);
        CheckForUnit(unit, backAndLeft, halfHeight);
        CheckForUnit(unit, backAndRight, halfHeight);
    }

    static void CheckForUnit(Unit unit, Vector3 direction, float halfHeight)
    {
        RaycastHit hit;
        Vector3 viewpoint = unit.transform.position + new Vector3(0, halfHeight, 0);
        if (Physics.Raycast(viewpoint, direction, out hit, 1.5f))
        {
            if (hit.collider.GetComponent<Unit>() != null)
            {
                unit.adjacentUnits.Add(hit.collider.GetComponent<Unit>());
            }
        }
    }

    //NPC only AI tool for checking there is an A* path to a target. 
    public static bool PathFound(NPC originUnit, TacticsMovement target)
    {
        originUnit.destination = target.gameObject;
        target.GetCurrentTile();
        Tile t = originUnit.FindPath(target.currentTile);
        originUnit.destination = null;
        if (t != null) { return true; }
        else { return false; }
    }

    //Tile Filters
    public static List<Tile> FindTilesWithLineOfSight(TacticsMovement origin, List<Tile> tiles, TacticsMovement target)
    {
        float offset = 0.5f + origin.halfHeight;
        List<Tile> filteredTiles = new List<Tile>();
        foreach (Tile t in tiles)
        {
            Vector3 POV = t.transform.position + new Vector3(0, offset);
            if (Physics.Raycast(POV, (target.gameObject.transform.position + new Vector3(0, target.GetComponent<TacticsMovement>().halfHeight) - POV), out RaycastHit hit))
            {
                if (hit.collider.GetComponent<TacticsMovement>() == target)
                {
                    filteredTiles.Add(t);
                }
            }
        }
        return filteredTiles;
    }
}
