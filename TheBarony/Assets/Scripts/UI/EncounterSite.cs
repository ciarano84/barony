﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EncounterSite : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData data)
    {
        // This will only execute if the objects collider was the first hit by the click's raycast
        Debug.Log(gameObject.name + ": I was clicked!");
    }

    public string SiteName;
    public bool reclaim = true;
    public Encounter encounter = null;
    public GameObject buttonIcon;

    public void ShowEncounter()
    {
        buttonIcon.SetActive(true);
    }

    public void ClearEncounter()
    {
        buttonIcon.SetActive(false);
        encounter = null;
    }

    private void OnMouseDown()
    {
        if (encounter != null && MapManager.uiState == MapManager.UIState.standard && !EventSystem.current.IsPointerOverGameObject())
        {
            encounter.Selected();
        }
    }


}
