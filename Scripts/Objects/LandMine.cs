using UnityEngine;
using DG.Tweening;
using System.Collections;

public class LandMine : MonoBehaviour
{
    public GameObject blastPrefab;
    public Material mat;
    public CapsuleCollider capsuleCollider;
    private Color targetColor = Color.black;
    private Tween lightAnimtion;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        lightAnimtion = mat.DOColor(targetColor, "_EmissionColor", 3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        StartCoroutine(_LoadCollider());    
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            lightAnimtion.Kill();
            Instantiate(blastPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    private IEnumerator _LoadCollider()
    {
        yield return new WaitForSeconds(4);
        capsuleCollider.enabled = true;
    }
}
