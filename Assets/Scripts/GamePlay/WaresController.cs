using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using HighlightPlus;

public class WaresController : MonoBehaviour
{
    public ShelfController shelfController;

    //public HighlightEffect highlightEffect;

    public int waresID;

    public int colID;

    public int deptID;

    public int ColID
    {
        get
        {
            return colID;
        }

        private set
        {
            colID = value;
        }
    }

    public int DeptID
    {
        get
        {
            return deptID;
        }

        private set
        {
            deptID = value;
        }
    }

    public Material[] material;

    public MeshRenderer meshRenderer;

    private Transform modelTrans;

    private int oldIndex;

    public enum STATE
    {
        IDLE,
        PROCESS
    }

    public STATE currentState;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitWares()
    {
        if (transform.childCount > 0)
        {
            modelTrans = transform.GetChild(0);
            material = modelTrans.GetComponent<MeshRenderer>().materials;
            meshRenderer = modelTrans.GetComponent<MeshRenderer>();
            //modelTrans.gameObject.AddComponent<HighlightEffect>();
            //highlightEffect = modelTrans.GetComponent<HighlightEffect>();
        }
        else
        {
            material = GetComponent<MeshRenderer>().materials;
            meshRenderer = GetComponent<MeshRenderer>();
            gameObject.AddComponent<HighlightEffect>();
            //highlightEffect = GetComponent<HighlightEffect>();
        }

        //highlightEffect.outlineColor = Color.red;
        //highlightEffect.outlineWidth = 0.15f;
        //highlightEffect.highlighted = false;
        currentState = STATE.IDLE;
    }

    public void SetColorHide()
    {
        //material.color = new Color(0.35f, 0.35f, 0.35f, 1.0f);
        for (int i = 0; i < material.Length; i++)
            material[i].SetColor("_Color", new Color(0.35f, 0.35f, 0.35f, 1.0f));
    }

    public void SetVisible(bool visible)
    {
        meshRenderer.enabled = visible;
    }

    public void SetColorShow()
    {
        // material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        for (int i = 0; i < material.Length; i++)
            material[i].SetColor("_Color", new Color(1.0f, 1.0f, 1.0f, 1.0f));
    }


    public void SelectWares()
    {
        //clear old slot
        oldIndex = shelfController.shelfSlotList[0].waresInRowSlot.IndexOf(this);
        shelfController.shelfSlotList[0].waresInRowSlot[oldIndex] = null;
        //highlightEffect.highlighted = true;
    }

    public void UnSelectWares()
    {
        //clear old slot
        shelfController.shelfSlotList[0].waresInRowSlot[oldIndex] = this;
        //highlightEffect.highlighted = false;
    }

    public void MoveBack()
    {
        //transform.localPosition = FindPosInShelf();
        transform.DOLocalMove(FindPosInShelf(), 0.2f).SetEase(Ease.Linear).SetDelay(0.0f).OnComplete(() =>
        {
            //highlightEffect.highlighted = false;

        });

    }

    private Vector3 FindPosInShelf()
    {
        Vector3 pos = Vector3.zero;
        pos = new Vector3((2 * colID + 1) * 0.5f * GameManager.Instance.gamePlaySetting.tileSizeX, 0.0f, -deptID * GameManager.Instance.gamePlaySetting.tileSizeZ);
        return pos;
    }

    public void MoveToAnotherShelf(ShelfController shelf, int emptySlot)
    {
        //bring to new shelf slot
        transform.SetParent(shelf.container.transform);
        shelf.shelfSlotList[0].waresInRowSlot[emptySlot] = this;
        //new coordinate
        colID = emptySlot;
        deptID = 0;
        //start to moving to new position
        shelfController = shelf;
        transform.DOMove(shelfController.shelfSlotList[0].warePointList[emptySlot].position, 0.1f);
        //highlightEffect.highlighted = false;
    }

    public void RemoveItem()
    {
        Destroy(gameObject);
    }
}
