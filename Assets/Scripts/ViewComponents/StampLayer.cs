using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampLayer : SliderNode
{
    public List<StampData> stampDataList = new List<StampData>();

    public List<CellStamp> stampList = new List<CellStamp>();

    public CellTable cellTablePrefab;

    public CellStamp cellStampPrefab;

    public RectTransform tableRoot;

    private bool isLoadItems;

    public override void InitView()
    {
        LoadStampData();

        if (!isLoadItems)
            LoadItem();
        else
            RefreshItems();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.U))
        {
            int randomIndex = Random.Range(0, stampDataList.Count);
            UnlockStamp(randomIndex);
        }
        
    }

    void LoadItem()
    {
        if (!isLoadItems)
            isLoadItems = true;

        CellTable table = null;

        for (int i = 0; i < stampDataList.Count; i++)
        {
            if (i% 2 == 0)
            {
                table = Instantiate(cellTablePrefab);
                table.transform.SetParent(tableRoot);
                table.transform.localScale = Vector3.one;
            }

            if(table != null)
            {
                CellStamp stamp = Instantiate(cellStampPrefab);
                stamp.transform.SetParent(table.stampRoot);
                stamp.transform.localScale = Vector3.one;
                stamp.ShowView(stampDataList[i]);
                stampList.Add(stamp);
            }
        }
    }

    private void RefreshItems()
    {
        for (int i = 0; i < stampList.Count; i++)
        {
            stampList[i].ShowView(stampDataList[i]);
        }

        for (int i = 0; i < stampDataList.Count; i++)
        {
            if (GameManager.Instance.currentLevel >= stampDataList[i].levelUnlock)
            {
                UnlockStamp(i);
            }
        }

    }

    void LoadStampData()
    {
        for (int i = 0; i < stampDataList.Count; i++)
        {
            stampDataList[i].stampState = PlayerPrefs.GetInt("StampState" + i.ToString());
            stampDataList[i].stampProgress = PlayerPrefs.GetInt("StampProgress"+ i.ToString());
            stampDataList[i].isStampClaim = PlayerPrefs.GetInt("IsStampClaim" + i.ToString());
        }
    }

    public void UnlockStamp(int packageID)
    {
        PlayerPrefs.SetInt("StampState" + packageID.ToString(), 1);
        stampDataList[packageID].stampState = 1;
        stampList[packageID].Unlock();
    }
}

[System.Serializable]
public class StampData
{
    public int stampID;

    public string stampName;

    public int levelUnlock;

    [HideInInspector]
    public int stampState;

    [HideInInspector]
    public int stampProgress;

    [HideInInspector]
    public int isStampClaim;

}
