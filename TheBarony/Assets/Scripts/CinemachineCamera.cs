using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineCamera : MonoBehaviour
{
    static Unit unitCurrentlyFollowed;
    static bool ready = false;
    public float rotateSpeed;

    static GameObject[] dollies;
    static GameObject[] vcams;

    public static void GetCameras()
    {
        dollies = GameObject.FindGameObjectsWithTag("dolly");
        vcams = GameObject.FindGameObjectsWithTag("vcam");
        ready = true;
    }

    void FixedUpdate()
    {
        if (ready)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                RotateVirtualCams(true);
            }
            if (Input.GetKey(KeyCode.E))
            {
                RotateVirtualCams(false);
            }
        }
    }

    public static void FollowUnit(TacticsMovement unit)
    {
        if (unitCurrentlyFollowed != null)
        {
            unit.vcam.transform.position = unitCurrentlyFollowed.transform.position;
        }
        unit.vcam.MoveToTopOfPrioritySubqueue();
        unitCurrentlyFollowed = unit;
    }

    void RotateVirtualCams(bool clockwise)
    {
        int rotationDirection = 1;
        if (!clockwise) rotationDirection = -1;

        for (int count = 0; count < dollies.Length; count++)
        {
            dollies[count].transform.Rotate((Vector3.up * rotateSpeed * Time.deltaTime) * rotationDirection);
        }
    }
}
