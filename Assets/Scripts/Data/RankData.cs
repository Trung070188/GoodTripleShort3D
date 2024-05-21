using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "RankData")]

public class RankData : ScriptableObject
{
    public List<RankDataModel> rankDataList = new List<RankDataModel>();
}

[System.Serializable]

public class RankDataModel
{
    public int rankID;

    public string userName;

    public int score;
}
