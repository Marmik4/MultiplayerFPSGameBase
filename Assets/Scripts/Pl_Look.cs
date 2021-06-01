using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Pl_Look : MonoBehaviourPunCallbacks
{
    public float SensitivityX = 100f;
    public float SensitivityY = 100f;
    public float min_Angle = 0f;
    public float max_Angle = 90f;
    float xRotation = 0f;

    public Transform playerBody;
    public Camera FPSCam;
    //public Transform weaponTarget;
    private float mouseX;
    private float mouseY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        Vector3 forward = FPSCam.transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(FPSCam.transform.position, forward, Color.green);

        mouseX = Input.GetAxis("Mouse X")*SensitivityX*Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y")*SensitivityY*Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation,min_Angle, max_Angle); 

        FPSCam.transform.localRotation= Quaternion.Euler(xRotation, 0f, 0f);
        //weaponTarget.rotation = FPSCam.transform.localRotation;
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
