using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ProductSet")]
public class ProductSet : ScriptableObject
{
    public List<ProductItem> productSetList = new List<ProductItem>(); 
}

[System.Serializable]

public class ProductItem
{
    public int ID;

    public List<int> productList;
}