using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum Factions {players, enemies, allies}

public class EncounterManager : MonoBehaviour
{
    public RostaInfo rosta;

    public GameObject encounterEndPanel;
    static GameObject staticEncounterPanel;

    public Text encounterEndtext;
    static Text staticEncounterEndtext;

    public GameObject[] arenaBlocks;
    GameObject[] arenaBlocksToBeAssignedTo;

    List<GameObject> playerSquad = new List<GameObject>();
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    //This is all in lieu of an actual system of pulling in enemies. 
    List<List<GameObject>> enemyCells = new List<List<GameObject>>(); 
    public int numberOfEnemyCells;
    public int EnemiesPerCell;

    private void Start()
    {
        staticEncounterPanel = encounterEndPanel;
        staticEncounterEndtext = encounterEndtext;
        GetPlayers();
        GetEnemies();
        SetPositions();
    }

    void GetPlayers()
    {
        //This next bit is in lieu of feeding in from the rosta. 
        for (int i = 0; i < rosta.squad.Count; i++)
        {
            GameObject player = Instantiate(playerPrefab);
            player.GetComponent<Unit>().unitInfo = rosta.squad[i].unitInfo; 
            playerSquad.Add(player);
        }
    }

    void GetEnemies()
    {
        for (int i = 0; i < numberOfEnemyCells; i++)
        {
            //is this overwriting? 
            enemyCells.Add(new List<GameObject>());
            for (int x = EnemiesPerCell; x > 0; x--)
            {
                GameObject enemy = Instantiate(enemyPrefab);
                enemyCells[i].Add(enemy);
                enemy.GetComponent<Unit>().unitInfo.faction = Factions.enemies;
            }
        }
    }
 
    void SetPositions()
    {
        //Randomize the blocks
        ShuffleArray(arenaBlocks);

        foreach (GameObject a in arenaBlocks)
        {
            ArenaBlock arenaBlockScript = a.GetComponent<ArenaBlock>();
            arenaBlockScript.spawnPoints = ShuffleArray(arenaBlockScript.spawnPoints);  
        }

        PlaceUnitsOnSpawnPoints(playerSquad, arenaBlocks[0].GetComponent<ArenaBlock>());

            int blockToPlaceOn = 1;
        foreach (List<GameObject> cell in enemyCells)
        {
            PlaceUnitsOnSpawnPoints(cell, arenaBlocks[blockToPlaceOn].GetComponent<ArenaBlock>());
            blockToPlaceOn++;
        }

    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            foreach (TacticsMovement unit in Initiative.order)
            {
                Debug.Log(unit + " belonging to faction " + unit.unitInfo.faction);
            }
        }    
    }

    public static void CheckForFactionDeath()
    {
        bool playerVictory = true;
        //checks to see if one side has wiped out the other. 
        foreach (TacticsMovement unit in Initiative.order)
        {
            if (unit.unitInfo.faction == Factions.enemies)
            {
                playerVictory = false;
                break;
            }
        }

        if (playerVictory) 
        {
            EncounterEnd(Factions.players);
            return;
        }

        foreach (TacticsMovement unit in Initiative.order)
        {
            if (unit.unitInfo.faction == Factions.players) return;
        }
        EncounterEnd(Factions.enemies);
    }

    static void CheckForOtherWinCondition()
    { 
        //have other win conditions added as methods, then do a delegate in the start to decide which is going to get called.     
    }

    static void EncounterEnd(Factions faction)
    {
        Initiative.queuedActions++;
        staticEncounterPanel.SetActive(true);
        staticEncounterEndtext.text = (faction + " are victorious!");
    }

    //Randomize arrays of gameobjects
    GameObject[] ShuffleArray(GameObject[] source)
    {
        for (int positionOfArray = 0; positionOfArray < source.Length; positionOfArray++)
        {
            GameObject obj = source[positionOfArray];
            int randomizeArray = Random.Range(0, positionOfArray);
            source[positionOfArray] = source[randomizeArray];
            source[randomizeArray] = obj;
        }

        return source;
    }

    void PlaceUnitsOnSpawnPoints(List<GameObject> units, ArenaBlock arenaBlock)
    {
        for (int i = 0; i < units.Count; i++)
        {
            Vector3 p = arenaBlock.spawnPoints[i].transform.position;
            //The following includes a hack. I'm not ACTUALLY working out where to place them on the Y, I'm just putting in a value that works for basic units as is. Will need remidying. 
            units[i].transform.position = new Vector3 (p.x, p.y + 0.5f/*2 * (p.y + units[i].GetComponent<TacticsMovement>().halfHeight)*/, p.z);
        }
    }
}
