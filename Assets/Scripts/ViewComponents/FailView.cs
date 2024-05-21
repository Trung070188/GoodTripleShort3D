using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailView : BaseView
{
    public override void InitView()
    {

    }

    public override void Start()
    {

    }

    public override void Update()
    {

    }

    public override void HideView()
    {
        base.HideView();
    }

    public void Close()
    {
        AudioManager.instance.btnSound.Play();
        HideView();
        GameManager.Instance.livesManager.ConsumeLife();
        GameManager.Instance.BackToHome();
    }

    public void ResumeByAds()
    {
        AudioManager.instance.btnSound.Play();
        GameManager.Instance.uiManager.gameView.ResumeTimer();
        HideView();
    }

    public void ResumeByCoin()
    {
        AudioManager.instance.btnSound.Play();
        if (GameManager.Instance.currentCoin >= 400)
        {
            GameManager.Instance.uiManager.gameView.ResumeTimer();
            HideView();
            GameManager.Instance.AddCoin(-400);
        }
        else
        {
            GameManager.Instance.uiManager.storeView.ShowView();
        }

    }
}
