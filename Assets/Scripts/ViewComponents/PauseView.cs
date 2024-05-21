using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseView : BaseView
{
    public GameObject musicOn, musicOff;

    public GameObject soundOn, soundOff;

    public GameObject vibrationOn, vibrationOff;

    public override void InitView()
    {
        if (AudioManager.instance.musicState == 0)
        {
            musicOn.SetActive(false);
            musicOff.SetActive(true);
        }
        else
        {
            musicOn.SetActive(true);
            musicOff.SetActive(false);
        }

        if (AudioManager.instance.soundState == 0)
        {
            soundOn.SetActive(false);
            soundOff.SetActive(true);
        }
        else
        {
            soundOn.SetActive(true);
            soundOff.SetActive(false);
        }

        if (AudioManager.instance.hapticState == 0)
        {
            vibrationOn.SetActive(false);
            vibrationOff.SetActive(true);
        }
        else
        {
            vibrationOn.SetActive(true);
            vibrationOff.SetActive(false);
        }
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
        InitView();
        GameManager.Instance.currentState = GameManager.GAME_STATE.IN_GAME_POPUP;
    }


    IEnumerator ResumeIE()
    {
        yield return new WaitForSeconds(1.0f);
        GameManager.Instance.currentState = GameManager.GAME_STATE.IN_GAME;
    }

    public void Continue()
    {
        AudioManager.instance.btnSound.Play();
        HideView();
        GameManager.Instance.currentState = GameManager.GAME_STATE.IN_GAME;
    }

    public void Replay()
    {
        AudioManager.instance.btnSound.Play();
        HideView();
        GameManager.Instance.ReplayLevel();
    }

    public void BackToHome()
    {
        AudioManager.instance.btnSound.Play();
        HideView();
        GameManager.Instance.BackToHome();
    }

    public void ToggleSound()
    {

        if (AudioManager.instance.soundState == 0)
        {
            soundOn.SetActive(true);
            soundOff.SetActive(false);
            AudioManager.instance.ToogleSound(true);
        }
        else
        {
            soundOn.SetActive(false);
            soundOff.SetActive(true);
            AudioManager.instance.ToogleSound(false);
        }

    }

    public void ToggleMusic()
    {

        if (AudioManager.instance.musicState == 0)
        {
            musicOn.SetActive(true);
            musicOff.SetActive(false);
            AudioManager.instance.ToogleMusic(true);
        }
        else
        {
            musicOn.SetActive(false);
            musicOff.SetActive(true);
            AudioManager.instance.ToogleMusic(false);
        }
    }

    public void ToggleHaptic()
    {

        if (AudioManager.instance.hapticState == 0)
        {
            vibrationOn.SetActive(true);
            vibrationOff.SetActive(false);
            AudioManager.instance.ToogleHaptic(true);
        }
        else
        {
            vibrationOn.SetActive(false);
            vibrationOff.SetActive(true);
            AudioManager.instance.ToogleHaptic(false);
        }
    }
}
