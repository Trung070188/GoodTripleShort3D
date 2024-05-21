using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SliderNode : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    public RectTransform nodeTransform;

    public abstract void InitView();

    public virtual void ShowView()
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public virtual void HideView()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
