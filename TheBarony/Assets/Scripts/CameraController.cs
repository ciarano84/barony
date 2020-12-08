using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public LayerMask whatCanBeClickedOn;
    public Vector3 offset;
    public float rotateSpeed;
    public float zoomSpeed;
    public float flySpeed;
    public GameObject lens;
    private Vector3 focus;
    static bool ready = false;

    static GameObject[] dollies;
    static GameObject[] vcams;

    public static void GetCameras()
    {
        //focus = transform.position;
        dollies = GameObject.FindGameObjectsWithTag("dolly");
        vcams = GameObject.FindGameObjectsWithTag("vcam");
        ready = true;
    }
    void FixedUpdate()
    {
        if (ready)
        {
            //working with cinemachine
            if (Input.GetKey(KeyCode.Q))
            {
                RotateVirtualCams(true);
            }
            if (Input.GetKey(KeyCode.E))
            {
                RotateVirtualCams(false);
            }






            //obsolete with cinemachine

            //zooms the camera. 
            lens.transform.Translate(Vector3.forward * Input.GetAxis("Mouse ScrollWheel") * zoomSpeed);

            // The basic click to relocate the camera 
            if (Input.GetMouseButtonDown(1))
            {
                Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;

                if (Physics.Raycast(myRay, out hitInfo, 100, whatCanBeClickedOn))
                {
                    transform.position = hitInfo.point;
                }
            }
            
            if (Input.GetKey(KeyCode.W))
            {
                //transform.Translate(Vector3.forward * flySpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                //transform.Translate(Vector3.forward * -flySpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A))
            {
                //transform.Translate(Vector3.left * flySpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                //transform.Translate(Vector3.right * flySpeed * Time.deltaTime);
            }
        }
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