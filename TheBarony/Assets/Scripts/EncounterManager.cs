using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum Factions {players, enemies, allies}

public class EncounterManager : MonoBehaviour
{
    public GameObject encounterEndPanel;
    static GameObject staticEncounterPanel;

    public Text encounterEndtext;
    static Text staticEncounterEndtext;

    public GameObject[] arenaBlocks;
    GameObject[] arenaBlocksToBeAssignedTo;

    List<GameObject> playerSquad = new List<GameObject>();
    public GameObject playerPrefab;

    List<Unit> enemySquad = new List<Unit>();

    private void Start()
    {
        staticEncounterPanel = encounterEndPanel;
        staticEncounterEndtext = encounterEndtext;
        GetPlayers();
        //Get enemies into scene (remember they will add themselves to initiative)
        SetPositions();
    }

    void GetPlayers()
    {
        //This next bit is in lieu of feeding in from the rosta. 
        for (int i = 4; i > 0; i--)
        {
            GameObject player = Instantiate(playerPrefab);
            playerSquad.Add(player);
        }
    }


    //This needs writing. Will be what positions players and enemies. 
    void SetPositions()
    {
        //choose one block for the players to be set up on.
        //set them up. 
        //chose another bock for the enemies to be set up on. 
        //set them up. 

        //Randomize the blocks
        ShuffleArray(arenaBlocks);
        foreach (GameObject a in arenaBlocks)
        {
            ArenaBlock arenaBlockScript = a.GetComponent<ArenaBlock>();
            arenaBlockScript.spawnPoints = ShuffleArray(arenaBlockScript.spawnPoints);  
        }

    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            foreach (TacticsMovement unit in Initiative.order)
            {
                Debug.Log(unit + " belonging to faction " + unit.faction);
            }
        }    
    }

    public static void CheckForFactionDeath()
    {
        bool playerVictory = true;
        //checks to see if one side has wiped out the other. 
        foreach (TacticsMovement unit in Initiative.order)
        {
            if (unit.faction == Factions.enemies)
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
            if (unit.faction == Factions.players) return;
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
}
