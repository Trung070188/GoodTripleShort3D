using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardAchievementView : BaseView
{
    private List<RewardModel> rewardList = new List<RewardModel>();

    public GameObject coinRewardItem;

    public GameObject hintRewardItem;

    public GameObject shuffleRewardItem;

    public GameObject freezeRewardItem;

    public Button claimBtn;

    public Text coinRewardTxt;

    public Text hintRewardTxt;

    public Text shuffleRewardTxt;

    public Text freezeRewardTxt;

    private bool hasCoin;

    public override void InitView()
    {
       
    }

    public void InitView(List<RewardModel> rewardPackage)
    {
        hasCoin = false;
        claimBtn.interactable = true;
        rewardList = rewardPackage;
        ShowRewardType();
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
    }

    public override void HideView()
    {
        base.HideView();
        coinRewardItem.SetActive(false);
        hintRewardItem.SetActive(false);
        shuffleRewardItem.SetActive(false);
        freezeRewardItem.SetActive(false);
        
    }

    public void Claim()
    {
        AudioManager.instance.moreCoinSound.Play();
        claimBtn.interactable = false;
        if (hasCoin)
        {
            GameManager.Instance.uiManager.coinView.InitView();
            GameManager.Instance.uiManager.coinView.ShowView();
            GameManager.Instance.uiManager.coinView.UpdateCoinPanelVfx();
            GetAllReward();
            StartCoroutine(ClaimIE());
        }
        else
        {
            GetAllReward();
            HideView();
        }
      
    }

    IEnumerator ClaimIE()
    {
        yield return new WaitForSeconds(2.0f);
        GameManager.Instance.uiManager.coinView.HideView();
        HideView();
    }

    private void ShowRewardType()
    {
        for(int i = 0; i < rewardList.Count; i++)
        {
            if (rewardList[i].rewardType == RewardModel.RewardType.COIN)
            {
                coinRewardItem.SetActive(true);
                coinRewardTxt.text = "+" + rewardList[i].rewardValue.ToString();
                hasCoin = true;
            }
                
            if (rewardList[i].rewardType == RewardModel.RewardType.HINT)
            {
                hintRewardItem.SetActive(true);
                hintRewardTxt.text = "+" + rewardList[i].rewardValue.ToString();
            }
               
            if (rewardList[i].rewardType == RewardModel.RewardType.SHUFFLE)
            {
                shuffleRewardItem.SetActive(true);
                shuffleRewardTxt.text = "+" + rewardList[i].rewardValue.ToString();
            }
                
            if (rewardList[i].rewardType == RewardModel.RewardType.FREEZE)
            {
                freezeRewardItem.SetActive(true);
                freezeRewardTxt.text = "+" + rewardList[i].rewardValue.ToString();
            }
               
        }
    }

    private void GetAllReward()
    {
        for (int i = 0; i < rewardList.Count; i++)
        {
            if (rewardList[i].rewardType == RewardModel.RewardType.COIN)
            {
                GameManager.Instance.AddCoin(rewardList[i].rewardValue);
            }

            if (rewardList[i].rewardType == RewardModel.RewardType.HINT)
            {
                GameManager.Instance.AddHint(rewardList[i].rewardValue);
            }

            if (rewardList[i].rewardType == RewardModel.RewardType.SHUFFLE)
            {
                GameManager.Instance.AddShuffle(rewardList[i].rewardValue);
            }

            if (rewardList[i].rewardType == RewardModel.RewardType.FREEZE)
            {
                GameManager.Instance.AddFreeze(rewardList[i].rewardValue);
            }

        }
    }

}

[System.Serializable]
public class RewardModel
{
    public enum RewardType
    {
        COIN,
        HINT,
        SHUFFLE,
        FREEZE
    }

    public RewardType rewardType;

    public int rewardValue;

}