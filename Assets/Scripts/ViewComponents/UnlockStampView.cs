using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockStampView : BaseView
{
    public Image icon;

    public override void InitView()
    {
        
    }

    public void InitView(int packageID, int stampID)
    {
        GameManager.Instance.currentState = GameManager.GAME_STATE.IN_GAME_POPUP;
        icon.sprite = Resources.Load<Sprite>("albums/" + (packageID + 1).ToString() + "/" + (stampID + 1).ToString());
        icon.SetNativeSize();
    }

    public override void Start()
    {
       
    }

    public override void Update()
    {
       
    }

    public override void ShowView()
    {
        base.ShowView();
        AudioManager.instance.unlockMission.Play();
    }

    public override void HideView()
    {
        AudioManager.instance.btnSound.Play();
        base.HideView();
        GameManager.Instance.currentState = GameManager.GAME_STATE.IN_GAME;
    }

}
