using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifePackageView : MonoBehaviour
{
    public Text valueTxt;

    public Text priceTxt;

    [HideInInspector]
    public LifePackageItem currentItem;

    public Config.IAPPackageID packageID;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowView(LifePackageItem item)
    {
        currentItem = item;
        valueTxt.text = item.packageValue + " m";
        priceTxt.text = item.packagePrice;
        packageID = item.iapPackage;
    }

    public void Purchase()
    {
        Debug.Log("Buy Item " + currentItem.iapPackage + " price : " + currentItem.packagePrice);
        GameManager.Instance.uiManager.storeLayer.BuyIAPPackage(packageID);
    }
}
