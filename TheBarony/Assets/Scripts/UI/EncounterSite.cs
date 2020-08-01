using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterSite : MonoBehaviour
{
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
}
