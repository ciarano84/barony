﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptDelegateEmptyHandlers : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Initiative.OnEncounterStart += OnEncounterStartEmptyHandler;
        TacticsMovement.OnEnterSquare += OnEncounterStartEmptyHandler;
        AttackManager.OnGraze += OnGrazeEmptyHandler;
        AttackManager.OnAttack += OnGrazeEmptyHandler;
        Unit.onKO += OnEncounterStartEmptyHandler;
        Initiative.OnTurnStart += OnEncounterStartEmptyHandler;
    }

    void OnEncounterStartEmptyHandler(Unit unit) { }
    void OnGrazeEmptyHandler(Unit unit, Unit unit2) { }
}
