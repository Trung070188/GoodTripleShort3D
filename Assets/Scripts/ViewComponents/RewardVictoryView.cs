using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using static AdsControl;

public class RewardVictoryView : BaseView
{
    public RectTransform titleTrans;

    public RectTransform rewardItemTrans;

    public RectTransform btnGroupTrans;

    public GameObject boxAnim;

    public int secondItemIndex;

    public Image secondItemSpr;

    public Sprite[] secondItemArr;

    public override void InitView()
    {
        
    }

    public override void ShowView()
    {
        base.ShowView();
        AudioManager.instance.openGiftSound.Play();
        secondItemIndex = Random.Range(0, 3);

        if (secondItemIndex == 0)
            secondItemSpr.sprite = secondItemArr[0];
        else if (secondItemIndex == 1)
            secondItemSpr.sprite = secondItemArr[1];
        else if (secondItemIndex == 2)
            secondItemSpr.sprite = secondItemArr[2];

        titleTrans.gameObject.SetActive(false);
        rewardItemTrans.gameObject.SetActive(false);
        btnGroupTrans.gameObject.SetActive(false);
        boxAnim.SetActive(true);
        boxAnim.GetComponent<Animator>().SetBool("Open", true);
        StartCoroutine(ShowItems());
    }

    IEnumerator ShowItems()
    {
        yield return new WaitForSeconds(1.5f);
        AudioManager.instance.openGiftSound.Play();
        boxAnim.SetActive(false);
        titleTrans.gameObject.SetActive(true);
        rewardItemTrans.gameObject.SetActive(true);
        btnGroupTrans.gameObject.SetActive(true);
    }

    public override void Start()
    {
        
    }

    public override void Update()
    {
       
    }

    public void Claim()
    {
        AudioManager.instance.btnSound.Play();

        if (AdsControl.Instance.currentAdsType == ADS_TYPE.ADMOB)
        {
            if (AdsControl.Instance.rewardedAd != null)
            {
                if (AdsControl.Instance.rewardedAd.CanShowAd())
                {
                    AdsControl.Instance.ShowRewardAd(EarnRW);
                }
            }
        }
        else if (AdsControl.Instance.currentAdsType == ADS_TYPE.UNITY)
        {
            ShowRWUnityAds();
        }
        else if (AdsControl.Instance.currentAdsType == ADS_TYPE.MEDIATION)
        {
            if (AdsControl.Instance.rewardedAd.CanShowAd())

                AdsControl.Instance.ShowRewardAd(EarnRW);

            else
                ShowRWUnityAds();
        }
    }

    public void EarnRW(Reward reward)
    {
        ClaimCB();
    }

    public void ShowRWUnityAds()
    {
        AdsControl.Instance.PlayUnityVideoAd((string ID, UnityAdsShowCompletionState callBackState) =>
        {

            if (ID.Equals(AdsControl.Instance.adUnityRWUnitId) && callBackState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                ClaimCB();
            }

            if (ID.Equals(AdsControl.Instance.adUnityRWUnitId) && callBackState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                AdsControl.Instance.LoadUnityAd();
            }

        });
    }

    public void ClaimCB()
    {
        GameManager.Instance.uiManager.coinView.InitView();
        GameManager.Instance.uiManager.coinView.ShowView();
        GameManager.Instance.uiManager.coinView.UpdateCoinPanelVfx();
        GameManager.Instance.AddCoin(10);
        if (secondItemIndex == 0)
            GameManager.Instance.AddHint(1);
        else if (secondItemIndex == 1)
            GameManager.Instance.AddShuffle(1);
        else if (secondItemIndex == 2)
            GameManager.Instance.AddFreeze(1);
        StartCoroutine(ClaimIE());
    }

    IEnumerator ClaimIE()
    {
        yield return new WaitForSeconds(2.0f);
        GameManager.Instance.uiManager.coinView.HideView();
        HideView();
    }

    public void LoseIt()
    {
        AudioManager.instance.btnSound.Play();
        HideView();
    }
    
}
