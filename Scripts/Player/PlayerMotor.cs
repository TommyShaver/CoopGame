using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotor : MonoBehaviour
{
    public float gravity = -9.8f;
    public float jumpHieght = 3f;
    public GameObject grenadePrefab;
    public GameObject bulletPrefab;
    public Transform throwPointBomb;
    public Transform gunPoint;
    public Transform camPos;
    

    private CharacterController characterController;
    private Vector3 playerVelocity;
    private bool isGround;
    private bool canDash;
    private bool canUseLeftHand;
    private bool canUseRightHand;
    private float speed = 7f;

    Gamepad gamepad = Gamepad.current;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        canUseLeftHand = true;
        canUseRightHand = true; 
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
        if(!canUseRightHand)
        {
            return;
        }
        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(.5f, 1.0f);
            Invoke(nameof(StopVibration), .1f);
        }
        RaycastHit hit;
        if (Physics.Raycast(camPos.transform.position, camPos.transform.forward, out hit, 100f))
        {
            Debug.Log("Hit: " + hit.transform.name);
        }
        GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
        Rigidbody _bulletRB = bullet.GetComponent<Rigidbody>();
        Vector3 shootingDirection = camPos.forward * 70f + Vector3.up;
        _bulletRB.AddForce(shootingDirection, ForceMode.Impulse);
    }



    public void ThorwGrenade()
    {
        if (!canUseLeftHand)
        {
            return;
        }
        canUseLeftHand = false;
        GameObject grenade = Instantiate(grenadePrefab, throwPointBomb.position, throwPointBomb.rotation);
        Rigidbody _bombRB = grenade.GetComponent<Rigidbody>();
        Vector3 thrownDirection = camPos.forward * 10f + Vector3.up * 1.5f;
        _bombRB.AddForce(thrownDirection, ForceMode.Impulse);
        StartCoroutine(Hold(2f, canUseLeftHand));
    }

    public void Punch()
    {
        if(!canUseLeftHand)
        {
            return;
        }
    }

    public void ThorwHelperItem()
    {
        if (!canUseLeftHand)
        {
            return;
        }
        //force feild
        //slow down 
        //Land mine
    }

    //Help Function -------------------------------------------------------------------

    public void CanControl(bool canControl)
    {
        canUseLeftHand = canControl;
        canUseRightHand = canControl;
    }
    private IEnumerator Hold(float timer, bool whichBool)
    {
        yield return new WaitForSeconds(timer);
;       canUseLeftHand = true;
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
