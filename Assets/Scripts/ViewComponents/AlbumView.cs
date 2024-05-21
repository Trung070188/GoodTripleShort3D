using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlbumView : BaseView
{
    public List<CellItem> itemList;

    public List<CellItemView> itemViewList;

    public int currentPackage;

    public string packageName;

    public Text bookNameTxt;

    public GameObject completeObj;

    public override void InitView()
    {
        
    }

    public void InitView(int packageID, string name)
    {
        currentPackage = packageID;
        packageName = name;
    }

    public override void Start()
    {
       
    }

    public override void Update()
    {
        
    }

    public override void ShowView()
    {
        base.ShowView();

        bookNameTxt.text = packageName;
        for(int i = 0; i < itemList.Count; i++)
        {
            int isUnlock = PlayerPrefs.GetInt("Stamp" + currentPackage + "_" + itemList[i].cellID);
            itemViewList[i].ShowView(itemList[i] , isUnlock, currentPackage);
        }

        if (PlayerPrefs.GetInt("StampState" + currentPackage) == 2)
            completeObj.SetActive(true);
        
    }

    public void Refresh()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            int isUnlock = PlayerPrefs.GetInt("Stamp" + currentPackage + "_" + itemList[i].cellID);
            itemViewList[i].ShowView(itemList[i], isUnlock, currentPackage);
        }
    }

    public void UnlockItem(int packageIndex, int itemIndex)
    {
        AudioManager.instance.btnSound.Play();
        PlayerPrefs.SetInt("Stamp" + packageIndex + "_" + itemIndex, 1);
        Refresh();
        GameManager.Instance.uiManager.stampLayer.stampList[packageIndex].MoreUnlock();
    }
}

[System.Serializable]
public class CellItem
{
    [HideInInspector]
    public int packageID;

    public int cellID;

    public int rare;

    [HideInInspector]
    public int isUnlock;

}
