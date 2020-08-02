using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RostaInfo : MonoBehaviour
{
    public List<UnitInfo> castle = new List<UnitInfo>();
    public List<UnitInfo> squad = new List<UnitInfo>();
    public List<CompanyInfo> companies = new List<CompanyInfo>();

    //used to detirmine a character's position in the squadview.
    public int currentUnitShown = 0;
    public int companyPosition;
    
    //The encounter currently being handled. 
    public static Encounter currentEncounter;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
