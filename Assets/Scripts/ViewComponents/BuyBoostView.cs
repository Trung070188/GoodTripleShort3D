using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using static AdsControl;

public class BuyBoostView : BaseView
{
    public enum BoostType
    {
        HINT,
        SHUFFLE,
        FREEZE
    }

    public BoostType currentType;

    public Image titleIcon;

    public Image freePack1Icon;

    public Image freePack2Icon;

    public List<BuyPackageData> IAPPackageData = new List<BuyPackageData>();

    public List<BuyPackageView> IAPPackageView = new List<BuyPackageView>();

    public Sprite[] boostIconList;

    public override void InitView()
    {

    }

    public void InitView(BoostType type)
    {
        currentType = type;
    }

    public override void ShowView()
    {
        base.ShowView();

        Sprite itemSpr = null;

        if (currentType == BoostType.HINT)
            itemSpr = boostIconList[0];
        else
        if (currentType == BoostType.SHUFFLE)
            itemSpr = boostIconList[1];
        else
        if (currentType == BoostType.FREEZE)
            itemSpr = boostIconList[2];

        titleIcon.sprite = itemSpr;
        freePack1Icon.sprite = itemSpr;
        freePack2Icon.sprite = itemSpr;

        for (int i = 0; i < IAPPackageData.Count; i++)
        {
            IAPPackageView[i].itemIcon.sprite = itemSpr;
            IAPPackageView[i].priceTxt.text = "$" + IAPPackageData[i].priceValue;
            IAPPackageView[i].totalTxt.text = "x" + IAPPackageData[i].totalValue;
        }
    }

    public override void Start()
    {

    }

    public override void Update()
    {

    }

    public void BuyByCoin()
    {
        AudioManager.instance.btnSound.Play();
        if (GameManager.Instance.currentCoin >= 400)
        {
            GameManager.Instance.AddCoin(-400);
            if (currentType == BoostType.HINT)
                GameManager.Instance.AddHint(1);
            else
        if (currentType == BoostType.SHUFFLE)
                GameManager.Instance.AddShuffle(1);
            else
        if (currentType == BoostType.FREEZE)
                GameManager.Instance.AddFreeze(1);
            HideView();
        }
        else
        {
            HideView();
            GameManager.Instance.uiManager.storeView.ShowView();
        }

    }

    public void WatchAds()
    {
        AudioManager.instance.btnSound.Play();

        if (AdsControl.Instance.currentAdsType == ADS_TYPE.ADMOB)
        {
            if (AdsControl.Instance.rewardedAd != null)
            {
                if (AdsControl.Instance.rewardedAd.CanShowAd())
                {
                    AdsControl.Instance.ShowRewardAd(EarnFreeBoost);
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

                AdsControl.Instance.ShowRewardAd(EarnFreeBoost);

            else
                ShowRWUnityAds();
        }



        HideView();
    }

    public void MoreCoins()
    {
        AudioManager.instance.btnSound.Play();
        GameManager.Instance.uiManager.storeLayer.BuyIAPPackage(Config.IAPPackageID.coin_900);
        HideView();
    }

    public void BuyIAP(int itemIndex)
    {
        AudioManager.instance.btnSound.Play();
        switch (itemIndex)
        {
            case 0:
                GameManager.Instance.uiManager.storeLayer.BuyIAPPackage(Config.IAPPackageID.boost_5);
                break;

            case 1:
                GameManager.Instance.uiManager.storeLayer.BuyIAPPackage(Config.IAPPackageID.boost_12);
                break;

            case 2:
                GameManager.Instance.uiManager.storeLayer.BuyIAPPackage(Config.IAPPackageID.boost_25);
                break;
        }

        HideView();
    }


    public void EarnFreeBoost(Reward reward)
    {

        if (currentType == BoostType.HINT)
            GameManager.Instance.AddHint(1);
        else if (currentType == BoostType.SHUFFLE)
            GameManager.Instance.AddShuffle(1);
        else if (currentType == BoostType.FREEZE)
            GameManager.Instance.AddFreeze(1);
    }

    public void ShowRWUnityAds()
    {
        AdsControl.Instance.PlayUnityVideoAd((string ID, UnityAdsShowCompletionState callBackState) =>
        {

            if (ID.Equals(AdsControl.Instance.adUnityRWUnitId) && callBackState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                if (currentType == BoostType.HINT)
                    GameManager.Instance.AddHint(1);
                else if (currentType == BoostType.SHUFFLE)
                    GameManager.Instance.AddShuffle(1);
                else if (currentType == BoostType.FREEZE)
                    GameManager.Instance.AddFreeze(1);
            }

            if (ID.Equals(AdsControl.Instance.adUnityRWUnitId) && callBackState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                AdsControl.Instance.LoadUnityAd();
            }

        });
    }
}

[System.Serializable]
public class BuyPackageView
{
    public Image itemIcon;

    public Text totalTxt;

    public Text priceTxt;

}


[System.Serializable]
public class BuyPackageData
{
    public int totalValue;

    public float priceValue;
}
