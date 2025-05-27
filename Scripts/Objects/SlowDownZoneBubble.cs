using DG.Tweening;
using UnityEngine;
using System.Collections;

public class SlowDownZoneBubble : MonoBehaviour
{
    public Light SDZ_spot;
    private void Start()
    {
        ScaleUpBubble();
        StartCoroutine(TimerForSlowDownZone());
        SDZ_spot.DOIntensity(100, .5f);
    }

    //I have been toucnhed --------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        ISlowZone slowZone = other.GetComponent<ISlowZone>();
        if (slowZone != null)
        {
            slowZone.SlowZone(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ISlowZone slowZone = other.GetComponent<ISlowZone>();
        if (slowZone != null)
        {
            slowZone.SlowZone(false);
        }
    }

    //Logic ---------------------------------------------------------
    private void ScaleUpBubble()
    {
        transform.DOScale(7f, .5f).SetEase(Ease.InOutBack);
    }

    private IEnumerator TimerForSlowDownZone()
    {
        yield return new WaitForSeconds(15f);
        transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
