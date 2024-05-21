using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using GoogleMobileAds.Api;
using static BuyBoostView;
using UnityEngine.Advertisements;
using static AdsControl;

public class WinView : BaseView
{
    public RectTransform topTrans;

    public RectTransform contentTrans;

    public RectTransform victoryBanner;

    public CanvasGroup propBg;

    public CanvasGroup adsMulti;

    public CanvasGroup btnGroup;

    public CanvasGroup nextBtn;

    public RectTransform lightTrans;

    public RectTransform arrowTrans;

    public Text claimTxt;

    public Text gotStarTxt;

    public Text rewardVictoryProgressTxt;

    public Image rewardVictoryProgressImg;

    public Text luckySpinProgressTxt;

    public Image luckySpinProgressImg;

    public int multiAdsCount;

    public Tween moveArrowTween;

    public RectTransform starVfxTarget;

    public GameObject starVfx1;

    public GameObject starVfx2;

    public GameObject starVfx3;

    private Vector3 localPos1;

    private Vector3 localPos2;

    private Vector3 localPos3;

    public Button claimButton;

    public Button nextButton;

    public override void InitView()
    {
        gotStarTxt.text = GameManager.Instance.inGameStar.ToString();
        //PlayerPrefs.SetInt("Star", GameManager.Instance.inGameStar);

        if (GameManager.Instance.currentRewardVictoryProgress < GameManager.Instance.gamePlaySetting.rewardVictoryProgressMax)
        {
            GameManager.Instance.currentRewardVictoryProgress++;
        }

        if (GameManager.Instance.currentLuckySpinProgress < GameManager.Instance.gamePlaySetting.luckySpinProgressMax)
        {
            GameManager.Instance.currentLuckySpinProgress++;
        }

        rewardVictoryProgressTxt.text = GameManager.Instance.currentRewardVictoryProgress + "/" + GameManager.Instance.gamePlaySetting.rewardVictoryProgressMax;
        rewardVictoryProgressImg.fillAmount = (float)GameManager.Instance.currentRewardVictoryProgress / (float)GameManager.Instance.gamePlaySetting.rewardVictoryProgressMax;

        luckySpinProgressTxt.text = GameManager.Instance.currentLuckySpinProgress + "/" + GameManager.Instance.gamePlaySetting.luckySpinProgressMax;
        luckySpinProgressImg.fillAmount = (float)GameManager.Instance.currentLuckySpinProgress / (float)GameManager.Instance.gamePlaySetting.luckySpinProgressMax;

        //reset if it reach max value
        if (GameManager.Instance.currentRewardVictoryProgress == GameManager.Instance.gamePlaySetting.rewardVictoryProgressMax)
        {
            GameManager.Instance.uiManager.rewardVictoryView.InitView();
            GameManager.Instance.uiManager.rewardVictoryView.ShowView();
            GameManager.Instance.currentRewardVictoryProgress = 0;
        }

        if (GameManager.Instance.currentLuckySpinProgress == GameManager.Instance.gamePlaySetting.luckySpinProgressMax)
        {
            GameManager.Instance.uiManager.luckyWheelView.InitView();
            GameManager.Instance.uiManager.luckyWheelView.ShowView();
            GameManager.Instance.currentLuckySpinProgress = 0;
        }

        PlayerPrefs.SetInt("Spin", GameManager.Instance.currentLuckySpinProgress);
        PlayerPrefs.SetInt("VictoryReward", GameManager.Instance.currentRewardVictoryProgress);

        PlayerPrefs.SetInt("XPRank", GameManager.Instance.currentXPRank);
    }

    public override void ShowView()
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        isShow = true;

