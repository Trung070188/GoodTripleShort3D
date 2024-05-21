using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FreeAdsTimer : MonoBehaviour
{
    private const string MAX_FREE_ADS_SAVEKEY = "MaxFreeAds";
    /// <summary>
    /// Key to save the number of currently available lives in the player preferences file.
    /// </summary>
    private const string FREE_ADS_SAVEKEY = "FreeAdsNumber";
    /// <summary>
    /// Key to save the recovery start time in the player preferences file.
    /// </summary>
    private const string RECOVERY_TIME_SAVEKEY = "FreeAdsRecoveryTime";

    public int MaxFreeAds { get; private set; }

    public int DefaultFreeAds = 10;

    [HideInInspector]
    public int freeAdsNumber;

    private DateTime recoveryStartTime;

    private double secondsToNextLife;

    public UnityEvent OnRecoveryTimeChanged;

    public UnityEvent OnLivesChanged;

    private bool calculateSteps;

    public bool HasMaxLives { get { return (freeAdsNumber >= MaxFreeAds); } }

    public double MinutesToRecover = 30D;

    public string LivesText { get { return freeAdsNumber.ToString(); } }

    public string CustomFullLivesText = "Full";

    public bool SimpleHourFormat = false;

    private bool applicationWasPaused;

    private void Awake()
    {
        RetrievePlayerPrefs();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (calculateSteps)
        {
            StepRecoveryTime();
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            applicationWasPaused = true;
            calculateSteps = false;
        }
        else if (applicationWasPaused)
        {
            applicationWasPaused = false;
            InitTimer();
        }
    }

    private void RetrievePlayerPrefs()
    {
        MaxFreeAds = PlayerPrefs.HasKey(MAX_FREE_ADS_SAVEKEY) ? PlayerPrefs.GetInt(MAX_FREE_ADS_SAVEKEY) : DefaultFreeAds;

        if (PlayerPrefs.HasKey(FREE_ADS_SAVEKEY) && PlayerPrefs.HasKey(RECOVERY_TIME_SAVEKEY))
        {
            freeAdsNumber = PlayerPrefs.GetInt(FREE_ADS_SAVEKEY);
            recoveryStartTime = new DateTime(long.Parse(PlayerPrefs.GetString(RECOVERY_TIME_SAVEKEY)));
        }
        else
        {
            freeAdsNumber = MaxFreeAds;
            recoveryStartTime = DateTime.Now;
        }

        if (freeAdsNumber > MaxFreeAds)
        {
            FillLives();
        }
    }

    private void SetSecondsToNextLifeToZero()
    {
        secondsToNextLife = 0;
        NotifyRecoveryTimeChanged();
    }

    private void NotifyAll()
    {
        NotifyRecoveryTimeChanged();
        NotifyLivesChanged();
    }

    private void NotifyRecoveryTimeChanged()
    {
        OnRecoveryTimeChanged.Invoke();
    }

    private void NotifyLivesChanged()
    {
        OnLivesChanged.Invoke();
    }

    private void StepRecoveryTime()
    {

        if (!HasMaxLives)
        {
            if (secondsToNextLife > 0D)
            {
                secondsToNextLife -= Time.deltaTime;
                NotifyRecoveryTimeChanged();
            }
            else
            {
                GiveOneLife();
                NotifyLivesChanged();
                if (HasMaxLives)
                {
                    SetSecondsToNextLifeToZero();
                }
                else
                {
                    ResetSecondsToNextLife();
                }
            }
        }
    }

    private void ResetSecondsToNextLife()
    {
        secondsToNextLife = MinutesToRecover * 60;
        NotifyRecoveryTimeChanged();
    }

    private void InitTimer()
    {

        secondsToNextLife = CalculateLifeRecovery().TotalSeconds;
        calculateSteps = true;
        NotifyAll();
    }

    public bool ConsumeLife()
    {
        bool result;
        if (freeAdsNumber > 0)
        {
            result = true;

            // If lifes where full, starts counting time for recovery.
            if (HasMaxLives)
            {
                recoveryStartTime = DateTime.Now;
                ResetSecondsToNextLife();
            }
            freeAdsNumber--;
            NotifyLivesChanged();
            SavePlayerPrefs();

        }
        else
        {
            result = false;
        }
        return result;
    }

    public void GiveOneLife()
    {
        if (!HasMaxLives)
        {
            freeAdsNumber++;
            recoveryStartTime = DateTime.Now;
            SavePlayerPrefs();
            NotifyAll();
        }
    }

    public void FillLives()
    {
       
            freeAdsNumber = MaxFreeAds;
            SetSecondsToNextLifeToZero();
            NotifyAll();
        
    }


    private void SavePlayerPrefs()
    {
        PlayerPrefs.SetInt(MAX_FREE_ADS_SAVEKEY, MaxFreeAds);
        PlayerPrefs.SetInt(FREE_ADS_SAVEKEY, freeAdsNumber);
        PlayerPrefs.SetString(RECOVERY_TIME_SAVEKEY, recoveryStartTime.Ticks.ToString());
        try
        {
            PlayerPrefs.Save();
        }
        catch (Exception e)
        {
            Debug.LogError("Could not save LivesManager preferences: " + e.Message);
        }
    }


    private TimeSpan CalculateLifeRecovery()
    {
        DateTime now = DateTime.Now;
        TimeSpan elapsed = now - recoveryStartTime;
        double minutesElapsed = elapsed.TotalMinutes;

        while ((!HasMaxLives) && (minutesElapsed >= MinutesToRecover))
        {
            freeAdsNumber++;
            recoveryStartTime = DateTime.Now;
            minutesElapsed -= MinutesToRecover;
        }

        SavePlayerPrefs();

        if (HasMaxLives)
        {
            return TimeSpan.Zero;
        }
        else
        {
            return TimeSpan.FromMinutes(MinutesToRecover - minutesElapsed);
        }
    }

    public string RemainingTimeString
    {
        get
        {
            if (HasMaxLives && !string.IsNullOrEmpty(CustomFullLivesText))
            {
                return CustomFullLivesText;
            }
            TimeSpan timerToShow = TimeSpan.FromSeconds(secondsToNextLife);
            if (timerToShow.TotalHours > 1D)
            {
                if (SimpleHourFormat)
                {
                    int hoursLeft = Mathf.RoundToInt((float)timerToShow.TotalHours);
                    return string.Format(">{0} hr{1}", hoursLeft, hoursLeft > 1 ? string.Empty : "");
                }
                return timerToShow.ToString().Substring(0, 8);
            }
            return timerToShow.ToString().Substring(3, 5);
        }
    }

    private void OnDestroy()
    {
        SavePlayerPrefs();
    }

}
