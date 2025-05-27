using System.Collections;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(TimerTillDeath());       
    }

    private IEnumerator TimerTillDeath()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }
}
