using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotor : MonoBehaviour, ISlowZone
{
    public float gravity = -9.8f;
    public float jumpHieght = 3f;
    public GameObject grenadePrefab;
    public GameObject bulletPrefab;
    public Transform throwPointBomb;
    public Transform gunPoint;
    public Transform camPos;
    public GameObject[] helperItems;

    private CharacterController characterController;
    private PlayerManager playerManager;
    private Vector3 playerVelocity;
    private bool isGround;
    private bool canDash;
    private bool canUseLeftHand;
    private bool canUseRightHand;
    private bool canJump;
    private float speed = 7f;
    private float SDZ_speed = 1.0f;
    private float aimSpeed = 1.0f;
    private int whichHelperItem = 0;
    Gamepad gamepad = Gamepad.current;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerManager = GetComponent<PlayerManager>(); 
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

    //Inbound commands ----------------------------------------------------------------------------
    //Turn on and off controls due to spawn and death
    public void CanUseHands(bool canUse)
    {
        canUseLeftHand = canUse;
        canUseRightHand = canUse;
        canJump = canUse;
    }










    // Controller Logic ----------------------------------------------------------------------------
    //Movement -------------------------------------------------------------------------------------
    public void ProcesMove(Vector2 input)
    {
        Vector3 moveDir = Vector3.zero;
        moveDir.x = input.x;
        moveDir.z = input.y;
        characterController.Move(transform.TransformDirection(moveDir) * speed * SDZ_speed * aimSpeed *  Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGround && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f; 
        }
        characterController.Move(playerVelocity * Time.deltaTime);
        //Send Movement to Animation
    }

    public void Jump()
    {
        if(!canJump)
        {
            return;
        }
        if (isGround)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHieght * -3.0f * gravity);
        }
        else if (canDash)
        {
            canDash = false;
            speed += 5f;
        }
        //Send Movement to Animation
    }

    public void SlowZone(bool slow)
    {
        if (slow)
        {
            SDZ_speed = SDZ_speed * .4f;
        }
        else
        {
            SDZ_speed = 1f;
        }
        //Send Movement to Animation (Slow down Animaiton)
    }
    public void AimSpeed(bool aiming)
    {
        if (isGround)
        {
            if (aiming)
            {
                aimSpeed = aimSpeed * .6f;
            }
            else
            {
                aimSpeed = 1f;
            }
        }
        else
        {
            aimSpeed = 1f;
        }
    }









    //HAnds ----------------------------------------------------------------------------------------
    //Player shot logic omg this was wild ==========================================================
    public void ShootWeapon()
    {
        if (!canUseRightHand)
        {
            return;
        }
        canUseRightHand = false;
        ControllerMotor();
        RayHitSpawn();
        SpawnBullet();
        StartCoroutine(GunHold(.2f));
    }
    private void RayHitSpawn()
    {
        RaycastHit hit;
        if (Physics.Raycast(camPos.transform.position, camPos.transform.forward, out hit, 100f))
        {
            GameObject objectHit = hit.transform.gameObject;
            float yCords = HitYReturn(objectHit);
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.Damage(5, hit.transform.forward.y);
            }
        }
    }
    private float HitYReturn(GameObject ob)
    {
        Vector3 objFwd = ob.transform.forward;
        float angle = Vector3.Angle(objFwd, Vector3.forward);
        float sign = Mathf.Sign(Vector3.Cross(objFwd, Vector3.forward).y);
        return angle * sign;
    }
    private void ControllerMotor()
    {
        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(.5f, 1.0f);
            Invoke(nameof(StopVibration), .1f);
        }
    }
    private void SpawnBullet()
    {
        //GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
        // Rigidbody _bulletRB = bullet.GetComponent<Rigidbody>();
        //Vector3 shootingDirection = camPos.forward * 80f;
        //_bulletRB.AddForce(shootingDirection, ForceMode.Impulse);
        //Send Movement to Animation and lit particle effect on gun.
    }
    //==============================================================================================
    








    //Left hand _____________________________________________________________________________________
    public void ThorwGrenade()
    {
        if (!canUseLeftHand)
        {
            return;
        }

        if(playerManager.DoIHaveItem(3))
        {
            canUseLeftHand = false;
            GameObject grenade = Instantiate(grenadePrefab, throwPointBomb.position, throwPointBomb.rotation);
            Rigidbody _bombRB = grenade.GetComponent<Rigidbody>();
            Vector3 thrownDirection = camPos.forward * 20f + Vector3.up * 1.5f;
            _bombRB.AddForce(thrownDirection, ForceMode.Impulse);
            StartCoroutine(Hold(2f, canUseLeftHand));
            //Send Movement to Animation and lit particle effect on gun.
        }
        //Send Movement to Animation and lit particle effect on gun.
    }

    public void Punch()
    {
        if (!canUseLeftHand)
        {
            return;
        }
        canUseLeftHand = false;
        //Send Movement to Animation and lit particle effect on gun.
    }



    public void WhichHelperItem(int i)
    {
        whichHelperItem = i;
    }

    public void ThorwHelperItem()
    {
        if (!canUseLeftHand)
        {
            return;
        }
        if(playerManager.DoIHaveItem(whichHelperItem))
        {
            canUseLeftHand = false;
            GameObject HelperItemsToss = Instantiate(helperItems[whichHelperItem], throwPointBomb.position, throwPointBomb.rotation);
            Rigidbody _bombRB = HelperItemsToss.GetComponent<Rigidbody>();
            Vector3 thrownDirection = camPos.forward * 10f + Vector3.up * 1.5f;
            _bombRB.AddForce(thrownDirection, ForceMode.Impulse);
            StartCoroutine(Hold(3f, canUseLeftHand));
            //Send Movement to Animation and lit particle effect on gun.
        }
    }

    //Help Function -------------------------------------------------------------------
    private IEnumerator Hold(float timer, bool whichBool)
    {
        yield return new WaitForSeconds(timer);
        canUseLeftHand = true;
    }
    private IEnumerator GunHold(float timer)
    {
        yield return new WaitForSeconds(timer);
        canUseRightHand = true;
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
