using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaBuilder : MonoBehaviour
{
    //The base list to take from.
    public static List<GameObject> baseList = new List<GameObject>();

    //The list you'll use in the interim to sort the blocks.
    static GameObject[] sortingArray = new GameObject[6];

    //The final list you'll want referenced to pull the blocks from. It should have 10 bocks, and no block appearing more than twice. 
    public static List<GameObject> arenaBlockList = new List<GameObject>();

    public static List<GameObject> encounterArenaBlockList = new List<GameObject>();

    public static GameObject[] tiles;

    public Transform[] inspectorArenaPoints;
    static public Transform[,] arenaPoints = new Transform[3,3]; 
    static int xInBlocks = 2;
    static int zInBlocks = 2;
    public static int size = 4;
    static bool preSetArenaPresent;

    private void Awake()
    {   
        //Load in all the blocks
        baseList.Add(GameAssets.i.ArenaBarns);
        baseList.Add(GameAssets.i.ArenaBog);
        baseList.Add(GameAssets.i.ArenaChapel);
        baseList.Add(GameAssets.i.ArenaHills);
        baseList.Add(GameAssets.i.ArenaMarket);
        baseList.Add(GameAssets.i.ArenaTavern);

        //Check to see if there is a preset arena, and if so, don't build one. 
        foreach (Transform ap in inspectorArenaPoints)
        {
            if (ap.GetComponent<ArenaPoint>().block != null)
            {
                if (!preSetArenaPresent) size = 0;
                preSetArenaPresent = true;
                arenaBlockList.Add(ap.GetComponent<ArenaPoint>().block.gameObject);
                encounterArenaBlockList.Add(ap.GetComponent<ArenaPoint>().block.gameObject);
                size++;
            }
        }
        if (preSetArenaPresent) return;

        //get all the inspector set grid points into the arenaPoints array. 
        int gridSize = (int)Math.Sqrt(inspectorArenaPoints.Length);
        int count = 0;
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                arenaPoints[i, j] = inspectorArenaPoints[count];
                count++;
            }
        }
    }

    public static void BuildArena()
    {
        if (!preSetArenaPresent)
        {
            CreateRandomBlockList(baseList);
            GetDimensions();
            PlaceArenaBlocks();
        }
        tiles = new GameObject[size * 144];
        tiles = GameObject.FindGameObjectsWithTag("tile");
    }

    public static void CreateRandomBlockList(List<GameObject> _blockList)
    {
        ShuffleBlocks(_blockList);

        for (int count = 0; count < sortingArray.Length; count++)
        {
            arenaBlockList.Add(sortingArray[count]);
        }

        ShuffleBlocks(_blockList);

        for (int count = sortingArray.Length; count < (sortingArray.Length *2); count++)
        {
            arenaBlockList.Add(sortingArray[count - sortingArray.Length]);
        }
    }

    static void ShuffleBlocks(List<GameObject> _originList)
    {   
        for (int count = 0; count < _originList.Count; count++)
        {
            sortingArray[count] = _originList[count];
        }
        sortingArray = EncounterManager.ShuffleArray(sortingArray);
    }

    static void GetDimensions()
    {
        xInBlocks = 2; zInBlocks = 2;

        if (RostaInfo.currentEncounter != null) size = RostaInfo.currentEncounter.arenaSize;

        switch (size)
        {
            case 6:
                int randomRoll = UnityEngine.Random.Range(0, 2);
                if (randomRoll == 0) xInBlocks = 3;
                else zInBlocks = 3;
                break;
            default:
                break;
        }
    }

    static void PlaceArenaBlocks()
    {
        int count = 0;
        for (int i = 0; i < xInBlocks; i++)
        {
            for (int j = 0; j < zInBlocks; j++)
            {
                //check for a dupe laid previously on the x dimension.
                if (i > 0)
                {
                    if (arenaPoints[i-1, j].GetComponent<ArenaPoint>().block.name == arenaBlockList[count].name) count = AbilityCheck.IncrementAndLoopNumber(count, arenaBlockList.Count);
                }

                //check for a dupe laid previously on the z dimension. 
                if (j > 0)
                {
                    if (arenaPoints[i, j - 1].GetComponent<ArenaPoint>().block.name == arenaBlockList[count].name) count = AbilityCheck.IncrementAndLoopNumber(count, arenaBlockList.Count);
                }

                GameObject block = Instantiate(arenaBlockList[count], arenaPoints[i, j]);
                encounterArenaBlockList.Add(block);
                arenaPoints[i, j].GetComponent<ArenaPoint>().block = block;
                count = AbilityCheck.IncrementAndLoopNumber(count, arenaBlockList.Count);
            }
        }
    }
}
