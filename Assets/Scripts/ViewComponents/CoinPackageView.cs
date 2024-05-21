using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinPackageView : MonoBehaviour
{
    public Text valueTxt;

    public Text priceTxt;

    public Image packageIcon;

    public Sprite[] iconList;

    [HideInInspector]
    public CoinPackageItem currentItem;

    public Config.IAPPackageID packageID;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowView(CoinPackageItem item)
    {
        currentItem = item;
        valueTxt.text = item.packageValue;
        priceTxt.text = item.packagePrice;
        packageIcon.sprite = iconList[item.packID];
        packageID = item.iapPackage;
    }

    public void Purchase()
    {
        Debug.Log("Buy Item " + currentItem.iapPackage + " price : " + currentItem.packagePrice);
        GameManager.Instance.uiManager.storeLayer.BuyIAPPackage(packageID);
    }
}
