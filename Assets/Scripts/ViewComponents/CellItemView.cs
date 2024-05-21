using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellItemView : MonoBehaviour
{
    public GameObject lockObj;

    public GameObject unlockObj;

    public Image stampIcon;

    public List<GameObject> unlockStar;

    public List<GameObject> lockStar;

    public CellItem currentItem;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowView(CellItem item, int isUnlock, int currentPackage)
    {
        currentItem = item;

        if (isUnlock == 0)
        {
            lockObj.SetActive(true);
            unlockObj.SetActive(false);
            // Debug.Log("RARE " + item.rare);
            for (int i = 0; i < lockStar.Count; i++)
            {
                if (i < item.rare)
                    lockStar[i].SetActive(true);
                else
                    lockStar[i].SetActive(false);
            }
        }
        else
        {
            lockObj.SetActive(false);
            unlockObj.SetActive(true);
            for (int i = 0; i < unlockStar.Count; i++)
            {
                if (i < item.rare)
                    unlockStar[i].SetActive(true);
                else
                    unlockStar[i].SetActive(false);
            }

            stampIcon.sprite = Resources.Load<Sprite>("albums/" + (currentPackage + 1).ToString() + "/" + (item.cellID + 1).ToString());
        }
    }

}
