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
    public float flySpeed;
    static bool playerControl = false;
    public CinemachineVirtualCamera playerCam;
    public GameObject playerCamDolly;

    static GameObject[] dollies;

    public static void GetCameras()
    {
        dollies = GameObject.FindGameObjectsWithTag("dolly");
        //vcams = GameObject.FindGameObjectsWithTag("vcam");
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

            zoomLevel = Mathf.Clamp(zoomLevel + -Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, minZoom, maxZoom);
            unitCurrentlyFollowed.GetComponent<TacticsMovement>().vcam.m_Lens.FieldOfView = zoomLevel;
            if (playerControl) playerCam.m_Lens.FieldOfView = zoomLevel;

            if (Input.GetKey(KeyCode.W))
            {
                if (playerControl == false)
                {
                    EnablePlayerControl();
                }
                playerCamDolly.transform.Translate(Vector3.forward * flySpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                if (playerControl == false)
                {
                    EnablePlayerControl();
                }
                playerCamDolly.transform.Translate(Vector3.forward * -flySpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A))
            {
                if (playerControl == false)
                {
                    EnablePlayerControl();
                }
                playerCamDolly.transform.Translate(Vector3.left * flySpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                if (playerControl == false)
                {
                    EnablePlayerControl();
                }
                playerCamDolly.transform.Translate(Vector3.right * flySpeed * Time.deltaTime);
            }
        }
    }

    public static void FollowUnit(TacticsMovement unit)
    {
        if (unitCurrentlyFollowed != null)
        {
            unit.vcam.transform.position = unitCurrentlyFollowed.transform.position;
        }

        //Move the new unit's camera to be top of the subqueue.
        playerControl = false;
        unit.vcam.MoveToTopOfPrioritySubqueue();
        unitCurrentlyFollowed = unit;
    }

    void RotateVirtualCams(bool clockwise)
    {
        int rotationDirection = 1;
        if (!clockwise) rotationDirection = -1;

        for (int count = 0; count < dollies.Length; count++)
        {
            if (dollies[count] != null)
            {
                dollies[count].transform.Rotate((Vector3.up * rotateSpeed * Time.deltaTime) * rotationDirection);
            }
        }

        playerCamDolly.transform.Rotate((Vector3.up * rotateSpeed * Time.deltaTime) * rotationDirection);
    }

    void EnablePlayerControl()
    {
        playerControl = true;
        playerCam.m_Lens.FieldOfView = zoomLevel;
        playerCamDolly.transform.position = Initiative.currentUnit.dolly.transform.position;
        playerCamDolly.transform.eulerAngles = Initiative.currentUnit.dolly.transform.eulerAngles;
        playerCam.MoveToTopOfPrioritySubqueue();
    }
}
