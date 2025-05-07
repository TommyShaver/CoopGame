using System.Collections;
using UnityEngine;


public class GerandeManager : MonoBehaviour
{
    public SphereCollider blastCollider;

    private float blastRange = 20;
    private bool  canHurt;

    private void Start()
    {
        Debug.Log("we are alive");
        StartCoroutine(GerandeTimer(RandomTimer()));
        blastCollider.enabled = false;  
        blastCollider.radius = 0;
    }
    
    //Logic ---------------------------------------------
    private int RandomTimer()
    {
        int randoTime = UnityEngine.Random.Range(4, 5);
        return randoTime;
    }
    

    private void Explode()
    {
        blastCollider.enabled = true;
        StartCoroutine(GerandeEndOfLife());
        //play explostion
        //play particles.

    }
    //IEnumerator ---------------------------------------
    private IEnumerator GerandeTimer(float howLong)
    {
        yield return new WaitForSeconds(howLong);
        Explode();
    }

    private IEnumerator GerandeEndOfLife()
    {
        for (int i = 0; i <= blastRange; i++)
        {
            yield return new WaitForSeconds(.01f);
            blastCollider.radius = i;
            canHurt = true;
        }
        Destroy(gameObject);
    }
    //Interface ----------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null && canHurt)
        {
            damageable.Damage(50);
        }
    }
}
