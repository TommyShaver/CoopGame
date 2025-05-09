using System.Collections;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(TimerTillDeath());       
    }
    private void OnTriggerEnter(Collider other)
    {
        StopCoroutine(TimerTillDeath());
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.Damage(5);
            Debug.Log("I hit something");
            Destroy(gameObject);
        }
    }

    private IEnumerator TimerTillDeath()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }
}
