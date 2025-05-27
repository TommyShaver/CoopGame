using DG.Tweening;
using System.Collections;
using UnityEngine;


public class GerandeManager : MonoBehaviour
{
    public GameObject blastPart;

    private Tween speeeen;

    private void Start()
    {
        float rotateSpeed = UnityEngine.Random.Range(1, 3);
        StartCoroutine(GerandeTimer(RandomTimer()));
        speeeen = transform.DORotate( new Vector3(_RandoDirection(),0,0), rotateSpeed).SetLoops(-1, LoopType.Restart);
    }

    //Logic ---------------------------------------------
    private int RandomTimer()
    {
        int randoTime = UnityEngine.Random.Range(2, 4);
        return randoTime;
    }

    private void Explode()
    {
        speeeen.Kill();
        Instantiate(blastPart, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    //Help function ---------------------------------------
    private IEnumerator GerandeTimer(float howLong)
    {
        yield return new WaitForSeconds(howLong);
        Explode();
    }

    private float _RandoDirection()
    {
        float whichDirection = 0;
        float posOrNeg = -1;
        int randoNumber = UnityEngine.Random.Range(0, 1);
        if (randoNumber == 0)
        {
            posOrNeg = 1;
        }
        whichDirection = 180 * posOrNeg;
        return whichDirection;
    }
}
