using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameView : BaseView
{
    [HideInInspector]
    public int currentCollectWares;

    [HideInInspector]
    public int collectWaresMax;

    [HideInInspector]
    public int collectStars;

    [HideInInspector]
    public int comboCount;

    [HideInInspector]
    public int timerMax;

    [HideInInspector]
    public float currentTime;

    public Text stageProgressTxt;

    public Image stageProgressBar;

    public Image comboLoading;

    public Text currentLevelTxt;

    public Text collectStarTxt;

    public Text timerTxt;

    public Text collectedStarTxt;

    public Text comboTxt;

    public Text currentRankTxt;

    public Text nextRankTxt;

    public Image rankProcessImg;

    public CanvasGroup comboTransform;

    public enum ComboState
    {
        HIDE,
        SHOWING,
        MOVING_FORWARD,
        MOVING_BACK
    };

    public ComboState currentComboState;

    private Tween comboTween;

    public BoostItem hintItem;

    public BoostItem shuffleItem;

    public BoostItem freezeItem;

    private bool InFreeze;

    public GameObject frozenPanel;

    public GameObject tutObj;

    public override void InitView()
    {
        if (GameManager.Instance.currentLevel == 1)
        {
            StartCoroutine(ShowTut());
            StartCoroutine(HideTutIE());
        }
        else
            tutObj.SetActive(false);
        

        comboTransform.alpha = 0.0f;
        currentCollectWares = 0;
        GameManager.Instance.inGameStar = 0;
        collectWaresMax = GameManager.Instance.gameBoard.currentLv.totalProductCount * 3;

        stageProgressTxt.text = currentCollectWares.ToString() + "/" + collectWaresMax.ToString();
        stageProgressBar.rectTransform.sizeDelta = new Vector2(140.0f * (float)currentCollectWares / (float)collectWaresMax, 16.0f);
        currentLevelTxt.text = "LV " + GameManager.Instance.currentLevel;

        collectStars = 0;
        collectStarTxt.text = collectStars.ToString();

        timerMax = GameManager.Instance.gameBoard.currentLv.time;

        if (timerMax == -1)
            timerMax = 200;

        currentTime = timerMax;
        timerTxt.gameObject.SetActive(true);

        collectedStarTxt.text = GameManager.Instance.inGameStar.ToString();

        comboCount = 0;
        currentComboState = ComboState.HIDE;
        InFreeze = false;
        frozenPanel.SetActive(false);

        hintItem.SetLock();
        shuffleItem.SetLock();
        freezeItem.SetLock();

        if (GameManager.Instance.currentLevel >= 2)
        {
            if (GameManager.Instance.currentHint > 0)
                hintItem.SetAvailable(GameManager.Instance.currentHint);
            else
                hintItem.SetUnAvailable();
        }

        if (GameManager.Instance.currentLevel >= 3)
        {
            if (GameManager.Instance.currentShuffle > 0)
                shuffleItem.SetAvailable(GameManager.Instance.currentShuffle);
            else
                shuffleItem.SetUnAvailable();
        }

        if (GameManager.Instance.currentLevel >= 4)
        {
            if (GameManager.Instance.currentFreeze > 0)
                freezeItem.SetAvailable(GameManager.Instance.currentFreeze);
            else
                freezeItem.SetUnAvailable();
        }

        UpdateRankBar();
    }

    public void UpdateRankBar()
    {
        int currentRankValue = (1 + GameManager.Instance.currentXPRank / GameManager.Instance.gamePlaySetting.rankStep);
        int nextRankValue = currentRankValue + 1;
        currentRankTxt.text = currentRankValue.ToString();
        nextRankTxt.text = nextRankValue.ToString();
        rankProcessImg.fillAmount = (float)(GameManager.Instance.currentXPRank - GameManager.Instance.gamePlaySetting.rankStep * (currentRankValue - 1))
            / (float)(GameManager.Instance.gamePlaySetting.rankStep);

    }

    public void RefreshBoostValue()
    {
        if (GameManager.Instance.currentHint > 0)
            hintItem.SetAvailable(GameManager.Instance.currentHint);
        else
            hintItem.SetUnAvailable();

        if (GameManager.Instance.currentShuffle > 0)
            shuffleItem.SetAvailable(GameManager.Instance.currentShuffle);
        else
            shuffleItem.SetUnAvailable();

        if (GameManager.Instance.currentFreeze > 0)
            freezeItem.SetAvailable(GameManager.Instance.currentFreeze);
        else
            freezeItem.SetUnAvailable();
    }

    public override void Start()
    {

    }

    IEnumerator ShowTut()
    {
        yield return new WaitForSeconds(1.0f);
        tutObj.SetActive(true);
    }

    IEnumerator HideTutIE()
    {
        yield return new WaitForSeconds(6.0f);
        tutObj.SetActive(false);
    }

    public void HideTut()
    {
        tutObj.SetActive(false);
    }

    public override void Update()
    {

        if (GameManager.Instance.currentState == GameManager.GAME_STATE.IN_GAME && !InFreeze)
            UpdateTimer();
    }

    public void UpdateStageProgress()
    {
        currentCollectWares += 3;

        int randomUnlockStamp = 0;

         randomUnlockStamp = UnityEngine.Random.Range(0, 20);
        if (randomUnlockStamp == 10)
            GameManager.Instance.UnlockStamp();

        GameManager.Instance.currentXPRank++;
        if ((GameManager.Instance.currentXPRank % GameManager.Instance.gamePlaySetting.rankStep) == 0)
            GameManager.Instance.uiManager.rwRankView.ShowView();
        UpdateRankBar();

        stageProgressTxt.text = currentCollectWares.ToString() + "/" + collectWaresMax.ToString();
        stageProgressBar.rectTransform.sizeDelta = new Vector2(140.0f * (float)currentCollectWares / (float)collectWaresMax, 16.0f);
        comboCount++;
        if (comboCount >= 2)
        {
            comboTxt.text = "X" + (comboCount - 1).ToString();
            if (currentComboState == ComboState.HIDE)
                ShowCombo();
            else if (currentComboState == ComboState.SHOWING)
            {
                currentComboProgressMax = 100;
                comboTween.Restart();
            }
            else if (currentComboState == ComboState.MOVING_FORWARD)
            {

            }
            else if (currentComboState == ComboState.MOVING_BACK)
            {
                ReturnCombo();
            }
        }

        if (currentCollectWares == collectWaresMax)
        {
            GameManager.Instance.currentState = GameManager.GAME_STATE.LEVEL_CLEAR;
            GameManager.Instance.ShowGameWin();
            Debug.Log("GAME WIN");
        }
    }

    public void GetStarCombo(int combo)
    {
        GameManager.Instance.inGameStar += combo;
        collectStarTxt.text = GameManager.Instance.inGameStar.ToString();
    }

    private void UpdateTimer()
    {
        if (GameManager.Instance.currentState == GameManager.GAME_STATE.TIME_OUT)
            return;

        if (GameManager.Instance.currentState == GameManager.GAME_STATE.LEVEL_CLEAR)
            return;

        if (currentTime - Time.deltaTime > 0.0f)
        {
            currentTime -= Time.deltaTime;
            timerTxt.text = TimeSpan.FromSeconds(currentTime).ToString(@"mm\:ss");
        }
        else
        {
            Debug.Log("Time OUT");
            timerTxt.gameObject.SetActive(false);
            GameManager.Instance.currentState = GameManager.GAME_STATE.TIME_OUT;
            GameManager.Instance.ShowGameFail();
        }
    }

    public void ResumeTimer()
    {
        GameManager.Instance.currentState = GameManager.GAME_STATE.IN_GAME;
        currentTime = 60;
        timerTxt.gameObject.SetActive(true);
    }

    private float currentComboProgress;

    private float currentComboProgressMax;

    private Vector3 comboPosOriginal;

    private void ShowCombo()
    {

        /*
        currentComboProgressMax = 100;
        currentComboState = ComboState.MOVING_FORWARD;
        comboTransform.DOLocalMoveX(-140, 0.25f).SetRelative(true).SetEase(Ease.Linear).OnComplete
        (
            () =>
            {
                currentComboState = ComboState.SHOWING;
            }
        );

        comboTween = DOTween.To(() => currentComboProgressMax, x => currentComboProgress = x, 0, 7.5f).SetEase(Ease.Linear)
              .OnUpdate(() =>
              {
                  //Debug.Log("current Combo " + currentComboProgress);
                  comboLoading.fillAmount = currentComboProgress / currentComboProgressMax;

              })
              .OnComplete(() =>
              {
                  HideCombo();
              });
        */

        currentComboState = ComboState.SHOWING;
        currentComboProgressMax = 100;
        comboTransform.alpha = 1.0f;
        comboTween = DOTween.To(() => currentComboProgressMax, x => currentComboProgress = x, 0, 7.5f).SetEase(Ease.Linear)
             .OnUpdate(() =>
             {
                 //Debug.Log("current Combo " + currentComboProgress);
                 comboLoading.fillAmount = currentComboProgress / currentComboProgressMax;

             })
             .OnComplete(() =>
             {
                 HideCombo();
             });
    }

    private void HideCombo()
    {
        /*
        currentComboState = ComboState.MOVING_BACK;
        comboTransform.DOLocalMoveX(140, 0.25f).SetRelative(true).SetEase(Ease.Linear).OnComplete
        (
            () =>
            {
                currentComboState = ComboState.HIDE;
                comboCount = 0;
            }
        );
        */
        currentComboState = ComboState.HIDE;
        comboCount = 0;
        comboTransform.alpha = 0.0f;
    }

    private void ReturnCombo()
    {
        /*
        currentComboState = ComboState.MOVING_FORWARD;
        float distance = 140.0f - comboTransform.localPosition.x;
        comboTransform.DOLocalMoveX(distance, 0.25f * distance / 140.0f).SetRelative(true).SetEase(Ease.Linear).OnComplete
        (
            () =>
            {
                currentComboState = ComboState.SHOWING;
            }
        );
        */
        comboTransform.alpha = 1.0f;
    }

    public void UseHint()
    {
        AudioManager.instance.btnSound.Play();
        if (hintItem.currentState == BoostItem.STATE.AVAILABLE)
        {
            Debug.Log("Use Hint");
            GameManager.Instance.gameBoard.ProcessHint();

            GameManager.Instance.currentHint--;
            PlayerPrefs.SetInt("Hint", GameManager.Instance.currentHint);

            if (GameManager.Instance.currentHint == 0)
                hintItem.SetUnAvailable();
        }
        else if (hintItem.currentState == BoostItem.STATE.UNAVAILABLE)
        {
            GameManager.Instance.uiManager.buyBoostView.InitView(BuyBoostView.BoostType.HINT);
            GameManager.Instance.uiManager.buyBoostView.ShowView();
        }

    }


    public void UseShuffle()
    {
        AudioManager.instance.btnSound.Play();
        if (shuffleItem.currentState == BoostItem.STATE.AVAILABLE)
        {
            Debug.Log("Use Shuffle");
            GameManager.Instance.gameBoard.ProcessShuffle();

            GameManager.Instance.currentShuffle--;
            PlayerPrefs.SetInt("Shuffle", GameManager.Instance.currentShuffle);

            if (GameManager.Instance.currentShuffle == 0)
                shuffleItem.SetUnAvailable();
        }
        else if (shuffleItem.currentState == BoostItem.STATE.UNAVAILABLE)
        {
            GameManager.Instance.uiManager.buyBoostView.InitView(BuyBoostView.BoostType.SHUFFLE);
            GameManager.Instance.uiManager.buyBoostView.ShowView();
        }

    }

    public void UseFreeze()
    {
        AudioManager.instance.btnSound.Play();
        if (freezeItem.currentState == BoostItem.STATE.AVAILABLE)
        {
            //Debug.Log("Use Freeze");
            InFreeze = true;
            StartCoroutine(DisableFreeze());

            GameManager.Instance.currentFreeze--;
            PlayerPrefs.SetInt("Freeze", GameManager.Instance.currentFreeze);

            if (GameManager.Instance.currentFreeze == 0)
                freezeItem.SetUnAvailable();
        }

        else if (freezeItem.currentState == BoostItem.STATE.UNAVAILABLE)
        {
            GameManager.Instance.uiManager.buyBoostView.InitView(BuyBoostView.BoostType.FREEZE);
            GameManager.Instance.uiManager.buyBoostView.ShowView();
        }
    }

    IEnumerator DisableFreeze()
    {
        frozenPanel.SetActive(true);
        yield return new WaitForSeconds(10.0f);
        InFreeze = false;
        frozenPanel.SetActive(false);
    }

    public void PauseGame()
    {
        AudioManager.instance.btnSound.Play();
        GameManager.Instance.uiManager.pauseView.ShowView();
    }

    public void RemoveAds()
    {
        AudioManager.instance.btnSound.Play();
        GameManager.Instance.uiManager.storeLayer.RemoveAds();
    }
}
