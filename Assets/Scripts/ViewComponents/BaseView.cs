using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class BaseView : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    [HideInInspector]
    public bool isShow;

    // Start is called before the first frame update
    public abstract void Start();


    // Update is called once per frame
    public abstract void Update();

    public abstract void InitView();

    public virtual void ShowView()
    {
        canvasGroup.alpha = 0.0f;
        

        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 1.0f, 0.1f).SetEase(Ease.Linear)
            .OnComplete(() => {

                canvasGroup.alpha = 1.0f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                isShow = true;
            });

    }

    public virtual void ShowView(string content)
    {
        ShowView();
    }

    public virtual void HideView()
    {

        AudioManager.instance.btnSound.Play();
        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 0.0f, 0.1f).SetEase(Ease.Linear)
           .OnComplete(() => {

               canvasGroup.alpha = 0.0f;
               canvasGroup.interactable = false;
               canvasGroup.blocksRaycasts = false;
               isShow = false;
           });
    }

}
