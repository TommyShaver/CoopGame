using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;


public class InputManager : MonoBehaviour
{
    private PlayerMovement playerMovment;
    private PlayerMovement.PlayerMoveActions movementOfPlayer;
    private PlayerMovement.PlayerActionsActions playerAction;

    private PlayerMotor motor;
    private PlayerLookAround lookAround;
    private PlayerHeadRotate headRotate;

    private bool isReady;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        playerMovment = new PlayerMovement();
        motor = GetComponent<PlayerMotor>();
        lookAround = GetComponent<PlayerLookAround>();
        headRotate = GetComponent<PlayerHeadRotate>();
        movementOfPlayer = playerMovment.PlayerMove;
        playerAction = playerMovment.PlayerActions;
        playerAction.Jump.performed += ctx => motor.Jump();
        playerAction.Jump.performed += ctx => motor.Dash();
        playerAction.FireGun.performed += ctx => motor.ShootWeapon();
        playerAction.TossGrenade.performed += ctx => motor.ThorwGrenade();
    }
    
    private void OnEnable()
    {
        movementOfPlayer.Enable();
        playerAction.Enable();
        movementOfPlayer.LookAround.performed += WhichController; 
    }

    private void OnDisable()
    {
        movementOfPlayer.Disable();
        playerAction.Disable();
        movementOfPlayer.LookAround.performed -= WhichController;
    }


    private void Start()
    {
        StartCoroutine(Hold()); //Wait for game to true load.
        Cursor.lockState = CursorLockMode.Locked; //Don't let different display effect mouse movemnt.
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        motor.ProcesMove(movementOfPlayer.Movement.ReadValue<Vector2>());
    }
    private void Update()
    {
        if (!isReady)
        {
            return;
        }
        lookAround.ProcessLook(movementOfPlayer.LookAround.ReadValue<Vector2>());
        headRotate.ProcessLook(movementOfPlayer.LookAround.ReadValue<Vector2>());

    }

    private IEnumerator Hold()
    {
        yield return new WaitForSeconds(1);
        isReady = true;
    }

    // Checks which controller is being used
    private void WhichController(InputAction.CallbackContext context)
    {
        var device = context.control.device;

        switch (device)
        {
            case Gamepad:
                lookAround.ControlSpeed("Gamepad");
                headRotate.ControlSpeed("Gamepad");
                break;
            case Mouse:
                lookAround.ControlSpeed("Mouse");
                headRotate.ControlSpeed("Mouse");
                break;
        }
    }

}
