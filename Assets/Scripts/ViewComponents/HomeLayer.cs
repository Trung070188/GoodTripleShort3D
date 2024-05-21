using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeLayer : SliderNode
{
    public Text openChestProgressTxt;

    public Image openChestProgressBar;

    public Text luckySpinProgressTxt;

    public Image luckySpinProgressBar;

    public override void InitView()
    {
        openChestProgressTxt.text = GameManager.Instance.currentStar + "/" + GameManager.Instance.gamePlaySetting.openChestNumber;
        luckySpinProgressTxt.text = GameManager.Instance.currentLuckySpinProgress + "/" + GameManager.Instance.gamePlaySetting.luckySpinProgressMax;

        openChestProgressBar.fillAmount = (float)(GameManager.Instance.currentStar) / (float)(GameManager.Instance.gamePlaySetting.openChestNumber);
        luckySpinProgressBar.fillAmount = (float)(GameManager.Instance.currentLuckySpinProgress) / (float)(GameManager.Instance.gamePlaySetting.luckySpinProgressMax);
    }
        // Start is called before the first frame update
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        if (GameManager.Instance.livesManager.lives >= 1)
        {
            Debug.Log("PLAY GAME");
            GameManager.Instance.InitGame();
            GameManager.Instance.uiManager.gameLoadingView.ShowView();
        }
        else
            GameManager.Instance.uiManager.homeView.sliderView.ShowNodeByIndex(0);
    }

    public void OpenChest()
    {
        AudioManager.instance.btnSound.Play();
        if (GameManager.Instance.currentStar >= GameManager.Instance.gamePlaySetting.openChestNumber)
        {
            GameManager.Instance.currentStar -= GameManager.Instance.gamePlaySetting.openChestNumber;
            GameManager.Instance.SaveStar();
            GameManager.Instance.uiManager.homeView.UpdateStarTxt();

            openChestProgressTxt.text = GameManager.Instance.currentStar + "/" + GameManager.Instance.gamePlaySetting.openChestNumber;
            openChestProgressBar.fillAmount = (float)(GameManager.Instance.currentStar) / (float)(GameManager.Instance.gamePlaySetting.openChestNumber);

            GameManager.Instance.uiManager.rewardAchievementView.InitView(GameManager.Instance.gamePlaySetting.openChestReward);
            GameManager.Instance.uiManager.rewardAchievementView.ShowView();
            AudioManager.instance.openGiftSound.Play();
        }
    }
}
