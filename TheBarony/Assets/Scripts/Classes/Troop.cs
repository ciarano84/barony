using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troop : TacticsMovement
{
    public int moveSpeed;
    public int health;
    public int damage;

    private void Start()
    {
        Init();
    }

}
