using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class CrosshairManager : MonoBehaviour
{
    public Camera camPos;

    private Image crosshair;
    private float rayDistnace = 25f;
    private void Awake()
    {
        crosshair = GetComponent<Image>();
    }

    private void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(camPos.transform.position, camPos.transform.forward, out hit, rayDistnace))
        {
            if(hit.collider.CompareTag("Player"))
            {
                crosshair.color = Color.red;
            }
            return;
        }

        if(crosshair.color == Color.red)
        {
            crosshair.color = Color.white;
        }
    }
}
