using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PL_Look_Alt : MonoBehaviour
{
    public static bool cursorLocked = true;

    public Transform FPSCam;
    public Transform player;
    public Transform weaponTarget;
    public float maxAngle = 30f;

    private Quaternion camCenter;
    public float ySensitivity;
    public float xSensitivity;

    void Start()
    {
        camCenter = FPSCam.localRotation;
    }

    void Update()
    {
        SetX();
        SetY();

        UpdateCursor();
    }

    void SetY()
    {
        float t_input = Input.GetAxis("Mouse Y") * ySensitivity * Time.deltaTime;
        Quaternion t_adj = Quaternion.AngleAxis(t_input, -Vector3.right);
        Quaternion t_delta = FPSCam.localRotation * t_adj;
        
        if(Quaternion.Angle(camCenter,t_delta)<maxAngle)
        {
            FPSCam.localRotation = t_delta;
        }
        weaponTarget.rotation = FPSCam.rotation;
    }

    void SetX()
    {
        float t_input = Input.GetAxis("Mouse X") * xSensitivity * Time.deltaTime;
        Quaternion t_adj = Quaternion.AngleAxis(t_input, Vector3.up);
        Quaternion t_delta = player.localRotation * t_adj;
        player.localRotation = t_delta;
    }

    void UpdateCursor()
    {
        if(cursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
