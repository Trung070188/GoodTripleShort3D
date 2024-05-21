using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using static BuyBoostView;
using UnityEngine.Advertisements;
using static AdsControl;

public class LuckyWheelView : BaseView
{
    public RectTransform rootItemTrans;

    public GameObject resultObject;

    public Text rewardValueTxt;

    public Image rewardIcon;

    public Sprite coinSpr;

    public Sprite hintSpr;

    public Sprite shuffleSpr;

    public Sprite freezeSpr;

    int randomRound;

    private List<int> rewardIndexList = new List<int>();

    public GameObject[] selectObjList;

    public GameObject freeBtn;

    public GameObject adsBtn;

    public GameObject closeBtn;

    public GameObject closeTextBtn;

    private int spinCount;

    private void StartSpin()
    {
        AudioManager.instance.btnSound.Play();
        freeBtn.SetActive(false);
        adsBtn.SetActive(false);
        closeBtn.SetActive(false);
        closeTextBtn.SetActive(false);
        spinCount--;

        randomRound = rewardIndexList[0];
        rewardIndexList.RemoveAt(0);

        rootItemTrans.DORotate(new Vector3(0f, 0f, 6 * 360 + 60 * randomRound), 5f, RotateMode.FastBeyond360).SetEase(Ease.InOutExpo).OnComplete(() =>
        {
            ShowReward();
        });


    }

    public override void Start()
    {

    }

    public override void Update()
    {

    }

    public override void InitView()
    {
        //AdsControl.Instance.HideBannerAd();
        randomRound = 0;
        spinCount = 6;
        resultObject.SetActive(false);
        rewardIndexList.Clear();

        freeBtn.SetActive(true);
        adsBtn.SetActive(false);
        closeBtn.SetActive(false);
        closeTextBtn.SetActive(true);

        List<int> tempIndexList = new List<int>();

        for (int i = 0; i < 6; i++)
        {
            tempIndexList.Add(i);
            selectObjList[i].SetActive(false);
        }


        for (int i = 0; i < 6; i++)
        {
            int randomIndex = Random.Range(0, tempIndexList.Count);
            rewardIndexList.Add(tempIndexList[randomIndex]);
            tempIndexList.RemoveAt(randomIndex);
        }
    }

    private void ShowReward()
    {
        AudioManager.instance.openGiftSound.Play();
        resultObject.SetActive(true);
        selectObjList[randomRound].SetActive(true);

        switch (randomRound)
        {
            case 0:
                rewardIcon.sprite = hintSpr;
                rewardValueTxt.text = "+1";
                GameManager.Instance.AddHint(1);
                break;

            case 1:
                rewardIcon.sprite = coinSpr;
                rewardValueTxt.text = "+20";
                GameManager.Instance.AddCoin(20);
                break;

            case 2:
                rewardIcon.sprite = shuffleSpr;
                rewardValueTxt.text = "+1";
                GameManager.Instance.AddShuffle(1);
                break;

            case 3:
                rewardIcon.sprite = coinSpr;
                rewardValueTxt.text = "+10";
                GameManager.Instance.AddCoin(10);
                break;

            case 4:
                rewardIcon.sprite = freezeSpr;
                rewardValueTxt.text = "+1";
                GameManager.Instance.AddFreeze(1);
                break;

            case 5:
                rewardIcon.sprite = coinSpr;
                rewardValueTxt.text = "+5";
                GameManager.Instance.AddCoin(5);
                break;
        }

        if (spinCount > 0)
        {
            freeBtn.SetActive(false);
            adsBtn.SetActive(true);
            closeBtn.SetActive(false);
            closeTextBtn.SetActive(true);
        }

        else
        {
            freeBtn.SetActive(false);
            adsBtn.SetActive(false);
            closeBtn.SetActive(true);
            closeTextBtn.SetActive(false);
        }
    }

    public void Close()
    {
        AudioManager.instance.btnSound.Play();
        HideView();
    }

    public void FreeSpin()
    {
        AudioManager.instance.btnSound.Play();
        StartSpin();

    }

    public void WatchAdsSpin()
    {
        AudioManager.instance.btnSound.Play();
        if (AdsControl.Instance.currentAdsType == ADS_TYPE.ADMOB)
        {
            if (AdsControl.Instance.rewardedAd != null)
            {
                if (AdsControl.Instance.rewardedAd.CanShowAd())
                {
                    AdsControl.Instance.ShowRewardAd(EarnFreeSpin);
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

                AdsControl.Instance.ShowRewardAd(EarnFreeSpin);

            else
                ShowRWUnityAds();
        }   
    }

    public void EarnFreeSpin(Reward reward)
    {
        StartSpin();
    }

    public void ShowRWUnityAds()
    {
        AdsControl.Instance.PlayUnityVideoAd((string ID, UnityAdsShowCompletionState callBackState) =>
        {

            if (ID.Equals(AdsControl.Instance.adUnityRWUnitId) && callBackState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                StartSpin();
            }

            if (ID.Equals(AdsControl.Instance.adUnityRWUnitId) && callBackState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                AdsControl.Instance.LoadUnityAd();
            }

        });
    }
}
