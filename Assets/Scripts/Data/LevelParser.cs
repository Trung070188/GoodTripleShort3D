using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;

public class LevelParser : MonoBehaviour
{
    public TextAsset levelCsvFile;

    string[][] dataArr = null;

    public List<LevelDataModel> levelDataList;

    // Start is called before the first frame update
    void Start()
    {
        levelDataList = new List<LevelDataModel>();
        ParseData();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            for(int i = 100; i < 200; i++)
                CreateLevelFile(i);
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
                LevelDataModel lv = new LevelDataModel();
                for (int j = 0; j < dataArr[i].Length; j++)
                {
                    // Debug.Log(dataArr[i][j]);
                    if (j == 0)
                        lv.ID = int.Parse(dataArr[i][0]);
                    else if (j == 1)
                        lv.sceneID = int.Parse(dataArr[i][1]);
                    else if (j == 3)
                        lv.time = int.Parse(dataArr[i][3]);
                    else if (j == 6)
                        lv.totalProductCount = int.Parse(dataArr[i][6]);                   
                    else if (j == 8)
                    {
                        if(dataArr[i][8] != "")
                        lv.dynamicProductSet = int.Parse(dataArr[i][8]);
                    }
                        
                    else if (j == 12)
                        lv.singlePush= int.Parse(dataArr[i][12]);
                    else if (j == 13)
                    {
                        string[] prefix = dataArr[i][13].Split('|');
                        if (prefix[0] != "")
                            lv.roundsEmptyPlaceCount = int.Parse(prefix[0]);
                    }
                        
                    

                  
                }

                levelDataList.Add(lv);
            }


        }
    }

    public void CreateLevelFile(int lvIndex)
    {
#if UNITY_EDITOR
        LevelData levelSO = ScriptableObject.CreateInstance<LevelData>();
        levelSO.ID = levelDataList[lvIndex].ID;
        levelSO.sceneID = levelDataList[lvIndex].sceneID;
        levelSO.time = levelDataList[lvIndex].time;
        levelSO.totalProductCount = levelDataList[lvIndex].totalProductCount;
        levelSO.dynamicProductSet = levelDataList[lvIndex].dynamicProductSet;
        levelSO.roundsEmptyPlaceCount = levelDataList[lvIndex].roundsEmptyPlaceCount;
        levelSO.singlePush = levelDataList[lvIndex].singlePush;
        UnityEditor.AssetDatabase.CreateAsset(levelSO, "Assets/Resources/GameLevels/" + "Level" + (lvIndex + 1).ToString() + ".asset");
#endif
    }
}

[System.Serializable]
public class LevelDataModel
{
    public int ID;

    public int sceneID;

    public int time;

    public int totalProductCount;

    public int dynamicProductSet;

    public int singlePush;

    public int roundsEmptyPlaceCount;
}
