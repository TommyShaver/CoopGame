using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;


public class InputManager : MonoBehaviour
{
    public PlayerHelperItem playerHelperItem;
    private PlayerMovement playerMovment;
    private PlayerMovement.PlayerMoveActions movementOfPlayer;
    private PlayerMovement.PlayerActionsActions playerAction;
    private PlayerMovement.PlayerUIActions playerUIActions;

    private PlayerMotor motor;
    private PlayerLookAround lookAround;

    private bool isReady;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        //Control inputs ------------------------------------------------------------------
        playerMovment = new PlayerMovement();
        motor = GetComponent<PlayerMotor>();
        lookAround = GetComponent<PlayerLookAround>();
        movementOfPlayer = playerMovment.PlayerMove;
        playerAction = playerMovment.PlayerActions;

        //Single pressed inputs -------------------------------------------------------------------
        //Jump -----------------------------------------------------------------
        playerAction.Jump.performed += ctx => motor.Jump();

        //Shoot ----------------------------------------------------------------
        playerAction.FireGun.performed += ctx => motor.ShootWeapon();

        //Left Hand ------------------------------------------------------------
        playerAction.TossGrenade.performed += ctx => motor.ThorwGrenade();
        playerAction.Melee.performed += ctx => motor.Punch();
        playerAction.UseItem.performed += ctx => motor.ThorwHelperItem();
        playerAction.SwitchItems.performed += ctx => playerHelperItem.SelectHelperItem();
        playerAction.SwitchItemMouse.performed += ctx => playerHelperItem.SelectHelperItem();

        //Zoom -----------------------------------------------------------------
        playerAction.Zoom.performed += ctx => motor.AimSpeed(true);
        playerAction.Zoom.canceled += ctx => motor.AimSpeed(false);
        playerAction.Zoom.performed += ctx => lookAround.AimHold(true);
        playerAction.Zoom.canceled += ctx => lookAround.AimHold(false);

        //Menu Inputs -----------------------------------------------------------------------------

    }
    
    private void OnEnable()
    {
        movementOfPlayer.Enable();
        playerAction.Enable();
        playerAction.Score.Enable();
        movementOfPlayer.LookAround.performed += WhichController; 
    }

    private void OnDisable()
    {
        movementOfPlayer.Disable();
        playerAction.Disable();
        playerAction.Score.Disable();
        movementOfPlayer.LookAround.performed -= WhichController;
    }


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //Don't let different display effect mouse movemnt.
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!isReady)
        {
            return;
        }
        motor.ProcesMove(movementOfPlayer.Movement.ReadValue<Vector2>());
        lookAround.ProcessLook(movementOfPlayer.LookAround.ReadValue<Vector2>());
    }
  
    //In bound commands ---------------------------------------------------------
    public void CanMoveAround(bool canUse)
    {
        isReady = canUse;
    }

    // Checks which controller is being used -----------------------------------
    private void WhichController(InputAction.CallbackContext context)
    {
        var device = context.control.device;

        switch (device)
        {
            case Gamepad:
                lookAround.ControlSpeed(true);
                break;
            case Mouse:
                lookAround.ControlSpeed(false);
                break;
        }
    }
}
