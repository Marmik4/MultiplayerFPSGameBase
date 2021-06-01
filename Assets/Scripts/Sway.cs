using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    public float intensity;
    public float smoothness;

    private Quaternion origin_Rotation;
    void Start()
    {
        origin_Rotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        updateSway();
    }

    void updateSway()
    {
        float x_move = Input.GetAxis("Mouse X");
        float y_move = Input.GetAxis("Mouse Y");

        Quaternion x_adj = Quaternion.AngleAxis(-intensity * x_move, Vector3.up);
        Quaternion y_adj = Quaternion.AngleAxis(intensity * x_move, Vector3.right);
        Quaternion target_rotation = origin_Rotation * x_adj * y_adj;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, target_rotation, Time.deltaTime * smoothness);
    }
}
