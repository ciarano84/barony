using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;

public class TurnManager : MonoBehaviour
{
    static Dictionary<string, List<TacticsMovement>> units = new Dictionary<string, List<TacticsMovement>>();
    static Queue<string> turnKey = new Queue<string>();
    static Queue<TacticsMovement> turnTeam = new Queue<TacticsMovement>();

    private void Update()
    {
        
        if (turnTeam.Count == 0)
        {
            InitTeamTurnQueue();
        }
    }

    static void InitTeamTurnQueue()
    {
        List<TacticsMovement> teamList = units[turnKey.Peek()];

        foreach (TacticsMovement unit in teamList)
        {
            turnTeam.Enqueue(unit);
        }

        StartTurn();
    }

    //I chose to make this public, against what happens in the vid. 
    //I chose to NOT make this public, against what happens in the vid. 
    static void StartTurn()
    {
        if (turnTeam.Count > 0)
        {
            turnTeam.Peek().BeginTurn();
        }
    }

    public static void EndTurn()
    {
        TacticsMovement unit = turnTeam.Dequeue();
        unit.EndTurn();

        if (turnTeam.Count > 0)
        {
            StartTurn();
        }
        else {
            string team = turnKey.Dequeue();
            turnKey.Enqueue(team);
        }
    }

    public static void AddUnit(TacticsMovement unit) 
    {
        List<TacticsMovement> list;

        if (!units.ContainsKey(unit.tag))
        {
            list = new List<TacticsMovement>();
            units[unit.tag] = list;
            if (!turnKey.Contains(unit.tag))
            {
                turnKey.Enqueue(unit.tag);
            }
        }
        else
        {
            list = units[unit.tag];
        }

        list.Add(unit);
    }

    //You'll need a remove function to remove a unit from the queue, for when it is defeated. 
    //You'll also need to handle having an entire team removed. 
}
