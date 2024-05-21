using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RateView : BaseView
{
    public override void InitView()
    {

    }

    public override void Start()
    {

    }

    public override void Update()
    {

    }

    public void Rate()
    {
        AudioManager.instance.btnSound.Play();
        HideView();
    }

    public void No()
    {
        AudioManager.instance.btnSound.Play();
        HideView();
    }
}
