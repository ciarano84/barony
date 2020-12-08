using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineCamera : MonoBehaviour
{
    static Unit unitCurrentlyFollowed;
    static bool ready = false;
    public float rotateSpeed;
    public float zoomSpeed;
    public float minZoom, maxZoom;
    public float zoomLevel;

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

            Initiative.currentUnit.vcam.m_Lens.FieldOfView += -Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            if (Initiative.currentUnit.vcam.m_Lens.FieldOfView > maxZoom) Initiative.currentUnit.vcam.m_Lens.FieldOfView = maxZoom;
            else if (Initiative.currentUnit.vcam.m_Lens.FieldOfView < minZoom) Initiative.currentUnit.vcam.m_Lens.FieldOfView = minZoom;
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
