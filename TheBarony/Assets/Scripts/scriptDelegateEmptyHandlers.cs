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
        AttackManager.OnWound += OnGrazeEmptyHandler;
        TacticsMovement.OnDodge += OnDodgeEmptyHandler;
        AttackManager.OnAttack += OnGrazeEmptyHandler;
        Unit.onKO += OnEncounterStartEmptyHandler;
        Initiative.OnTurnStart += OnEncounterStartEmptyHandler;
        Effect.OnEffectEnd += OnEffectEndHandler;
        Initiative.OnActionTaken += OnEncounterStartEmptyHandler;
        Unit.onSetFocus += OnEncounterStartEmptyHandler;
        //Missile.OnMissileHit += OnEncounterStartEmptyHandler;
    }

    void OnEncounterStartEmptyHandler(Unit unit) { }
    void OnGrazeEmptyHandler(Unit unit, Unit unit2) { }
    void OnEffectEndHandler(Unit unit, Effect effect) { }
    void OnDodgeEmptyHandler(Unit defender, Result result) { }
}
