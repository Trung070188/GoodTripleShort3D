using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MileStoneView : MonoBehaviour
{
    public Image rewardIcon;

    public Text levelUnlockTxt;

    public Text titleTxt;

    public Image progressBar;

    public Color lockColor;

    public Color unLockColor;

    public MileStoneItem currentItem;

    public GameObject gotObj;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowView(MileStoneItem item)
    {
        currentItem = item;
        levelUnlockTxt.text = "LV." + item.levelUnlock.ToString();
        titleTxt.text = item.name;
        progressBar.fillAmount = (float)GameManager.Instance.currentLevel / (float)item.levelUnlock;

        if (GameManager.Instance.currentLevel < item.levelUnlock)
            rewardIcon.color = lockColor;
        else
            rewardIcon.color = unLockColor;
        if (currentItem.gotReward == 1)
            gotObj.SetActive(true);

    }

    public void Unlock()
    {
       
        if (currentItem.gotReward == 0 && currentItem.levelUnlock <= GameManager.Instance.currentLevel)
        {
            AudioManager.instance.btnSound.Play();
            currentItem.gotReward = 1;
            PlayerPrefs.SetInt("GotReward" + currentItem.mileStoneID.ToString(), 1);
            ShowView(currentItem);
            //show reward
            GameManager.Instance.uiManager.rewardAchievementView.InitView(GameManager.Instance.gamePlaySetting.openChestReward);
            GameManager.Instance.uiManager.rewardAchievementView.ShowView();
        }
       

    }
}
