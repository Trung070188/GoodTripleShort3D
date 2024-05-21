using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankLayer : SliderNode
{
    public RankData rankingData;

    public CanvasGroup allTab;

    public CanvasGroup monthlyTab;

    public RankItem rankItemPrefab;

    public RectTransform allTabRoot;

    public RectTransform monthlyTabRoot;

    public GameObject allTabSelectObj;

    public GameObject monthlyTabSelectObj;

    public Text myRankingTxt;

    public Text myUserNameTxt;

    public Text myScoreTxt;

    private bool loaded;

    public override void InitView()
    {
        myRankingTxt.text = "100+";
        myUserNameTxt.text = "Your Ranking";
        myScoreTxt.text = GameManager.Instance.currentStar.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        loaded = false;
        LoadView();
        ChooseAllTab();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadView()
    {
        for(int i = 0; i < GameManager.Instance.gamePlaySetting.rankingSize; i++)
        {
            RankItem item = Instantiate(rankItemPrefab);
            item.ShowView(rankingData.rankDataList[i]);
            item.transform.SetParent(allTabRoot);
            item.transform.localScale = Vector3.one;
        }

        for (int i = 0; i < GameManager.Instance.gamePlaySetting.rankingSize; i++)
        {
            RankItem item = Instantiate(rankItemPrefab);
            item.ShowView(rankingData.rankDataList[i + GameManager.Instance.gamePlaySetting.rankingSize]);
            item.transform.SetParent(monthlyTabRoot);
            item.transform.localScale = Vector3.one;
        }
    }

    public void ChooseAllTab()
    {
        if (loaded)
            AudioManager.instance.btnSound.Play();

        allTab.alpha = 1.0f;
        allTab.interactable = true;
        allTab.blocksRaycasts = true;
        allTabSelectObj.SetActive(true);

        monthlyTab.alpha = 0.0f;
        monthlyTab.interactable = false;
        monthlyTab.blocksRaycasts = false;
        monthlyTabSelectObj.SetActive(false);

        loaded = true;
    }

    public void ChooseMonthlyTab()
    {
        AudioManager.instance.btnSound.Play();
        allTab.alpha = 0.0f;
        allTab.interactable = false;
        allTab.blocksRaycasts = false;
        allTabSelectObj.SetActive(false);

        monthlyTab.alpha = 1.0f;
        monthlyTab.interactable = true;
        monthlyTab.blocksRaycasts = true;
        monthlyTabSelectObj.SetActive(true);
    }
}
