using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class CrosshairManager : MonoBehaviour
{
    public Camera camPos;

    private Image crosshair;
    private float rayDistnace = 50f;
    private bool showCrosshair;

    private void Awake()
    {
        crosshair = GetComponent<Image>();    
    }
    private void Start()
    {
        CrosshairZero();
    }

    public void CrosshairZero()
    {
        showCrosshair = false;
        CrosshairLogic(0f);
    }

    public void DefaultColor()
    {
        showCrosshair = true;
        CrosshairLogic(.3f);
    }

    private void Update()
    {
        if (!showCrosshair) 
        {
            return;
        }
        RaycastHit hit;
        if(Physics.Raycast(camPos.transform.position, camPos.transform.forward, out hit, rayDistnace))
        {
            if(hit.collider.CompareTag("Player"))
            {
                crosshair.color = Color.red;

                return;
            }
        }
        if (crosshair.color == Color.red)
        {
            DefaultColor();
        }
    }

    private void CrosshairLogic(float value)
    {
        crosshair.color = Color.white;
        Color currentColor = crosshair.color;
        currentColor.a = value;
        crosshair.color = currentColor;
    }
}
