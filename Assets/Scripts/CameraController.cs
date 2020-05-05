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

    private void Start()
    {
        focus = this.transform.position;
    }
    void Update()
    {
       //zooms the camera. 
        lens.transform.Translate(Vector3.forward * Input.GetAxis("Mouse ScrollWheel") * zoomSpeed);

        // The basic click to relocate the camera 
        if (Input.GetMouseButtonDown(1))
        {
            Ray myRay = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(myRay, out hitInfo, 100, whatCanBeClickedOn))
            {
                transform.position = hitInfo.point;
            }
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up * -rotateSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * flySpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.forward * -flySpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * flySpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * flySpeed * Time.deltaTime);
        }
    }
}