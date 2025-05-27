using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class EnemyScript : MonoBehaviour
{
    public int requestTime;
    public bool isShooting;
    public bool isMoving;
    public bool isthorwingNad;
    public GameObject firePoint;
    public GameObject bomb;
    public GameObject bullet;

    private bool actionStarted = true;


    private void Start()
    {
        if(isShooting)
        {
            StartCoroutine(_StartAction(EnemyShooting, requestTime));
        }
        if (isMoving) 
        {
            EnemyMoving();
        }
        if(isthorwingNad)
        {
            StartCoroutine(_StartAction(ThorwingJump, requestTime));
        }
    }

    //Function ----------------------------------------------------------------------------------------
    private void EnemyShooting()
    {
        SpawnRay();
        SpawnBullet();
    }
    private void SpawnRay()
    {
        RaycastHit hit;
        if (Physics.Raycast(firePoint.transform.position, firePoint.transform.forward, out hit, 100f))
        {
            GameObject objectHit = transform.gameObject;
            float yCords = HitYReturn(objectHit);
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.Damage(5, yCords);
            }
        }
    }
    private void SpawnBullet()
    {
       // GameObject _bullet = Instantiate(bullet, firePoint.transform.position, firePoint.transform.rotation);
      //  Rigidbody _bulletRB = _bullet.GetComponent<Rigidbody>();
       // Vector3 shootingDirection = transform.forward * 80f;
     //   _bulletRB.AddForce(shootingDirection, ForceMode.Impulse);
    }
    private float HitYReturn(GameObject ob)
    {
        Vector3 objFwd = ob.transform.forward;
        float angle = Vector3.Angle(objFwd, Vector3.forward);
        float sign = Mathf.Sign(Vector3.Cross(objFwd, Vector3.forward).y);
        return angle * sign;
    }
    // Enmey Move ---------------------------------------------------------------------------------------------
    private void EnemyMoving()
    {
        transform.DOMoveX(5f, 3f).SetLoops(-1, LoopType.Yoyo);
        StartCoroutine(_StartAction(EnemyShooting, requestTime));
    }

    //Throwing nad ---------------------------------------------------------------------------------------------
    private void ThorwingJump()
    {
        transform.DOJump(transform.position, 1f, 1, 1);
        GameObject grenade = Instantiate(bomb, firePoint.transform.position, firePoint.transform.rotation);
        Rigidbody _bombRB = grenade.GetComponent<Rigidbody>();
        Vector3 thrownDirection = firePoint.transform.forward * 15f + Vector3.up * .5f;
        _bombRB.AddForce(thrownDirection, ForceMode.Impulse);
    }


    //Repeat this action --------------------------------------------------------------------------------------
    private IEnumerator _StartAction(Action function, int time)
    {
        while(actionStarted)
        {
            yield return new WaitForSeconds(time);
            function();
        }
    }
}
