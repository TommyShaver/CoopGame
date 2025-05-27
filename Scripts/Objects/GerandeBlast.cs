using System.Collections;
using UnityEngine;

public class GerandeBlast : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(CleanUp()); 
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.Damage(50, 500f);
        }
    }
    
    private IEnumerator CleanUp()
    {
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }
}
