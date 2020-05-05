using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    GameObject followedTroop;
    float offset = 1.75f;

    //adjust this to change speed
    float speed = 2f;

    float height = 0.2f;

    //Bobs the selector (horrid function that needs clearing up, too much on update). 
    void Update()
    {
        //get the objects current position and put it in a variable so we can access it later with less code
        Vector3 pos = transform.position;
        //calculate what the new Y position will be
        float newY = Mathf.Sin(Time.time * speed);
        //set the object's Y to the new calculated Y
        transform.position = new Vector3(pos.x, (newY * height), pos.z);
        transform.position = new Vector3(followedTroop.transform.position.x, transform.position.y + offset, followedTroop.transform.position.z);
    }

    public void AssignSelector(GameObject troop) {
        followedTroop = troop;
    }
}
