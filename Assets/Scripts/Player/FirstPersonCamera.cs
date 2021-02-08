using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public Transform playerBody;
    public float lookSens = 100f;

    private float xRotation = 0f;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //RotateCamera();
    }
    void FixedUpdate()
    {
        RotateCameraFixedUpdate();
    }
    void RotateCamera()
    {
        float _mouseX = Input.GetAxis("Mouse X") * lookSens * Time.deltaTime;
        float _mouseY = Input.GetAxis("Mouse Y") * lookSens * Time.deltaTime;

        xRotation -= _mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * _mouseX);
    }

    void RotateCameraFixedUpdate()
    {
        float _mouseX = Input.GetAxis("Mouse X") * lookSens; // 0.02f
        float _mouseY = Input.GetAxis("Mouse Y") * lookSens; // 0.02f

        xRotation -= _mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * _mouseX);
    }
}
