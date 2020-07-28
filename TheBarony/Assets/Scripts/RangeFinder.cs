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

    public static List<Tile> FindTilesNotNextToEnemy(TacticsMovement origin, List<Tile> tiles, Factions opposingFaction)
    {
        List<Tile> filteredTiles = new List<Tile>();

        List<Unit> opponents = new List<Unit>();
        switch (opposingFaction)
        {
            case Factions.players:
                opponents = Initiative.players;
                break;
            case Factions.enemies:
                opponents = Initiative.enemies;
                break;
            default:
                break;
        }
        
        foreach (Tile t in tiles)
        {
            bool found = false;

            foreach (Unit opponent in opponents)
            {
                TacticsMovement opponentTactics = opponent.GetComponent<TacticsMovement>();
                opponentTactics.GetCurrentTile();
                opponentTactics.currentTile.FindNeighbours(opponentTactics.jumpHeight, null);

                foreach (Tile orthagonalTile in opponentTactics.currentTile.adjacencyList)
                {
                    if (t == orthagonalTile) found = true;
                }
                foreach (Tile diagonalTile in opponentTactics.currentTile.diagonalAdjacencyList)
                {
                    if (t == diagonalTile) found = true;
                }
                if (!found) filteredTiles.Add(t);
            }
        }
        return filteredTiles;
    }

    public static Tile FindTileFurthestFromOpponents(TacticsMovement origin, List<Tile> tiles)
    {
        Tile furthest = null;
        float highestRunValue = 0f;
        foreach (Tile t in tiles)
        {
            float runValue = 0f;
            foreach (Unit opponent in Initiative.players)
            {
                runValue += Vector3.Distance(opponent.transform.position, t.transform.position);
            }
            if (runValue > highestRunValue)
            {
                furthest = t;
                highestRunValue = runValue;
            }  
        }
        return furthest;
    }

    public static Tile FindFlankingTile(TacticsMovement origin, List<Tile> tiles, TacticsMovement target)
    {
        Debug.Log("Looking for flanking tile");
        Tile flankingTile = null;

        RangeFinder.FindAdjacentUnits(target);
        foreach (TacticsMovement u in target.adjacentUnits)
        {
            u.GetCurrentTile();
            Tile allyTile = u.currentTile;
            foreach (Tile t in target.currentTile.adjacencyList)
            {
                Vector3 relTargetPosition = t.transform.InverseTransformPoint(target.currentTile.transform.position);
                Vector3 relOtherAttackerPosition = t.transform.InverseTransformPoint(allyTile.transform.position);
                if (relOtherAttackerPosition.z > (relTargetPosition.z + 0.1f))
                {
                    flankingTile = t;
                    break;
                }
            }
            foreach (Tile t in target.currentTile.diagonalAdjacencyList)
            {
                Vector3 relTargetPosition = t.transform.InverseTransformPoint(target.currentTile.transform.position);
                Vector3 relOtherAttackerPosition = t.transform.InverseTransformPoint(allyTile.transform.position);
                if (relOtherAttackerPosition.z > (relTargetPosition.z + 0.1f))
                {
                    flankingTile = t;
                    break;
                }
            }
        }
        if (tiles.Contains(flankingTile)) return flankingTile;
        else return null;
    }
}
