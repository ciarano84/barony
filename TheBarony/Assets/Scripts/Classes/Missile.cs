using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Missile : MonoBehaviour
{
    float speed = 16;
    public Vector3 target;

    private void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;

        //if it reaches the object

        //if it lives for too long
    }
}
