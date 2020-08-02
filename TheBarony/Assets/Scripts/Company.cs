using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Company : MonoBehaviour
{ 
    public Transform origin;
    public Transform destination;
    public Encounter targetEncounter;
    public List<Unit> units = new List<Unit>();
    bool moving = false;
    float travelSpeed = 0.01f;
    float timer = 0;

    private void Update()
    {
        if (!moving) return;
        else 
        {
            if (targetEncounter == null)
            {
                Debug.LogWarning("No encounter to travel to, heading back");
                destination = MapManager.theCastle.transform;
            }
            
            transform.position = Vector3.MoveTowards(transform.position, destination.position, travelSpeed);
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
