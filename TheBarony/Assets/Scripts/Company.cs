using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CompanyInfo
{
    public string companyName;
    public Transform origin;
    public Transform destination;
    public Encounter targetEncounter;
    public List<UnitInfo> units = new List<UnitInfo>();
    public float travelSpeed = 0.01f;
    public Company company;

    public void CreateCompany()
    {
        GameObject go = GameObject.Instantiate(GameAssets.i.company);
        Company newCompany = go.GetComponent<Company>();
        newCompany.companyInfo = this;
        company = newCompany;
        newCompany.transform.position = origin.transform.position;
    }
}

public class Company : MonoBehaviour
{
    public CompanyInfo companyInfo;
    bool moving = false;
    
    float timer = 0;

    private void Update()
    {
        if (!moving) return;
        else 
        {
            if (companyInfo.targetEncounter == null)
            {
                Debug.LogWarning("No encounter to travel to, heading back");
                companyInfo.destination = MapManager.theCastle.transform;
            }
            
            transform.position = Vector3.MoveTowards(transform.position, companyInfo.destination.position, companyInfo.travelSpeed);
            timer += Time.deltaTime;
            if (timer > 1)
            {
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
