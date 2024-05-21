using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;


public class RewardRankView :BaseView
{
    public Button getCoinBtn;

    public Button getCoinX2Btn;

    public Text coinTxt;

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
        base.ShowView();
        AudioManager.instance.openGiftSound.Play();
        GameManager.Instance.currentState = GameManager.GAME_STATE.IN_GAME_POPUP;
        coinTxt.text = "+" + GameManager.Instance.gamePlaySetting.rankRewardBonus;
    }

    public override void HideView()
    {
        AudioManager.instance.btnSound.Play();
        base.HideView();
        GameManager.Instance.currentState = GameManager.GAME_STATE.IN_GAME;
    }

    public void GetCoin()
    {
        AudioManager.instance.btnSound.Play();
        getCoinBtn.interactable = false;
        getCoinX2Btn.interactable = false;
        GameManager.Instance.uiManager.coinView.ShowView();
        GameManager.Instance.uiManager.coinView.UpdateCoinPanel();
        GameManager.Instance.AddCoin(GameManager.Instance.gamePlaySetting.rankRewardBonus);
        GameManager.Instance.uiManager.coinView.UpdateCoinPanelVfx();
        AudioManager.instance.moreCoinSound.Play();
        StartCoroutine(GetCoinIE());
    }

    public void GetCoinX2()
    {
        AudioManager.instance.btnSound.Play();

        if (AdsControl.Instance.currentAdsType == AdsControl.ADS_TYPE.ADMOB)
        {
            if (AdsControl.Instance.rewardedAd != null)
            {
                if (AdsControl.Instance.rewardedAd.CanShowAd())
                {
                    AdsControl.Instance.ShowRewardAd(EarnRW);
                }
            }
        }
        else if (AdsControl.Instance.currentAdsType == AdsControl.ADS_TYPE.UNITY)
        {
            ShowRWUnityAds();
        }
        else if (AdsControl.Instance.currentAdsType == AdsControl.ADS_TYPE.MEDIATION)
        {
            if (AdsControl.Instance.rewardedAd.CanShowAd())

                AdsControl.Instance.ShowRewardAd(EarnRW);

            else
                ShowRWUnityAds();
        }
    }

    public void GetCoinX2CB()
    {
        GameManager.Instance.uiManager.coinView.ShowView();
        GameManager.Instance.uiManager.coinView.UpdateCoinPanel();
        GameManager.Instance.AddCoin(GameManager.Instance.gamePlaySetting.rankRewardBonus * 2);
        GameManager.Instance.uiManager.coinView.UpdateCoinPanelVfx();
        StartCoroutine(GetCoinIE());
    }

    IEnumerator GetCoinIE()
    {
        yield return new WaitForSeconds(1.5f);
        getCoinBtn.interactable = true;
        getCoinX2Btn.interactable = true;
        HideView();
        GameManager.Instance.uiManager.coinView.HideView();
    }

    public void EarnRW(Reward reward)
    {
        GetCoinX2CB();
    }

    public void ShowRWUnityAds()
    {
        AdsControl.Instance.PlayUnityVideoAd((string ID, UnityAdsShowCompletionState callBackState) =>
        {

            if (ID.Equals(AdsControl.Instance.adUnityRWUnitId) && callBackState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                GetCoinX2CB();
            }

            if (ID.Equals(AdsControl.Instance.adUnityRWUnitId) && callBackState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                AdsControl.Instance.LoadUnityAd();
            }

        });
    }
}
