using UnityEngine;
using System.Collections;
public class PlayerHeadRotate : MonoBehaviour
{
    private float xRotation = 0f;

    public GameObject playerMouth;
    public float xSensitivity = 30f;
    public float ySensitivity = 20f;

    public void ProcessLook(Vector2 input)
    {
        float lookX = input.x;
        float lookY = input.y;

        xRotation -= (lookY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 30f);
        playerMouth.transform.localRotation = Quaternion.Euler(xRotation + 10, 0f, 0f);
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
