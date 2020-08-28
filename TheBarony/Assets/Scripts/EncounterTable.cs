using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum EnemyType { BANDITS, GOBLINOIDS };

public class EncounterTable
{
    public static List<GameObject> LookUpEnemyCellContents(EnemyCompany enemyCompany, int cellStrength, EnemyType enemyType)
    {
        List<GameObject> monsters = new List<GameObject>();

        //have to find a way to add in the elite.

        if (enemyType == EnemyType.BANDITS)
        {
            if (!enemyCompany.leaderAdded)
            {
                monsters.Add(GameAssets.i.EliteBanditHunter);
                enemyCompany.leaderAdded = true;
            }
            
            if (cellStrength == 0)
            {
                monsters.Add(GameAssets.i.BanditAxeman);
                monsters.Add(GameAssets.i.BanditAxeman);
            }
            else if (cellStrength == 1)
            {
                monsters.Add(GameAssets.i.BanditHunter);
                monsters.Add(GameAssets.i.BanditHunter);
                monsters.Add(GameAssets.i.BanditCuthroat);
                monsters.Add(GameAssets.i.BanditCuthroat);
            }
            else
            {
                monsters.Add(GameAssets.i.BanditHunter);
                monsters.Add(GameAssets.i.BanditHunter);
                monsters.Add(GameAssets.i.BanditAxeman);
                monsters.Add(GameAssets.i.BanditAxeman);
                monsters.Add(GameAssets.i.BanditAxeman);
            }
        }
        else if (enemyType == EnemyType.GOBLINOIDS)
        {
            if (!enemyCompany.leaderAdded)
            {
                monsters.Add(GameAssets.i.EliteOrcBrute);
                enemyCompany.leaderAdded = true;
            }

            if (cellStrength == 0)
            {
                monsters.Add(GameAssets.i.GoblinHunter);
                monsters.Add(GameAssets.i.GoblinHunter);
                monsters.Add(GameAssets.i.GoblinScout);
                monsters.Add(GameAssets.i.GoblinScout);
            }
            else if (cellStrength == 1)
            {
                monsters.Add(GameAssets.i.OrcDefender);
                monsters.Add(GameAssets.i.OrcDefender);
                monsters.Add(GameAssets.i.GoblinHunter);
                monsters.Add(GameAssets.i.GoblinHunter);
            }
            else
            {
                monsters.Add(GameAssets.i.OrcBrute);
                monsters.Add(GameAssets.i.OrcBrute);
                monsters.Add(GameAssets.i.GoblinScout);
                monsters.Add(GameAssets.i.GoblinScout);
                monsters.Add(GameAssets.i.GoblinScout);
            }
        }
        return monsters;
    }

    public static EnemyCompany CreateEnemyCompany(Encounter encounter)
    {
        EnemyCompany ec = new EnemyCompany();
        int roll;
        int cellStrength = 0;
        bool createCell;

        //Detirmine monster type.
        roll = UnityEngine.Random.Range(0, 2);
        switch (roll)
        {
            case 0: encounter.enemyType = EnemyType.BANDITS; break;
            case 1: encounter.enemyType = EnemyType.GOBLINOIDS; break;
            default: break;
        }

        for (int cellCount = encounter.arenaSize -1; cellCount > 0 && ec.cells.Count < 3; cellCount--)
        {
            roll = UnityEngine.Random.Range(0,6) + encounter.difficulty;
            createCell = true;
            switch (roll)
            {
                case 0: createCell = false; break;
                case 1: createCell = false; break;
                case 2: cellStrength = 0; break;
                case 3: cellStrength = 1; break;
                case 4: cellStrength = 2; break;
                default: cellStrength = 2; break;
            }
            if (createCell)
            {
                ec.AddCell(LookUpEnemyCellContents(ec, cellStrength, encounter.enemyType), cellStrength);
            }
        }
        return ec;
    } 
}

public class EnemyCompany
{
    public List<Cell> cells = new List<Cell>();
    public bool leaderAdded;

    public class Cell
    {
        public int cellStrength;
        public List<GameObject> enemies;
    }

    public void AddCell(List<GameObject> contents, int cellStrength)
    {
        Cell cell = new Cell();
        cell.cellStrength = cellStrength;
        cell.enemies = contents;
        cells.Add(cell);
    }
}