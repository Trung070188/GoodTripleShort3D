using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankParser : MonoBehaviour
{
    public TextAsset levelCsvFile;

    string[][] dataArr = null;

    public List<RankDataModel> rankDataList;

    // Start is called before the first frame update
    void Start()
    {
        rankDataList = new List<RankDataModel>();
        ParseData();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CreateDataFile();
        }
    }

    void ParseData()
    {
        //i = row j = column
        dataArr = CsvParser2.Parse(levelCsvFile.text);

        for (int i = 0; i < dataArr.Length; i++)
        {
            if (i >= 4)
            {
                RankDataModel rk = new RankDataModel();
                for (int j = 0; j < dataArr[i].Length; j++)
                {
                    // Debug.Log(dataArr[i][j]);
                    if (j == 0)
                        rk.rankID = int.Parse(dataArr[i][0]);
                    else if (j == 1)
                        rk.userName = dataArr[i][1];
                    else if (j == 2)
                        rk.score = int.Parse(dataArr[i][2]);

                }

                rankDataList.Add(rk);
            }


        }
    }

    public void CreateDataFile()
    {

#if UNITY_EDITOR
        RankData rankSO = ScriptableObject.CreateInstance<RankData>();

        for(int i = 0; i < rankDataList.Count; i++)
        {
            RankDataModel rankModel = new RankDataModel();
            rankModel.rankID = rankDataList[i].rankID;
            rankModel.userName = rankDataList[i].userName;
            rankModel.score = rankDataList[i].score;
            rankSO.rankDataList.Add(rankModel);
        }

       
        UnityEditor.AssetDatabase.CreateAsset(rankSO, "Assets/Resources/Rank/" + "AIRank.asset");
#endif

    }
}
