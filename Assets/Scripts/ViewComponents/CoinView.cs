using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CoinView : BaseView
{
    public RectTransform coinVfx1;

    public RectTransform coinVfx2;

    public RectTransform coinVfx3;

    public RectTransform targetTrs;

    public Text coinTxt;

    private Vector3 localPos1;

    private Vector3 localPos2;

    private Vector3 localPos3;

    public override void InitView()
    {
        coinTxt.text = GameManager.Instance.currentCoin.ToString();
    }

    public override void Start()
    {
        localPos1 = coinVfx1.position;
        localPos2 = coinVfx2.position;
        localPos3 = coinVfx3.position;

        
    }

    public override void Update()
    {
       if(Input.GetKeyDown(KeyCode.Space))
            UpdateCoinPanelVfx();
    }

    public void UpdateCoinPanel()
    {
        coinTxt.text = GameManager.Instance.currentCoin.ToString();
    }

    public void UpdateCoinPanelVfx()
    {
        AudioManager.instance.moreCoinSound.Play();
        coinVfx1.gameObject.SetActive(true);
        coinVfx1.DOMove(targetTrs.position, 0.5f).OnComplete(
            () =>
            {
                coinVfx1.gameObject.SetActive(false);
                coinVfx1.position = localPos1;
            }
            );

        coinVfx2.gameObject.SetActive(true);
        coinVfx2.DOMove(targetTrs.position, 0.5f).SetDelay(0.25f).OnComplete(
            () =>
            {
                coinVfx2.gameObject.SetActive(false);
                coinVfx2.position = localPos2;
            }
            );

        coinVfx3.gameObject.SetActive(true);
        coinVfx3.DOMove(targetTrs.position, 0.5f).SetDelay(0.5f).OnComplete
            (
            ()=>
            {
                coinVfx3.gameObject.SetActive(false);
                coinVfx3.position = localPos3;
                coinTxt.text = GameManager.Instance.currentCoin.ToString();
            }
            );
    }

}
