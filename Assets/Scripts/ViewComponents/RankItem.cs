using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankItem : MonoBehaviour
{
    public Text rankTxt;

    public Image rankMedal;

    public Text userNameTxt;

    public Text scoreTxt;

    public Sprite[] medalSpr;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowView(RankDataModel rankDataModel)
    {
        if (rankDataModel.rankID > GameManager.Instance.gamePlaySetting.rankingSize)
            rankDataModel.rankID -= GameManager.Instance.gamePlaySetting.rankingSize;

        rankTxt.text = rankDataModel.rankID.ToString();
        userNameTxt.text = rankDataModel.userName.ToString();
        scoreTxt.text = rankDataModel.score.ToString();

        if (rankDataModel.rankID == 1)
            rankMedal.sprite = medalSpr[0];
        else if (rankDataModel.rankID == 2)
            rankMedal.sprite = medalSpr[1];
        else if (rankDataModel.rankID == 3)
            rankMedal.sprite = medalSpr[2];
        else
            rankMedal.gameObject.SetActive(false);
    }
}
