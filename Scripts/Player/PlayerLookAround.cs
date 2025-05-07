using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLookAround : MonoBehaviour
{
    private float xRotation = 0f;

    public Camera cam;
    public float xSensitivity = 30f;
    public float ySensitivity = 30f;


    public void ProcessLook(Vector2 input)
    {
        float lookX = input.x;
        float lookY = input.y;

        xRotation -= (lookY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 30f);
        cam.transform.localRotation = Quaternion.Euler(xRotation + 10, 0f, 0f);
        transform.Rotate(Vector3.up * (lookX * Time.deltaTime) * xSensitivity);
    }

    public void ControlSpeed(string whichController)
    {
        if (whichController == "Gamepad")
        {
            xSensitivity = 300f;
            ySensitivity = 300f;
        }
        else
        {
            xSensitivity = 5f;
            ySensitivity = 5f;
        }

    }
}
