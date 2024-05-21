using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GamePlaySetting gamePlaySetting;

    public ParticleSystem comboVfx;

    public ParticleSystem winVfx;

    public ClearVfx clearVfx;

    public FlyingStarRoot starRoot;

    public BoardGenerator gameBoard;

    public UIManager uiManager;

    public LivesManager livesManager;

    public FreeAdsTimer freeAdsTimer;

    [HideInInspector]
    public int collectedStar;

    [HideInInspector]
    public int currentLevel;

    [HideInInspector]
    public int currentHint;

    [HideInInspector]
    public int currentShuffle;

    [HideInInspector]
    public int currentFreeze;

    public bool isTestLevel;

    public int testLevel;

    [HideInInspector]
    public int currentCoin;

    [HideInInspector]
    public int currentStar;

    [HideInInspector]
    public int inGameStar;

    [HideInInspector]
    public int currentHeart;

    [HideInInspector]
    public int currentXPRank;

    [HideInInspector]
    public int currentLuckySpinProgress;

    [HideInInspector]
    public int currentRewardVictoryProgress;

    public enum GAME_STATE
    {
        IN_HOME,
        IN_GAME,
        IN_GAME_POPUP,
        TIME_OUT,
        LEVEL_CLEAR
    };

    public GAME_STATE currentState;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        //InitGame();
        LoadGameData();
        currentState = GAME_STATE.IN_HOME;
        uiManager.homeView.InitView();
        uiManager.homeView.ShowView();
        uiManager.storeView.InitView();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.R))
        {
            uiManager.rewardAchievementView.InitView(gamePlaySetting.rewardPackage1);
            uiManager.rewardAchievementView.ShowView();
        }
        */

    }

    public void UnlockStamp()
    {
        int packageID = 0;
        int stampID = Random.Range(0, 8);
        int isUnlock = 0; ;

        if (currentLevel >= 5 && currentLevel < 10)
        {
            packageID = 0;
            isUnlock = PlayerPrefs.GetInt("Stamp" + packageID + "_" + stampID);
            if (isUnlock == 0)
            {
                uiManager.albumView.UnlockItem(0, stampID);
                uiManager.unlockStampView.InitView(packageID, stampID);
                uiManager.unlockStampView.ShowView();
            }
               
        }

        else if (currentLevel >= 10 && currentLevel < 20)
        {
            packageID = Random.Range(0, 2);
            isUnlock = PlayerPrefs.GetInt("Stamp" + packageID + "_" + stampID);
            if (isUnlock == 0)
            {
                uiManager.albumView.UnlockItem(packageID, stampID);
                uiManager.unlockStampView.InitView(packageID, stampID);
                uiManager.unlockStampView.ShowView();
            }
                
        }

        else if (currentLevel >= 20)
        {
            packageID = Random.Range(0, 3);
            isUnlock = PlayerPrefs.GetInt("Stamp" + packageID + "_" + stampID);
            if (isUnlock == 0)
            {
                uiManager.albumView.UnlockItem(packageID, stampID);
                uiManager.unlockStampView.InitView(packageID, stampID);
                uiManager.unlockStampView.ShowView();
            }
        }
    }

    public void ShowGameWin()
    {
        StartCoroutine(ShowGameWinIE());
    }

    public void ShowGameFail()
    {
        GameManager.Instance.uiManager.failView.ShowView();
    }

    IEnumerator ShowGameWinIE()
    {
        yield return new WaitForSeconds(1.0f);
        AudioManager.instance.fireworkSound.Play();
        winVfx.gameObject.SetActive(true);
        winVfx.Play();
        yield return new WaitForSeconds(1.5f);
        uiManager.gameView.HideView();
        AudioManager.instance.winSound.Play();
        uiManager.winView.InitView();
        uiManager.winView.ShowView();
    }

    public void InitGame()
    {
        currentState = GAME_STATE.IN_GAME;
        uiManager.homeView.HideView();
        gameBoard.GenBoard();
        uiManager.gameView.InitView();
        uiManager.gameView.ShowView();
    }

    private void LoadGameData()
    {
        if (PlayerPrefs.GetInt("FirstOpen") == 0)
        {
            PlayerPrefs.SetInt("Coin", 100);
            PlayerPrefs.SetInt("Star", 0);
            PlayerPrefs.SetInt("Heart", 5);

            PlayerPrefs.SetInt("Hint", 1);
            PlayerPrefs.SetInt("Shuffle", 1);
            PlayerPrefs.SetInt("Freeze", 1);

            PlayerPrefs.SetInt("FirstOpen", 1);
        }

        currentLevel = PlayerPrefs.GetInt("Level");

        if (currentLevel == 0)
        {
            currentLevel = 1;
            PlayerPrefs.SetInt("Level", 1);
        }

        if (isTestLevel)
            currentLevel = testLevel;

        currentCoin = PlayerPrefs.GetInt("Coin");
        currentStar = PlayerPrefs.GetInt("Star");
        currentHeart = PlayerPrefs.GetInt("Heart");

        currentHint = PlayerPrefs.GetInt("Hint");
        currentShuffle = PlayerPrefs.GetInt("Shuffle");
        currentFreeze = PlayerPrefs.GetInt("Freeze");

        currentLuckySpinProgress = PlayerPrefs.GetInt("Spin");
        currentRewardVictoryProgress = PlayerPrefs.GetInt("VictoryReward");

        currentXPRank = PlayerPrefs.GetInt("XPRank");
    }

    public void ReplayLevel()
    {
        GameManager.Instance.uiManager.gameLoadingView.ShowView();
        winVfx.Stop();
        winVfx.gameObject.SetActive(false);
        uiManager.winView.HideView();
        gameBoard.CleanBoard();
        currentState = GAME_STATE.IN_GAME;
        inGameStar = 0;
        gameBoard.GenBoard();
        uiManager.gameView.InitView();
        uiManager.gameView.ShowView();
    }

    public void BackToHome()
    {
        gameBoard.CleanBoard();
        uiManager.gameView.HideView();
        currentState = GAME_STATE.IN_HOME;
        GameManager.Instance.uiManager.gameLoadingView.ShowView();
        inGameStar = 0;
        uiManager.homeView.InitView();
        uiManager.homeView.ShowView();
        uiManager.storeView.InitView();
    }

    public void NextLevel()
    {
        GameManager.Instance.uiManager.gameLoadingView.ShowView();
        winVfx.Stop();
        winVfx.gameObject.SetActive(false);
        uiManager.winView.HideView();
        gameBoard.CleanBoard();
        currentLevel++;
        PlayerPrefs.SetInt("Level", currentLevel);
        if (isTestLevel)
            currentLevel = testLevel;

        currentState = GAME_STATE.IN_GAME;
        inGameStar = 0;
        gameBoard.GenBoard();
        uiManager.gameView.InitView();
        uiManager.gameView.ShowView();
    }

    public void AddCoin(int moreCoin)
    {
        currentCoin += moreCoin;
        PlayerPrefs.SetInt("Coin", currentCoin);
        uiManager.homeView.UpdateCoinTxt();
        uiManager.storeView.UpdateCoinTxt();

    }

    public void SaveStar()
    {
        currentStar += inGameStar;
        PlayerPrefs.SetInt("Star", currentStar);
        uiManager.homeView.UpdateStarTxt();
        uiManager.storeView.UpdateStarTxt();
    }

    public void AddHint(int moreHint)
    {
        currentHint += moreHint;
        PlayerPrefs.SetInt("Hint", currentHint);
        uiManager.gameView.RefreshBoostValue();
    }

    public void AddShuffle(int moreShuffle)
    {
        currentShuffle += moreShuffle;
        PlayerPrefs.SetInt("Shuffle", currentShuffle);
        uiManager.gameView.RefreshBoostValue();
    }

    public void AddFreeze(int moreFreeze)
    {
        currentFreeze += moreFreeze;
        PlayerPrefs.SetInt("Freeze", currentFreeze);
        uiManager.gameView.RefreshBoostValue();
    }
}
