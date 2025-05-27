using UnityEngine;
using DG.Tweening;
using System.Linq;

public class PlayerLookAround : MonoBehaviour
{

    public Camera cam;
    public GameObject playerMouth;
    
    private float xSensitivity;
    private float ySensitivity;
    private float xRotation;
    private bool isGamepad;
    private void Start()
    {
        ControllerDefaultsUI();
    }
    public void ProcessLook(Vector2 input)
    {
        float lookX = input.x;
        float lookY = input.y;

        xRotation -= (lookY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -70f, 90f);
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerMouth.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * (lookX * Time.deltaTime) * xSensitivity);
        //Debug.Log("lookX:" + lookX + " xSen:" + xSensitivity);
    }

    public void ControlSpeed(bool _isController)
    {
        if(_isController == isGamepad)
        {
            return;
        }


        if (_isController == true)
        {
           xSensitivity = 110f;
           ySensitivity = 100f;
            isGamepad = true;
        }
        else
        {
            xSensitivity = 10f;
            ySensitivity = 10f;
            isGamepad = false;
        }

    }


    //Aiming ---------------------------------------------------------
    public void AimHold(bool isAiming)
    {
        if (isAiming)
        {
            ControllerSpeed();
        }
        else
        {
            ControllerDefaultsUI();
        }
    }
    private void ControllerSpeed()
    {
        if (isGamepad)
        {
            xSensitivity = 50f;
            ySensitivity = 30f;
        }
        else
        {
            xSensitivity = 2f;
            ySensitivity = 1f;
        }
    }
    private void ControllerDefaultsUI()
    {
        if (isGamepad)
        {
           xSensitivity = 110f;
           ySensitivity = 100f;
        }
        else
        {
            xSensitivity = 10f;
            ySensitivity = 10f;
        }
    }
}
