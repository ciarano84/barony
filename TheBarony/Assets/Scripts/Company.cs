using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CompanyInfo
{
    public string companyName;
    public Transform origin;
    public Transform destination;
    public TransformSave originSave;
    public TransformSave destinationSave;
    public TransformSave currentLocationSave;
    public Encounter targetEncounter;
    public List<UnitInfo> units = new List<UnitInfo>();
    public float travelSpeed = 0.04f;
    public Company company;

    public void CreateCompany()
    {
        GameObject go = GameObject.Instantiate(GameAssets.i.company);
        Company newCompany = go.GetComponent<Company>();
        newCompany.companyInfo = this;
        company = newCompany;
        origin = TransformSave.ReturnTransform(originSave);
        destination = TransformSave.ReturnTransform(destinationSave);
        if (currentLocationSave != null)
        {
            newCompany.transform.position = TransformSave.ReturnTransform(currentLocationSave).position;
        }
        else newCompany.transform.position = origin.position;
        company.mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        company.mapManager.companies.Add(company);
    }
}

public class Company : MonoBehaviour
{
    public CompanyInfo companyInfo;
    bool moving = false;
    public MapManager mapManager;

    float timer = 0;

    private void Update()
    {
        if (!moving) return;
        else 
        {
            bool endMove = false;
            MapUIManager.uiState = MapUIManager.UIState.noInput;

            if (companyInfo.targetEncounter == null)
            {
                Debug.LogWarning("No encounter to travel to, heading back");
                companyInfo.destination = MapManager.theCastle.transform;
            }
            
            transform.position = Vector3.MoveTowards(transform.position, companyInfo.destination.position, companyInfo.travelSpeed);
            
            //check to see if it has reached its destination. 
            if (Vector3.Distance(transform.position, companyInfo.destination.transform.position) <= 0.01f)
            {
                companyInfo.currentLocationSave = TransformSave.StoreTransform(companyInfo.destination.gameObject);
            }
            
            timer += Time.deltaTime;
            if (timer > 1) endMove = true;
            
            if (endMove)
            {
                companyInfo.currentLocationSave = TransformSave.StoreTransform(gameObject);
                MapUIManager.uiState = MapUIManager.UIState.standard;
                mapManager.CheckForAvailableEncounters();
                mapManager.ReturnCompanysToCastle();
                moving = false;
                timer = 0;
                return;
            }
        }
    }

    public void Travel()
    {
        moving = true;
    }
}
