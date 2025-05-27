using UnityEngine;
using DG.Tweening;

public class SlowDownZoneManager : MonoBehaviour
{
    public GameObject SlowDownZone;

    private Tween speeeeeeen;

    private void Start()
    {
        float rotateSpeed = UnityEngine.Random.Range(1, 5);
        speeeeeeen = transform.DORotate(new Vector3(0, _RandoDirection(), 0), rotateSpeed).SetLoops(-1, LoopType.Restart);
    }
    private void OnCollisionEnter(Collision collision)
    {
        speeeeeeen.Kill();
        Instantiate(SlowDownZone, transform.position, transform.rotation);
        Destroy(gameObject);
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
