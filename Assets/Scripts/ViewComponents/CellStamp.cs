using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellStamp : MonoBehaviour
{
    public Text stampNameTxt;

    public Text unlockTxt;

    public Image stampIcon;

    public Image bgImage;

    public Sprite[] stampSprList;

    public Color lockColor;

    public Color unlockColor;

    public GameObject progressRoot;

    public Image progressBar;

    public Text progressValueTxt;

    public GameObject finishObj;

    public StampData currentStamp;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowView(StampData stampData)
    {
        currentStamp = stampData;
        stampNameTxt.text = stampData.stampName;
        stampIcon.sprite = stampSprList[stampData.stampID];

        if (stampData.stampState == 0)
        {
            bgImage.color = lockColor;
            stampIcon.color = lockColor;
            stampNameTxt.color = lockColor;
            unlockTxt.gameObject.SetActive(true);
            unlockTxt.text = "Level " + stampData.levelUnlock.ToString();
            progressRoot.SetActive(false);
            finishObj.SetActive(false);
        }
        else if (stampData.stampState == 1)
        {
            bgImage.color = unlockColor;
            stampIcon.color = unlockColor;
            stampNameTxt.color = unlockColor;
            unlockTxt.gameObject.SetActive(false);
            progressRoot.SetActive(true);
            progressBar.fillAmount = (float)stampData.stampProgress / 8.0f;
            progressValueTxt.text = stampData.stampProgress.ToString() + "/8";
            finishObj.SetActive(false);
        }
        else if (stampData.stampState == 2)
        {
            bgImage.color = unlockColor;
            stampIcon.color = unlockColor;
            stampNameTxt.color = unlockColor;
            unlockTxt.gameObject.SetActive(false);
            progressRoot.SetActive(false);
            finishObj.SetActive(true);
        }

    }

    public void Unlock()
    {
        bgImage.color = unlockColor;
        stampIcon.color = unlockColor;
        stampNameTxt.color = unlockColor;
        unlockTxt.gameObject.SetActive(false);
        progressRoot.SetActive(true);
        progressBar.fillAmount = (float)currentStamp.stampProgress / 8.0f;
        progressValueTxt.text = currentStamp.stampProgress.ToString() + "/8";
        finishObj.SetActive(false);
    }

    public void MoreUnlock()
    {
        currentStamp.stampProgress++;
        progressBar.fillAmount = (float)currentStamp.stampProgress / 8.0f;
        progressValueTxt.text = currentStamp.stampProgress.ToString() + "/8";
        PlayerPrefs.SetInt("StampProgress" + currentStamp.stampID.ToString(), currentStamp.stampProgress);
        if (currentStamp.stampProgress == 8)
            finishObj.SetActive(true);
    }

    public void ShowDetail()
    {
        AudioManager.instance.btnSound.Play();
        if (currentStamp.stampProgress == 8 && currentStamp.stampState == 1)
        {
            currentStamp.stampState = 2;
            bgImage.color = unlockColor;
            stampIcon.color = unlockColor;
            stampNameTxt.color = unlockColor;
            unlockTxt.gameObject.SetActive(false);
            progressRoot.SetActive(false);
            finishObj.SetActive(true);
            PlayerPrefs.SetInt("StampState" + currentStamp.stampID, 2);

            GameManager.Instance.uiManager.rewardAchievementView.InitView(GameManager.Instance.gamePlaySetting.unlockStampReward);
            GameManager.Instance.uiManager.rewardAchievementView.ShowView();
        }

        if (currentStamp.stampState == 1 || currentStamp.stampState == 2)
        {
            GameManager.Instance.uiManager.albumView.InitView(currentStamp.stampID, currentStamp.stampName);
            GameManager.Instance.uiManager.albumView.ShowView();
        }

    }
}
