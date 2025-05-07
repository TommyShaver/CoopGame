using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using UnityEditor.ShaderGraph.Internal;

public class PlayerBlink : MonoBehaviour
{
    private float eyesOpenPos = .3f;
    private float eyesClosedPos = 0;

    private float blinkTime;

    public GameObject leftEye;
    public GameObject rightEye;
    public GameObject mouthMove;

    private void Start()
    {
        leftEye.transform.DOScaleY(eyesOpenPos, 0f);
        rightEye.transform.DOScaleY(eyesOpenPos, 0f);
    }

    private void Update()
    {
        if(blinkTime >= 0)
        {
            blinkTime -= Time.deltaTime;
        }
        else
        {
            blinkTime = UnityEngine.Random.Range(4f, 7f);
            BlinkEyes();
        }
        Test();
    }

    private void BlinkEyes()
    {
        float blinkduration = UnityEngine.Random.Range(0.1f, 0.4f);
        leftEye.transform.DOScaleY(eyesClosedPos, blinkduration);
        rightEye.transform.DOScaleY(eyesClosedPos, blinkduration).OnComplete(() => {
            leftEye.transform.DOScaleY(eyesOpenPos, blinkduration);
            rightEye.transform.DOScaleY(eyesOpenPos, blinkduration);
        });
    }

    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            mouthMove.transform.DOLocalMoveY(10f, .1f).OnComplete(() => {
                mouthMove.transform.DOLocalMoveY(7.320001f, .1f);
            });
        }
    }
}
