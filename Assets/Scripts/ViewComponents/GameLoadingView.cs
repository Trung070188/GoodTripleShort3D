using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameLoadingView : BaseView
{
    public RectTransform bgLeft;

    public RectTransform bgRight;

    public RectTransform titleLeft;

    public RectTransform titleRight;

    public override void InitView()
    {

    }

    public override void Start()
    {

    }

    public override void Update()
    {

    }

    public override void ShowView()
    {
         if (GameManager.Instance.currentState == GameManager.GAME_STATE.IN_HOME)
            AdsControl.Instance.HideBannerAd();

        canvasGroup.alpha = 1.0f;

        bgLeft.localPosition = new Vector3(-240, 0, 0);
        bgRight.localPosition = new Vector3(240, 0, 0);
        titleLeft.localPosition = new Vector3(0, 0, 0);
        titleRight.localPosition = new Vector3(0, 0, 0);
        AudioManager.instance.loadingGameSound.Play();

        bgLeft.DOLocalMoveX(-400, 1f).SetRelative(true).SetDelay(0.5f).SetEase(Ease.Linear);
        bgRight.DOLocalMoveX(400, 1f).SetRelative(true).SetDelay(0.5f).SetEase(Ease.Linear);
        titleLeft.DOLocalMoveX(-400, 1f).SetRelative(true).SetDelay(0.5f).SetEase(Ease.Linear);
        titleRight.DOLocalMoveX(400, 1f).SetRelative(true).SetDelay(0.5f).SetEase(Ease.Linear).OnComplete
            (
            () =>
            {
                HideView();
                bgLeft.localPosition = new Vector3(-240, 0, 0);
                bgRight.localPosition = new Vector3(240, 0, 0);
                titleLeft.localPosition = new Vector3(0, 0, 0);
                titleRight.localPosition = new Vector3(0, 0, 0);

                if (GameManager.Instance.currentState == GameManager.GAME_STATE.IN_GAME)
                    AdsControl.Instance.ShowBannerAd();
               
            }
            );
    }

    public override void HideView()
    {
        canvasGroup.alpha = 0.0f;
    }
}
