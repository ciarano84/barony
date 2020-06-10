using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Missile : MonoBehaviour
{
    readonly float speed = 16;
    Transform startingPoint;
    float step;
    public Vector3 target;
    public bool launched = false;
    public bool hit = false;

    public void Start()
    {
        startingPoint = this.transform;
    }

    public void Launch(bool onTarget)
    {
        launched = true;
        Destroy(this.gameObject, 2);
        if (onTarget) hit = true;
    }

    private void Update()
    {
        if (launched)
        {
            step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(startingPoint.position, target, step);
        }

        if (hit)
        {
            if (Vector3.Distance(this.transform.position, target) < 0.02) Destroy(this.gameObject);
        }
            
            

        //Might need this to accomdate for aiming at the feet. 
        //Vector3 direction = target.unitTargeted.transform.position;
        //direction.y += target.unitTargeted.GetComponent<TacticsMovement>().halfHeight;

    }
}
