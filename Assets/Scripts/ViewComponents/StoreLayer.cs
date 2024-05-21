using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using UnityEngine.Advertisements;
using static AdsControl;

public class StoreLayer : SliderNode
{
    //gift item
    public List<GiftPackageItem> giftPackageList;

    public GiftPackageView giftPackageViewPrefab;

    public RectTransform giftPackageViewRoot;

    //coin item
    public List<CoinPackageItem> coinPackageList;

    public CoinPackageView coinPackageViewPrefab;

    public RectTransform coinPackageViewRoot;

    //life item
    public List<LifePackageItem> lifePackageList;

    public LifePackageView lifePackageViewPrefab;

    public RectTransform lifePackageViewRoot;

    public Text freeAdsValueTxt;

    public Text freeAdsTimerTxt;

    public override void InitView()
    {
       
    }

    private void LoadItem()
    {
        for (int i = 0; i < giftPackageList.Count; i++)
        {
            GiftPackageView itemView = Instantiate(giftPackageViewPrefab);
            itemView.transform.parent = giftPackageViewRoot;
            itemView.transform.localScale = Vector3.one;
            itemView.ShowView(giftPackageList[i]);
        }

        for (int i = 0; i < coinPackageList.Count; i++)
        {
            CoinPackageView itemView = Instantiate(coinPackageViewPrefab);
            itemView.transform.parent = coinPackageViewRoot;
            itemView.transform.localScale = Vector3.one;
            itemView.ShowView(coinPackageList[i]);
        }

        for (int i = 0; i < lifePackageList.Count; i++)
        {
            LifePackageView itemView = Instantiate(lifePackageViewPrefab);
            itemView.transform.parent = lifePackageViewRoot;
            itemView.transform.localScale = Vector3.one;
            itemView.ShowView(lifePackageList[i]);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadItem();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FreeCoin()
    {
        AudioManager.instance.btnSound.Play();
        if (GameManager.Instance.freeAdsTimer.freeAdsNumber > 0)
        {
            if (AdsControl.Instance.currentAdsType == ADS_TYPE.ADMOB)
            {
                if (AdsControl.Instance.rewardedAd != null)
                {
                    if (AdsControl.Instance.rewardedAd.CanShowAd())
                    {
                        AdsControl.Instance.ShowRewardAd(EarnFreeCoin);
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

                    AdsControl.Instance.ShowRewardAd(EarnFreeCoin);

                else
                    ShowRWUnityAds();
            }


        }

    }

    public void ShowRWUnityAds()
    {
        AdsControl.Instance.PlayUnityVideoAd((string ID, UnityAdsShowCompletionState callBackState) =>
        {

            if (ID.Equals(AdsControl.Instance.adUnityRWUnitId) && callBackState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                GameManager.Instance.freeAdsTimer.ConsumeLife();
                GameManager.Instance.AddCoin(50);
            }

            if (ID.Equals(AdsControl.Instance.adUnityRWUnitId) && callBackState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                AdsControl.Instance.LoadUnityAd();
            }

        });
    }

    public void EarnFreeCoin(Reward reward)
    {
        GameManager.Instance.freeAdsTimer.ConsumeLife();
        GameManager.Instance.AddCoin(50);
    }

    public void BuyIAPPackage(Config.IAPPackageID packageID)
    {
        AudioManager.instance.btnSound.Play();

        IAPManager.instance.BuyConsumable(packageID, (string iapID, IAPManager.IAP_CALLBACK_STATE state) =>
        {
            if (state == IAPManager.IAP_CALLBACK_STATE.SUCCESS)
            {

                Debug.Log("SUCCESSSUCCESS " + iapID);

                if (iapID.Equals(Config.IAPPackageID.remove_ads.ToString()))
                {
                    AdsControl.Instance.RemoveAds();
                }
                else
                {
                    BuySuccesss(packageID);
                }
            }
            else
            {
                Debug.Log("Buy Fail!");
               
            }
        });


      
    }

    public void BuySuccesss(Config.IAPPackageID packageID)
    {
        switch (packageID)
        {
            case Config.IAPPackageID.big_bundle:
                GameManager.Instance.AddCoin(3200);
                GameManager.Instance.AddHint(1);
                GameManager.Instance.AddShuffle(1);
                GameManager.Instance.AddFreeze(1);
                break;

            case Config.IAPPackageID.super_bundle:
                GameManager.Instance.AddCoin(5400);
                GameManager.Instance.AddHint(2);
                GameManager.Instance.AddShuffle(2);
                GameManager.Instance.AddFreeze(2);
                break;

            case Config.IAPPackageID.huge_bundle:
                GameManager.Instance.AddCoin(10500);
                GameManager.Instance.AddHint(4);
                GameManager.Instance.AddShuffle(4);
                GameManager.Instance.AddFreeze(4);
                break;

            case Config.IAPPackageID.mega_bundle:
                GameManager.Instance.AddCoin(23000);
                GameManager.Instance.AddHint(6);
                GameManager.Instance.AddShuffle(6);
                GameManager.Instance.AddFreeze(6);
                break;

            case Config.IAPPackageID.brilliant_bundle:
                GameManager.Instance.AddCoin(45000);
                GameManager.Instance.AddHint(13);
                GameManager.Instance.AddShuffle(13);
                GameManager.Instance.AddFreeze(13);
                break;

            case Config.IAPPackageID.coin_900:
                GameManager.Instance.AddCoin(900);
                break;

            case Config.IAPPackageID.coin_2400:
                GameManager.Instance.AddCoin(2400);
                break;

            case Config.IAPPackageID.coin_5400:
                GameManager.Instance.AddCoin(5400);
                break;

            case Config.IAPPackageID.coin_11000:
                GameManager.Instance.AddCoin(11000);
                break;

            case Config.IAPPackageID.coin_24000:
                GameManager.Instance.AddCoin(24000);
                break;

            case Config.IAPPackageID.coin_42000:
                GameManager.Instance.AddCoin(42000);
                break;

            case Config.IAPPackageID.heart_infinitive_30m:
                GameManager.Instance.livesManager.GiveInifinite(30);
                break;

            case Config.IAPPackageID.heart_infinitive_120m:
                GameManager.Instance.livesManager.GiveInifinite(120);
                break;

            case Config.IAPPackageID.boost_5:
                if (GameManager.Instance.uiManager.buyBoostView.currentType == BuyBoostView.BoostType.HINT)
                    GameManager.Instance.AddHint(5);
                else if (GameManager.Instance.uiManager.buyBoostView.currentType == BuyBoostView.BoostType.SHUFFLE)
                    GameManager.Instance.AddShuffle(5);
                else if (GameManager.Instance.uiManager.buyBoostView.currentType == BuyBoostView.BoostType.FREEZE)
                    GameManager.Instance.AddFreeze(5);
                break;

            case Config.IAPPackageID.boost_12:
                if (GameManager.Instance.uiManager.buyBoostView.currentType == BuyBoostView.BoostType.HINT)
                    GameManager.Instance.AddHint(12);
                else if (GameManager.Instance.uiManager.buyBoostView.currentType == BuyBoostView.BoostType.SHUFFLE)
                    GameManager.Instance.AddShuffle(12);
                else if (GameManager.Instance.uiManager.buyBoostView.currentType == BuyBoostView.BoostType.FREEZE)
                    GameManager.Instance.AddFreeze(12);
                break;

            case Config.IAPPackageID.boost_25:
                if (GameManager.Instance.uiManager.buyBoostView.currentType == BuyBoostView.BoostType.HINT)
                    GameManager.Instance.AddHint(25);
                else if (GameManager.Instance.uiManager.buyBoostView.currentType == BuyBoostView.BoostType.SHUFFLE)
                    GameManager.Instance.AddShuffle(25);
                else if (GameManager.Instance.uiManager.buyBoostView.currentType == BuyBoostView.BoostType.FREEZE)
                    GameManager.Instance.AddFreeze(25);
                break;
        }
    }

    public void RemoveAds()
    {
        AudioManager.instance.btnSound.Play();
        BuyIAPPackage(Config.IAPPackageID.remove_ads);
        Debug.Log("Remove Ads");
    }

    public void Restore()
    {
        AudioManager.instance.btnSound.Play();
        IAPManager.instance.RestorePurchases();
        Debug.Log("Restore");
    }

    public void UpdateFreeAdsTxt()
    {
        freeAdsValueTxt.text = GameManager.Instance.freeAdsTimer.LivesText;
        GameManager.Instance.uiManager.storeView.UpdateFreeAdsTxt();
    }

    public void UpdateFreeAdsTimerTxt()
    {
        freeAdsTimerTxt.text = GameManager.Instance.freeAdsTimer.RemainingTimeString;
        GameManager.Instance.uiManager.storeView.UpdateFreeAdsTimerTxt();
    }

  
}

[System.Serializable]
public class GiftPackageItem
{
    public string packageName;

    public string packagePrice;

    public int packID;

    public Config.IAPPackageID iapPackage;

    public List<int> itemValueList;
}

[System.Serializable]
public class CoinPackageItem
{
    public string packageValue;

    public string packagePrice;

    public int packID;

    public Config.IAPPackageID iapPackage;

}

[System.Serializable]
public class LifePackageItem
{
    public string packageValue;

    public string packagePrice;

    public int packID;

    public Config.IAPPackageID iapPackage;

}