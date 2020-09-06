using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaPoint : MonoBehaviour
{
    public GameObject block;

    private void Start()
    {
        int rotationRoll = Random.Range(0, 4);
        float rotation;

        switch (rotationRoll)
        {
            case 1:
                rotation = 90f;
                break;
            case 2:
                rotation = 180f;
                break;
            case 3:
                rotation = 270f;
                break;
            default:
                rotation = 0;
                break;
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotation, transform.eulerAngles.z);
    }
}
