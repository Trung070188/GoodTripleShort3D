using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingView : BaseView
{
    public GameObject musicOn, musicOff;

    public GameObject soundOn, soundOff;

    public GameObject vibrationOn, vibrationOff;

    public string androidGameUrl;

    public string iosGameUrl;

    public string termUrl;

    public string privacyUrl;

    public string contactUrl;

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
    }

    public override void HideView()
    {
        base.HideView();
        AudioManager.instance.btnSound.Play();
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

    public void ContactUs()
    {
        AudioManager.instance.btnSound.Play();
        Application.OpenURL(contactUrl);
    }

    public void Rate()
    {
        AudioManager.instance.btnSound.Play();
#if UNITY_IOS
        Application.OpenURL(iosGameUrl);
#elif UNITY_ANDROID
        Application.OpenURL(androidGameUrl);
#endif
        
    }

    public void Term()
    {
        AudioManager.instance.btnSound.Play();
        Application.OpenURL(termUrl);
    }

    public void Policy()
    {
        AudioManager.instance.btnSound.Play();
        Application.OpenURL(privacyUrl);
    }


    public void Restore()
    {
        AudioManager.instance.btnSound.Play();
        IAPManager.instance.RestorePurchases();
       // Debug.Log("Restore");
    }

    public void QuitGame()
    {
        AudioManager.instance.btnSound.Play();
        Application.Quit();
    }
}
