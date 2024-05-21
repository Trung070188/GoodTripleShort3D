using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftPackageView : MonoBehaviour
{
    public Text packageName;

    public Image packageIcon;

    public Text packagePriceText;

    public List<GiftView> giftList;

    public Sprite[] itemSprList;

    public Sprite[] packageSprList;

    [HideInInspector]
    public GiftPackageItem currentItem;

    public Config.IAPPackageID packageID;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowView(GiftPackageItem item)
    {
        currentItem = item;
        packageName.text = item.packageName;
        packagePriceText.text = item.packagePrice;
        packageIcon.sprite = packageSprList[item.packID];
        packageID = item.iapPackage;

        for (int i = 0; i < giftList.Count; i++)
        {
            giftList[i].giftIcon.sprite = itemSprList[i];
            giftList[i].valueText.text = item.itemValueList[i].ToString();
        }
    }

    public void Purchase()
    {
        Debug.Log("Buy Item " + currentItem.iapPackage + " price : " + currentItem.packagePrice);
        GameManager.Instance.uiManager.storeLayer.BuyIAPPackage(packageID);
    }
}

[System.Serializable]
public class GiftView
{
    public Image giftIcon;

    public Text valueText;
}