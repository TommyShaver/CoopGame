using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
using System;
using TMPro;

public class PlayerManager : MonoBehaviour, IDamageable, IGotItem
{
    public GameObject[] v_cams;
    public CinemachineBasicMultiChannelPerlin shakeCamer;
    public PlayerDamageMeter playerDamageMeter;
    public CrosshairManager crosshairManager;
    public TextMeshProUGUI gernadesTextCount;
    public TextMeshProUGUI helperItemTextCount;

    //Grab scripts --------------------------------------
    private InputManager inputManager;
    private PlayerAnimation playerAnimation;
    private PlayerBlink playerBlink;
    private PlayerLookAround playerLookAround;
    private PlayerMotor playerMotor;

    //Inventor -----------------------------------------
    private int gernadeCount;
    private int slowdownZoneCount;
    private int landmineCount;
    private int forceFeildCount;

    //Health -------------------------------------------
    private int health = 100;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerBlink = GetComponent<PlayerBlink>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerLookAround = GetComponent<PlayerLookAround>();
        playerMotor = GetComponent<PlayerMotor>();
    }
    private void Start()
    {
        WhichCamera(0);
    }



    //Health --------------------------------------------
    private void PlayerHealth()
    {

    }
    
    private void PlayerDed()
    {
        inputManager.CanMoveAround(false);
        playerMotor.CanUseHands(false);
        crosshairManager.CrosshairZero();
    }

    public void Damage(int damange, float yRotationHit)
    {
        float camShakeDuration = .2f;
        shakeCamer.AmplitudeGain = damange;
        shakeCamer.FrequencyGain = 1;
        if (damange > 5) 
        {
            camShakeDuration = 1; 
        }
        playerDamageMeter.UI_DamangeMeter(yRotationHit, HitYReturn(gameObject));
        StartCoroutine(CleanUpTimer(CleanUpShakingCameras, camShakeDuration));
        //Playsound effect
        //Health
    }










    //Cameras -------------------------------------------
    private void WhichCamera(int cam)
    {
        v_cams[cam].SetActive(true);
        for (int i = 0; i < v_cams.Length; i++) 
        {
            if(i != cam)
            {
                v_cams [i].SetActive(false);
            }
        }
        switch (cam) 
        {
            case 0:
                WhichCamera(1);
                StartCoroutine(CleanUpTimer(OnCameraSwitch, 1));
                break;
            case 1:
                
                break;
            case 2:
                break;
            default:
                WhichCamera(1);
                StartCoroutine(CleanUpTimer(OnCameraSwitch, 1));
                break;
        }
    }

    private void OnCameraSwitch()
    {
        inputManager.CanMoveAround(true);
        playerMotor.CanUseHands(true);
        crosshairManager.DefaultColor();
    }









    //Inventroy ------------------------------------------------
    public void IGotItem(string item)
    {
        switch (item)
        {
            case "SlowDownZone":
                slowdownZoneCount++;
                break;
            case "Gerande":
                gernadeCount++;
                break;
            case "Sheild":
                forceFeildCount++;
                break;
            case "Landmine":
                landmineCount++;
                break;
        }
    }
    public bool DoIHaveItem(int itemSlot)
    {
        bool doWeHaveItem = false;
        switch (itemSlot)
        {
            case 0:
                if(slowdownZoneCount > 0) { doWeHaveItem = true; }
                break;
            case 1:
                if(forceFeildCount > 0) { doWeHaveItem = true; }
                break;
            case 2:
                if (landmineCount > 0) { doWeHaveItem = true; }
                break;
            case 3:
                if (gernadeCount > 0) { doWeHaveItem = true; }
                break;
        }
        return doWeHaveItem;
    }
    public void WhichHelperItemSelected(int item)
    {
        playerMotor.WhichHelperItem(item);
    }













    //Clean up function -----------------------------------------
    private void CleanUpShakingCameras()
    {
        shakeCamer.AmplitudeGain = 0;
        shakeCamer.FrequencyGain = 0;
    }

    //Helper Function ------------------------------------
    private IEnumerator CleanUpTimer(Action action, float time)
    {
        yield return new WaitForSeconds(time);
        action();
    }

    //Return Y in interface
    private float HitYReturn(GameObject ob)
    {
        Vector3 objFwd = ob.transform.forward;
        float angle = Vector3.Angle(objFwd, Vector3.forward);
        float sign = Mathf.Sign(Vector3.Cross(objFwd, Vector3.forward).y);
        return angle * sign;
    }
}
