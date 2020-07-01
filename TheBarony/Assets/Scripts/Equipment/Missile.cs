using System.Collections;
using System.Collections.Generic;
// System.Numerics;
using UnityEngine;
using UnityEngine.Animations;

public class Missile : MonoBehaviour
{
    readonly float speed = 16;
    float step;
    public Vector3 target;
    public Vector3 missVariance = new Vector3(1, 1, 0);
    public bool launched = false;
    public bool hit = false;
    Vector3 dir;


    public void Launch(bool onTarget)
    {
        if (onTarget)
        {
            dir = target - transform.position;
        }
        else
        {
            dir = (target + missVariance) - transform.position;
        }
        launched = true;
        step = speed * Time.deltaTime;
        Destroy(this.gameObject, 2);
    }

    private void Update()
    {
        transform.Translate(dir.normalized * step, Space.World);
        if (Vector3.Distance(this.transform.position, target) < 0.25) Destroy(this.gameObject);

        //Might need this to accomdate for aiming at the feet. 
        //Vector3 direction = target.unitTargeted.transform.position;
        //direction.y += target.unitTargeted.GetComponent<TacticsMovement>().halfHeight;
    }
}
