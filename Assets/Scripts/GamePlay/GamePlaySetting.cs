using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "GamePlaySetting")]
public class GamePlaySetting : ScriptableObject
{
    public int colSizeMax;

    public int deptSizeMax;

    public float tileSizeX;

    public float tileSizeY;

    public float tileSizeZ;

    public int openChestNumber;

    public int luckySpinProgressMax;

    public int rewardVictoryProgressMax;

    public int rankStep;

    public int rankRewardBonus;

    public int rankingSize;

    public List<RewardModel> rewardPackage1;

    public List<RewardModel> rewardPackage2;

    public List<RewardModel> rewardPackage3;

    public List<RewardModel> openChestReward;

    public List<RewardModel> unlockStampReward;
}