        contentTrans.gameObject.SetActive(false);
        victoryBanner.localScale = new Vector3(0.5f, 1.0f, 1.0f);
        //arrowTrans.localPosition = new Vector3(-200.0f, 73.0f, 0.0f);
        lightTrans.DOLocalRotate(new Vector3(0.0f, 0.0f, -360.0f), 2.0f).SetRelative().SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);

        propBg.alpha = 0.0f;
        adsMulti.alpha = 0.0f;
        btnGroup.alpha = 0.0f;
        nextBtn.alpha = 0.0f;

        victoryBanner.DOScaleX(1.0f, 0.25f).SetEase(Ease.Linear).OnComplete
            (
            () =>
            {
                contentTrans.gameObject.SetActive(true);
                contentTrans.localScale = new Vector3(1.0f, 0.5f, 1.0f);
                contentTrans.DOScaleY(1.0f, 0.25f).SetEase(Ease.Linear).OnComplete
                (
                    () =>
                    {

                        propBg.DOFade(1.0f, 0.35f).SetEase(Ease.Linear).OnComplete
                        (
                            () =>
                            {

                                adsMulti.DOFade(1.0f, 0.35f).SetEase(Ease.Linear).OnComplete
                                (
                                () =>
                                {
                                    if (moveArrowTween == null)
                                        moveArrowTween = arrowTrans.DOLocalMoveX(400, 1.5f).SetRelative(true).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).OnUpdate
                                          (
                                              () =>
                                              {
                                                  if (arrowTrans.localPosition.x >= -200.0f && arrowTrans.localPosition.x <= -150.0f)
                                                  {
                                                      claimTxt.text = "Claim X2";
                                                      multiAdsCount = 2;
                                                  }
                                                  else if (arrowTrans.localPosition.x >= -150.0f && arrowTrans.localPosition.x <= -75.0f)
                                                  {
                                                      claimTxt.text = "Claim X3";
                                                      multiAdsCount = 3;
                                                  }
                                                  else if (arrowTrans.localPosition.x >= -75.0f && arrowTrans.localPosition.x <= -20.0f)
                                                  {
                                                      claimTxt.text = "Claim X4";
                                                      multiAdsCount = 4;
                                                  }

                                                  else if (arrowTrans.localPosition.x >= -20.0f && arrowTrans.localPosition.x <= 20.0f)
                                                  {
                                                      claimTxt.text = "Claim X5";
                                                      multiAdsCount = 5;
                                                  }
                                                  else if (arrowTrans.localPosition.x >= 20.0f && arrowTrans.localPosition.x <= 75.0f)
                                                  {
                                                      claimTxt.text = "Claim X4";
                                                      multiAdsCount = 4;
                                                  }
                                                  else if (arrowTrans.localPosition.x >= 75.0f && arrowTrans.localPosition.x <= 150.0f)
                                                  {
                                                      claimTxt.text = "Claim X3";
                                                      multiAdsCount = 4;
                                                  }
                                                  else if (arrowTrans.localPosition.x >= 150.0f && arrowTrans.localPosition.x <= 200.0f)
                                                  {
                                                      claimTxt.text = "Claim X2";
                                                      multiAdsCount = 2;
                                                  }
                                              }
                                          );
                                    else moveArrowTween.Play();

                                    btnGroup.DOFade(1.0f, 0.35f).SetEase(Ease.Linear).OnComplete
                                    (
                                      () =>
                                      {

                                          nextBtn.DOFade(1.0f, 0.35f).SetDelay(2.0f).SetEase(Ease.Linear).OnComplete
                                          (
                                            () =>
                                            {

                                            }
                                          );
                                      }
                                    );

                                }
                                );


                            }
                        );

                    }
                );

            }
            );
    }

    public override void Start()
    {
        localPos1 = starVfx1.transform.position;
        localPos2 = starVfx2.transform.position;
        localPos3 = starVfx3.transform.position;
    }

    public override void Update()
    {

    }

    public void ClaimRW()
    {
        AudioManager.instance.btnSound.Play();

        if (AdsControl.Instance.currentAdsType == ADS_TYPE.ADMOB)
        {
            if (AdsControl.Instance.rewardedAd != null)
            {
                if (AdsControl.Instance.rewardedAd.CanShowAd())
                {
                    AdsControl.Instance.ShowRewardAd(EarnMulti);
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

                AdsControl.Instance.ShowRewardAd(EarnMulti);

            else
                ShowRWUnityAds();
        }

    }

    public void NextLevel()
    {
        AudioManager.instance.btnSound.Play();
        GameManager.Instance.SaveStar();
        moveArrowTween.Pause();

        if (GameManager.Instance.currentLevel >= 2)
            AdsControl.Instance.ShowInterstital();

        GameManager.Instance.NextLevel();
    }

    IEnumerator WaitVfx()
    {
        yield return new WaitForSeconds(1.5f);
        GameManager.Instance.NextLevel();
        claimButton.interactable = true;
        nextButton.interactable = true;
    }

    public void PlayStarVfx()
    {
        AudioManager.instance.moreCoinSound.Play();
        starVfx1.SetActive(true);
        starVfx1.transform.DOMove(starVfxTarget.position, 0.5f).OnComplete(
            () =>
            {
                starVfx1.SetActive(false);
                starVfx1.transform.position = localPos1;
            }
            );

        starVfx2.gameObject.SetActive(true);
        starVfx2.transform.DOMove(starVfxTarget.position, 0.5f).SetDelay(0.25f).OnComplete(
            () =>
            {
                starVfx2.gameObject.SetActive(false);
                starVfx2.transform.position = localPos2;
            }
            );

        starVfx3.gameObject.SetActive(true);
        starVfx3.transform.DOMove(starVfxTarget.position, 0.5f).SetDelay(0.5f).OnComplete
            (
            () =>
            {
                starVfx3.gameObject.SetActive(false);
                starVfx3.transform.position = localPos3;
                gotStarTxt.text = (GameManager.Instance.inGameStar).ToString();
            }
            );
    }

    public void EarnMulti(Reward reward)
    {
        //Debug.Log("CLAIM MULTI STAR");
        GameManager.Instance.inGameStar *= multiAdsCount;
        GameManager.Instance.SaveStar();
        moveArrowTween.Pause();
        claimButton.interactable = false;
        nextButton.interactable = false;

        PlayStarVfx();
        StartCoroutine(WaitVfx());
    }

    public void ShowRWUnityAds()
    {
        AdsControl.Instance.PlayUnityVideoAd((string ID, UnityAdsShowCompletionState callBackState) =>
        {

            if (ID.Equals(AdsControl.Instance.adUnityRWUnitId) && callBackState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                //Debug.Log("CLAIM MULTI STAR");
                GameManager.Instance.inGameStar *= multiAdsCount;
                GameManager.Instance.SaveStar();
                moveArrowTween.Pause();
                claimButton.interactable = false;
                nextButton.interactable = false;

                PlayStarVfx();
                StartCoroutine(WaitVfx());
            }

            if (ID.Equals(AdsControl.Instance.adUnityRWUnitId) && callBackState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                AdsControl.Instance.LoadUnityAd();
            }

        });
    }

}
