﻿using System;
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

    public Transform[] inspectorArenaPoints;
    static public Transform[,] arenaPoints = new Transform[3,3]; 
    static int xInBlocks = 2;
    static int zInBlocks = 2;
    public static int size = 4;

    private void Awake()
    {   
        //Load in all the blocks
        baseList.Add(GameAssets.i.ArenaBarns);
        baseList.Add(GameAssets.i.ArenaBog);
        baseList.Add(GameAssets.i.ArenaChapel);
        baseList.Add(GameAssets.i.ArenaHills);
        baseList.Add(GameAssets.i.ArenaMarket);
        baseList.Add(GameAssets.i.ArenaTavern);

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
        CreateRandomBlockList(baseList);
        GetDimensions();
        PlaceArenaBlocks();
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
                Instantiate(arenaBlockList[count], arenaPoints[i, j]);
                count++;
            }
        }
    }
}
