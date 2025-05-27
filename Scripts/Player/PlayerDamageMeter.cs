using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using static UnityEngine.Rendering.DebugUI;
using UnityEditor.Experimental.GraphView;

public class PlayerDamageMeter : MonoBehaviour
{
    public Image[] damangeTaken;

    private bool[] animPlaying = new bool[4];
    private Tween animationTween;

    private void Start()
    {
        for (int i = 0; i < damangeTaken.Length; i++)
        {
            damangeTaken[i].DOFade(0, 0f);
        }
    }
    public void UI_DamangeMeter(float yCordsEnemy, float yPlayer)
    {
        if (yCordsEnemy == 500)
        {
            for (int i = 0; i < damangeTaken.Length; i++)
            {
                damangeTaken[i].DOFade(.75f, .1f);
                damangeTaken[i].DOFade(0, .5f).SetDelay(.5f);
            }
            return;
        }
        float angle = Mathf.DeltaAngle(yPlayer, yCordsEnemy);
        CalcDirction(angle);
    } 

    private void CalcDirction(float angle)
    {
        if (angle > -45 && angle < 45)
            DamageAnimation(3);
        else if (angle >= 45 && angle < 135)
            DamageAnimation(2);
        else if (angle <= -45 && angle > -135)
            DamageAnimation(1);
        else
            DamageAnimation(0);

    }

    private void DamageAnimation(int whichAnim)
    {
        //Check to see if its fading.
        if (animPlaying[whichAnim])
        {
            animationTween.Kill();
        }

        //Show Anim
        damangeTaken[whichAnim].DOFade(1, .1f).SetEase(Ease.InOutSine);
        animPlaying[whichAnim] = true;

        //Start fade down.
        animationTween = damangeTaken[whichAnim].DOFade(0, .5f).SetDelay(.5f).OnComplete(() => {
            animPlaying[whichAnim] = false;
        });
    }
   
}
