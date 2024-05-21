using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeView : BaseView
{
    public SliderView sliderView;

    public Text coinTxt;

    public Text starTxt;

    public Text heartTxt;

    public Text heartFullTxt;

    public GameObject addHeart;

    public override void InitView()
    {
        sliderView.InitView();

        UpdateCoinTxt();
        UpdateStarTxt();
        //heartTxt.text = GameManager.Instance.currentHeart.ToString();

        /*
        if (GameManager.Instance.currentHeart >= 5)
        {
            addHeart.SetActive(false);
            heartFullTxt.gameObject.SetActive(true);
            heartFullTxt.text = "Full";
        }
        else
        {
            addHeart.SetActive(true);
            heartFullTxt.gameObject.SetActive(false);
        }
         */  
    }

    public void UpdateCoinTxt()
    {
        coinTxt.text = GameManager.Instance.currentCoin.ToString();
    }

    public void UpdateStarTxt()
    {
        starTxt.text = GameManager.Instance.currentStar.ToString();
    }

    public override void Start()
    {
        
    }

    public override void Update()
    {
        //for test
        if (Input.GetKeyUp(KeyCode.C))
        {
            OnButtonConsumePressed();
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            OnButtonGiveOnePressed();
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            OnButtonFillPressed();
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            OnButtonIncreaseMaxPressed();
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            GameManager.Instance.livesManager.AddLifeSlots(1, true);
        }

        if (Input.GetKeyUp(KeyCode.I))
        {
            OnButtonInfinitePressed();
        }
    }

    public override void ShowView()
    {
        base.ShowView();
        sliderView.ShowMidleNode();
        //AudioManager.instance.btnSound.Play();
    }

    public override void HideView()
    {
        base.HideView();
        AudioManager.instance.btnSound.Play();
    }

    public void OnLivesChanged()
    {
        Debug.Log("Update Lives : " + GameManager.Instance.livesManager.LivesText);
        heartTxt.text = GameManager.Instance.livesManager.LivesText;
        GameManager.Instance.uiManager.storeView.OnLivesChanged();
    }

    public void OnTimeToNextLifeChanged()
    {
        heartFullTxt.text = GameManager.Instance.livesManager.RemainingTimeString;
        GameManager.Instance.uiManager.storeView.OnTimeToNextLifeChanged();
    }

    public void OnButtonConsumePressed()
    {
        if (GameManager.Instance.livesManager.ConsumeLife())
        {
            // Go to your game!
            Debug.Log("A life was consumed and the player can continue!");
            //ResultDisplay.Show(true);
        }
        else
        {
            // Tell player to buy lives, then:
            // LivesManager.GiveOneLife();
            // or
            // LivesManager.FillLives();
            Debug.Log("Not enough lives to play!");
           // ResultDisplay.Show(false);
        }
    }

    public void OnButtonGiveOnePressed()
    {
        GameManager.Instance.livesManager.GiveOneLife();
    }

    public void OnButtonFillPressed()
    {
        GameManager.Instance.livesManager.FillLives();
    }

    public void OnButtonInfinitePressed()
    {
        GameManager.Instance.livesManager.GiveInifinite(30);
    }

    public void OnButtonIncreaseMaxPressed()
    {
        GameManager.Instance.livesManager.AddLifeSlots(1);
        Debug.LogFormat("Max lives is now {0}", GameManager.Instance.livesManager.MaxLives);
    }

    public void OnButtonResetPressed()
    {
        GameManager.Instance.livesManager.ResetPlayerPrefs();
        Debug.LogFormat("Max lives is now {0}", GameManager.Instance.livesManager.MaxLives);
        OnLivesChanged();
        OnTimeToNextLifeChanged();
    }

    public void ShowSetting()
    {
        AudioManager.instance.btnSound.Play();
        GameManager.Instance.uiManager.settingView.ShowView();
    }

    public void ShowShop()
    {
        AudioManager.instance.btnSound.Play();
        sliderView.ShowNodeByIndex(0);
    }

  
}
