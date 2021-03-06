﻿using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Factions {players, enemies, allies}

public class EncounterManager : MonoBehaviour
{
    RostaInfo rosta;
    public static EncounterManager encounterManager;

    public GameObject encounterEndPanel;
    static GameObject staticEncounterPanel;

    public Text encounterEndtext;
    static Text staticEncounterEndtext;

    //public GameObject[] arenaBlocks;
    GameObject[] arenaBlocksToBeAssignedTo;

    public List<GameObject> playerSquad = new List<GameObject>();
    public GameObject playerPrefab;
    public GameObject goblinCutters;
    public GameObject orcBruisers;
    public GameObject goblinArchers;

    //This is all in lieu of an actual system of pulling in enemies. 
    List<List<GameObject>> enemyCells = new List<List<GameObject>>(); 

    //Debug
    public enum EncounterSettings { Standard, Test };
    public EncounterSettings encounterSettings = EncounterSettings.Standard;
    public enum TestClassType { PRIEST, DEFENDER, SCOUT, ANY };
    public TestClassType testClassType;
    public int playerCount;

    private void Awake()
    {
        if (RostaInfo.encounter == true || encounterSettings == EncounterSettings.Test)
        {
            RostaInfo.encounter = true;
            rosta = GameObject.Find("PlayerData" + "(Clone)").GetComponent<RostaInfo>();
            encounterManager = this;
            staticEncounterPanel = encounterEndPanel;
            staticEncounterEndtext = encounterEndtext;
            StartCoroutine(PositioningUnits());
        }

        if (RostaInfo.currentEncounter == null)
        {
            Encounter encounter = new Reclaim();
            encounter.permanent = true;
            //encounter.GetReferences();
            //Will need to change this formula if I want to get 3 by 3 arenas, but for now we're just making arenasizes 4 (2 b 2) and 6 (2 b 3). 
            encounter.arenaSize = (((int)UnityEngine.Random.Range(1, 3)) * 2) + 2;
            encounter.difficulty = UnityEngine.Random.Range(0, 2);
            encounter.enemyCompany = EncounterTable.CreateEnemyCompany(encounter);
            RostaInfo.currentEncounter = encounter;
        }
    }

    IEnumerator PositioningUnits()
    {
        //Checks to see if there are present enemies in the scene. If there are it will block new enemies being bought in. 
        GameObject presetEnemy = GameObject.FindGameObjectWithTag("character");
        
        //This waits to give the player data time to catch up. Hopefully not needed once the player data persists between scenes. 
        yield return new WaitForSeconds(0.1f);
        GetPlayers();
        if (presetEnemy == null) GetEnemies();
        yield return new WaitForSeconds(0.1f);
        ArenaBuilder.BuildArena();
        SetPositions();
        CombatLog.UpdateCombatLog("Units ready!");
        yield break;
    }

    void GetPlayers()
    {
        if (encounterSettings == EncounterSettings.Standard)
        {
            CompanyInfo companyInfo = RostaInfo.currentEncounter.selectedCompany;
            for (int i = 0; i < companyInfo.units.Count; i++)
            {
                GameObject player = Instantiate(GameAssets.i.PlayerUnit);
                player.GetComponent<Unit>().unitInfo = companyInfo.units[i];
                playerSquad.Add(player);
            }
        }

        if (encounterSettings == EncounterSettings.Test)
        {
            for (int i = 0; i < playerCount; i++)
            {
                GameObject player = Instantiate(GameAssets.i.PlayerUnit);
                player.GetComponent<Unit>().unitInfo = rosta.castle[i];
                playerSquad.Add(player);
            }
        }
    }

    void GetEnemies()
    {
        for (int CellCount = 0; CellCount < RostaInfo.currentEncounter.enemyCompany.cells.Count; CellCount++)
        {
            List<GameObject> enemies = new List<GameObject>();
            for (int EnemyCount = 0; EnemyCount < RostaInfo.currentEncounter.enemyCompany.cells[CellCount].enemies.Count; EnemyCount++)
            {
                GameObject enemyGO = Instantiate(RostaInfo.currentEncounter.enemyCompany.cells[CellCount].enemies[EnemyCount]);
                enemies.Add(enemyGO);
            }
            enemyCells.Add(enemies);
        }
    }
 
    void SetPositions()
    {
        for (int count = 0; count < ArenaBuilder.size; count++)
        {
            ArenaBlock arenaBlockScript = ArenaBuilder.arenaBlockList[count].GetComponent<ArenaBlock>();
            arenaBlockScript.spawnPoints = ShuffleArray(arenaBlockScript.spawnPoints);
        }

        PlaceUnitsOnSpawnPoints(playerSquad, ArenaBuilder.encounterArenaBlockList[0].GetComponent<ArenaBlock>());

        int blockToPlaceOn = 1;
        foreach (List<GameObject> cell in enemyCells)
        {
            PlaceUnitsOnSpawnPoints(cell, ArenaBuilder.encounterArenaBlockList[blockToPlaceOn].GetComponent<ArenaBlock>());
            blockToPlaceOn++;
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
            RostaInfo.currentEncounter.completionState = Encounter.CompletionState.VICTORY;
            encounterManager.StartCoroutine(encounterManager.EncounterEnd(Factions.players));
            return;
        }

        foreach (TacticsMovement unit in Initiative.order)
        {
            if (unit.unitInfo.faction == Factions.players) return;
        }
        RostaInfo.currentEncounter.completionState = Encounter.CompletionState.DEFEAT;
        encounterManager.StartCoroutine(encounterManager.EncounterEnd(Factions.enemies));
    }

    static void CheckForOtherWinCondition()
    { 
        //have other win conditions added as methods, then do a delegate in the start to decide which is going to get called.     
    }

    public IEnumerator EncounterEnd(Factions faction)
    {
        Initiative.queuedActions++;
        staticEncounterPanel.SetActive(true);
        staticEncounterEndtext.text = (faction + " are victorious!");
        RostaInfo.encounter = false;
        yield return new WaitForSeconds(3f);
        RostaInfo.currentEncounter.selectedCompany.units.Clear();
        foreach (GameObject unit in playerSquad)
        {
            if (unit != null)
            {
                UnitInfo info = unit.GetComponent<Unit>().unitInfo;
                RostaInfo.currentEncounter.selectedCompany.units.Add(info);
            }
        }
        Initiative.ResetStatics();
        SceneManager.LoadScene("Map");
        yield break;
    }

    //Randomize arrays of gameobjects
    public static GameObject[] ShuffleArray(GameObject[] source)
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
            units[i].transform.position = new Vector3 (p.x, p.y + 0.05f, p.z);
        }
    }
}
