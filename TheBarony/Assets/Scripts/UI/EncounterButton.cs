using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EncounterButton : MonoBehaviour
{
    public Encounter encounter;
    public Company company;
    public Text buttonText;

    public void StartEncounter()
    {
        encounter.selectedCompany = company.companyInfo;
        encounter.EncounterButtonSelected();
    }
}
