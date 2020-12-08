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
            if (Input.GetKey(KeyCode.Q))
            {
                Debug.Log("key down registered.");
                //transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
                RotateVirtualCams();
            }
            if (Input.GetKey(KeyCode.E))
            {
                //transform.Rotate(Vector3.up * -rotateSpeed * Time.deltaTime);
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

    void RotateVirtualCams()
    {
        for (int count = 0; count < dollies.Length; count++)
        {
            dollies[count].transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
        }


            //for (int count = 0; count < dollies.Length; count++)
            //{
            //    vcams[count].GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().FollowTargetRotation



            //    //vcams[count].transform.SetParent(dollies[count].transform);
            //    //dollies[count].transform.Rotate(direction * rotateSpeed * Time.deltaTime);
            //    //vcams[count].transform.SetParent(null);


            ////another attempt
            ////vcams[count].transform.RotateAround(dollies[count].transform.position, Vector3.up, 100 * Time.deltaTime);
            //}
    }
}