using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class SliderView : MonoBehaviour
{
    public List<SliderNode> nodeList;

    public int midleNodeIndex;

    [HideInInspector]
    public int currentNodeIndex;

    [HideInInspector]
    public float canvasWidth;

    public RectTransform arrow;

    public enum MovingState
    {
        IDLE,
        MOVING
    };

    public MovingState currentState;

    public float witdhScreenSize;

    public Canvas canvas;

    public void InitView()
    {
        witdhScreenSize = canvas.GetComponent<RectTransform>().sizeDelta.x;
        midleNodeIndex = (nodeList.Count - 1) / 2;

        for (int i = 0; i < nodeList.Count; i++)
        {
            nodeList[i].InitView();
            nodeList[i].ShowView();
            nodeList[i].nodeTransform.offsetMin = new Vector3((i - midleNodeIndex) * witdhScreenSize, nodeList[i].nodeTransform.offsetMin.y);
            nodeList[i].nodeTransform.offsetMax = new Vector2((i - midleNodeIndex) * witdhScreenSize, nodeList[i].nodeTransform.offsetMax.y);
        }

        currentState = MovingState.IDLE;
    }

    public void ShowMidleNode()
    {
        currentNodeIndex = midleNodeIndex;
        nodeList[midleNodeIndex].ShowView();
    }

    public void ShowNodeByIndex(int index)
    {
        if (currentState == MovingState.MOVING)
            return;

        Debug.Log("Show Node " + index);
        AudioManager.instance.btnSound.Play();
        float distanceTranslate = (currentNodeIndex - index) * witdhScreenSize;

        if(currentNodeIndex != index)
        {
            currentState = MovingState.MOVING;
            for (int i = 0; i < nodeList.Count; i++)
            {
                MoveNodeByDistance(i, distanceTranslate, index);
            }
        }

        arrow.DOLocalMoveX(-(currentNodeIndex - index) * 145.0f, 0.35f).SetRelative(true).SetEase(Ease.InQuad);
    
    }

    private void MoveNodeByDistance(int nodeIndex, float moveDistance, int lastNodeIndex)
    {
        

        Vector2 currentNodeMin = Vector3.zero;

        DOTween.To(() => new Vector3((nodeIndex - currentNodeIndex) * witdhScreenSize, nodeList[nodeIndex].nodeTransform.offsetMin.y), x => currentNodeMin = x,
            new Vector3((nodeIndex - currentNodeIndex) * witdhScreenSize + moveDistance, nodeList[nodeIndex].nodeTransform.offsetMin.y), 0.35f).SetEase(Ease.InQuad)
              .OnUpdate(() =>
              {
                  nodeList[nodeIndex].nodeTransform.offsetMin = currentNodeMin;

              })
              .OnComplete(() =>
              {

              });

        Vector2 currentNodeMax = Vector3.zero;

        DOTween.To(() => new Vector3((nodeIndex - currentNodeIndex) * witdhScreenSize, nodeList[nodeIndex].nodeTransform.offsetMax.y), x => currentNodeMax = x,
            new Vector3((nodeIndex - currentNodeIndex) * witdhScreenSize + moveDistance, nodeList[nodeIndex].nodeTransform.offsetMax.y), 0.35f).SetEase(Ease.InQuad)
              .OnUpdate(() =>
              {
                  nodeList[nodeIndex].nodeTransform.offsetMax = currentNodeMax;

              })
              .OnComplete(() =>
              {
                  if(nodeIndex == (nodeList.Count - 1))
                  {
                      currentNodeIndex = lastNodeIndex;
                      currentState = MovingState.IDLE;
                  }
                 
              });

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
    public void SetLeft(this RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public void SetRight(this RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public void SetTop(this RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public void SetBottom(this RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }
    */
}
