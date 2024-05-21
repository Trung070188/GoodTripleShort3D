using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "LevelData")]

public class LevelData : ScriptableObject
{
    public int ID;

    public int sceneID;

    public int time;

    public int totalProductCount;

    public int dynamicProductSet;

    public int singlePush;

    public int roundsEmptyPlaceCount;
}
