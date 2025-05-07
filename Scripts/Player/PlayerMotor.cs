using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotor : MonoBehaviour
{
    public float gravity = -9.8f;
    public float jumpHieght = 3f;
    public GameObject grenadePrefab;
    public Transform firePoint;
    public Transform camPos;

    private CharacterController characterController;
    private Vector3 playerVelocity;
    private bool isGround;
    private bool canDash;
    private bool canTossBomb;
    private float speed = 7f;

    Gamepad gamepad = Gamepad.current;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        canTossBomb = true;
    }

    // Update is called once per frame
    void Update()
    {
        isGround = characterController.isGrounded;
        if (isGround)
        {
            RestoreStat();
        }

    }

    // Controller Logic ----------------------------------------------------------------------------
    //Movement -------------------------------------------------------------------------------------
    public void ProcesMove(Vector2 input)
    {
        Vector3 moveDir = Vector3.zero;
        moveDir.x = input.x;
        moveDir.z = input.y;
        characterController.Move(transform.TransformDirection(moveDir) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGround && playerVelocity.y < 0) { playerVelocity.y = -2f; }
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGround)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHieght * -3.0f * gravity);
        }
    }
    public void Dash()
    {
        if (!isGround && canDash == true)
        {
            canDash = false;
            speed = speed + 5;
        }
    }

    //HAnds ----------------------------------------------------------------------------------------
    public void ShootWeapon()
    {
        Debug.Log("Gun Shoot");
        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(.5f, 1.0f);
            Invoke(nameof(StopVibration), .1f);
        }
    }

    public void ThorwGrenade()
    {
        if (canTossBomb)
        {
            canTossBomb = false;
            GameObject grenade = Instantiate(grenadePrefab, firePoint.position, firePoint.rotation);
            Rigidbody _bombRB = grenade.GetComponent<Rigidbody>();
            Vector3 thrownDirection = camPos.forward * 15f + Vector3.up * 1.5f;
            _bombRB.AddForce(thrownDirection, ForceMode.Impulse);
            StartCoroutine(Hold(2f, canTossBomb));
        }
    }

    public void Punch()
    {

    }

    public void ThorwHelperItem()
    {
        //force feild
        //slow down 
        //
    }

    //Help Function -------------------------------------------------------------------
    private IEnumerator Hold(float timer, bool whichBool)
    {
        yield return new WaitForSeconds(timer);
;       canTossBomb = true;
    }
    private void RestoreStat()
    {
        speed = 7f;
        canDash = true;
    }

    private void StopVibration()
    {
        Gamepad.current?.SetMotorSpeeds(0f, 0f);
    }
}
