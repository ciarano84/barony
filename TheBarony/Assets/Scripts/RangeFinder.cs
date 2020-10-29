using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
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

    public static void FindAdjacentUnits(TacticsMovement unit)
    {
        unit.adjacentUnits.Clear();

        foreach (Neighbour n in unit.currentTile.neighbours)
        {
            if (n != null)
            {
                if (n.tile != null)
                {
                    if (n.tile.occupant != null)
                    {
                        unit.adjacentUnits.Add(n.tile.occupant);
                    }
                }
            }
        }
    }

    static void CheckForUnit(Unit unit, Vector3 direction, float halfHeight)
    {
        RaycastHit hit;
        Vector3 viewpoint = unit.transform.position + new Vector3(0, halfHeight, 0);
        if (Physics.Raycast(viewpoint, direction, out hit, 1.5f, ~9))
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
        //Trying to find the cause of creatures moving into each other's tiles. 
        Debug.Log("findTileNextToEnemy called");
        
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
                //The next line shouldn't be needed as tiles should be allocated when a unit moves into them. 
                opponentTactics.GetCurrentTile();
                //opponentTactics.currentTile.FindNeighbours(opponentTactics.jumpHeight, null);
                opponentTactics.currentTile.CheckNeighbours(opponentTactics.jumpHeight, null);

                foreach (Tile orthagonalTile in opponentTactics.currentTile.adjacencyList)
                {
                    if (t == orthagonalTile) found = true;
                }
                foreach (Tile diagonalTile in opponentTactics.currentTile.diagonalAdjacencyList)
                {
                    if (t == diagonalTile) found = true;
                }
            }
            if (!found) filteredTiles.Add(t);
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
        Tile flankingTile = null;

        FindAdjacentUnits(target);
        foreach (TacticsMovement u in target.adjacentUnits)
        {
            if (u != origin)
            {
                if (u.unitInfo.faction == origin.unitInfo.faction)
                {
                    u.GetCurrentTile();
                    Tile allyTile = u.currentTile;
                    Vector3 flankingTileLocation = allyTile.transform.position + ((target.currentTile.transform.position - allyTile.transform.position) * 2);
                    Vector3 overlapBoxSize = new Vector3(.25f, origin.jumpHeight * 4, .25f);
                    Collider[] colliders = Physics.OverlapBox(flankingTileLocation, overlapBoxSize);
                    foreach (Collider collider in colliders)
                    {
                        if (collider.tag == "tile")
                        {
                            flankingTile = collider.gameObject.GetComponent<Tile>();
                        }
                    }
                }
            }
        }
        if (tiles.Contains(flankingTile)) return flankingTile;
        else return null;
    }

    //This can return null. 
    public static Tile FindTileNextToTarget(TacticsMovement _origin, TacticsMovement _target)
    {
        _origin.GetCurrentTile();
        
        Tile closestTile = null;
        float maxDistance = Mathf.Infinity;

        foreach (Neighbour neighbour in _target.currentTile.neighbours)
        {
            if (neighbour != null)
            {
                foreach (Tile tileCanBeWalkedTo in _origin.selectableTiles)
                {
                    if (tileCanBeWalkedTo == neighbour.tile)
                    {
                        if (Vector3.Distance(_origin.transform.position, tileCanBeWalkedTo.transform.position) < maxDistance)
                        {
                            maxDistance = Vector3.Distance(_origin.transform.position, tileCanBeWalkedTo.transform.position);
                            closestTile = neighbour.tile;
                        }
                    }
                }
            }
        }
        return closestTile;
    }

    //This returns a number which is how many more of your allies their are than the enemy, if it's negative, it's how many more of them there are. 
    public static int HowOutnumberedAmI(Unit _unit)
    {
        int outNumberCount = 1;

        foreach (TacticsMovement unit in Initiative.order)
        {
            if (LineOfSight(_unit, unit))
            {
                if (unit.unitInfo.faction != _unit.unitInfo.faction) outNumberCount--;
                if (unit.unitInfo.faction == _unit.unitInfo.faction) outNumberCount++;
            }
        }
        return outNumberCount;
    }

    //Find a direction. The subject is asking the question here, and wants to know the direction to the object. 
    public static int FindDirection(Transform _subject, Transform _object)
    { 
        UnityEngine.Vector3 s = _subject.position;
        UnityEngine.Vector3 o = _object.position;
        float[] positions = new float[8];
        positions[0] = o.z - s.z;
        positions[1] = ((o.z - s.z) + (o.x - s.x)) / 1.4f;
        positions[2] = o.x - s.x;
        positions[3] = ((o.x - s.x) + (s.z - o.z)) / 1.4f;
        positions[4] = s.z - o.z;
        positions[5] = ((s.z - o.z) + (s.x - o.x)) / 1.4f;
        positions[6] = s.x - o.x;
        positions[7] = ((s.x - o.x) + (o.z - s.z)) / 1.4f;

        int position = 0;
        float p = positions[0];
        for (int count = 0; count < 8; count++)
        {
            if (positions[count] > p)
            {
                position = count;
                p = positions[count];
            }
        }
        return position;
    }

    public static Tile FindTileToDodgeTo(TacticsMovement defender, Unit attacker, int direction)
    {
        List<Neighbour> viableNeighbours = new List<Neighbour>();
        List<Tile> tiles = new List<Tile>();

        //if it's a ranged attack, pick squares to the side and add them to the list. 
        if (attacker.mainWeapon.weaponData.rangeType == WeaponData.Range.ranged)
        {
            int opposite = GetDirection(direction + 4);
            for (int count = 0; count < 8; count++)
            {
                if (defender.currentTile.neighbours[count] != null)
                {
                    if (count != direction && count != opposite)
                    {
                        viableNeighbours.Add(defender.currentTile.neighbours[count]);
                    }
                }     
            }
        }
        //if it's melee then dodge away or to the side. 
        else if (attacker.mainWeapon.weaponData.rangeType == WeaponData.Range.melee)
        {
            int right = GetDirection(direction - 1);
            int left = GetDirection(direction + 1);

            for (int count = 0; count < 8; count++)
            {
                if (defender.currentTile.neighbours[count] != null)
                {
                    if (count != direction && count != right && count != left)
                    {
                        viableNeighbours.Add(defender.currentTile.neighbours[count]);
                    }
                }
            }
        }

        if (viableNeighbours.Count == 0) return null;

        //Check squares on the list for all that are unobscured, empty and don't have any height difference. 
        foreach (Neighbour neighbour in viableNeighbours)
        {
            if (neighbour.height <= 0 && neighbour.tile.occupant == null && !neighbour.tile.difficultTerrain)
            {
                tiles.Add(neighbour.tile);
            }
        }

        if (tiles.Count == 0) return null;

        int r = UnityEngine.Random.Range(0, tiles.Count);
        return tiles[r]; 
    }

    public static int GetDirection(int n)
    {
        if (n > 7) n -= 8;
        if (n < 0) n += 8;
        return n;
    }
}
