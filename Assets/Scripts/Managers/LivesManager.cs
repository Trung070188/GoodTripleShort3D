using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LivesManager : MonoBehaviour
{
    #region Constants
    /// <summary>
    /// Key to save the maximum number of lives for the player.
    /// </summary>
    private const string MAX_LIVES_SAVEKEY = "MaxLives";
    /// <summary>
    /// Key to save the number of currently available lives in the player preferences file.
    /// </summary>
    private const string LIVES_SAVEKEY = "Lives";
    /// <summary>
    /// Key to save the recovery start time in the player preferences file.
    /// </summary>
    private const string RECOVERY_TIME_SAVEKEY = "LivesRecoveryTime";
    /// <summary>
    /// Key to save the starting time of infinite lives.
    /// </summary>
    private const string INFINITE_LIVES_TIME_SAVEKEY = "InfiniteLivesStartTime";
    /// <summary>
    /// Key to save the total minutes of infinite lives given to the player.
    /// </summary>
    private const string INFINITE_LIVES_MINUTES_SAVEKEY = "InfiniteLivesMinutes";
    #endregion

    private double remainingSecondsWithInfiniteLives;

    public int MaxLives { get; private set; }

    public int DefaultMaxLives = 5;

    private DateTime infiniteLivesStartTime;

    private int infiniteLivesMinutes;

    [HideInInspector]
    public int lives;

    private DateTime recoveryStartTime;

    public bool HasInfiniteLives { get { return remainingSecondsWithInfiniteLives > 0D; } }

    private double secondsToNextLife;

    public UnityEvent OnRecoveryTimeChanged;

    public UnityEvent OnLivesChanged;

    private bool calculateSteps;

    public bool HasMaxLives { get { return (lives >= MaxLives); } }

    public double MinutesToRecover = 30D;

    public string LivesText { get { return HasInfiniteLives ? "∞" : lives.ToString(); } }

    public string CustomFullLivesText = "Full";

    public bool SimpleHourFormat = false;

    private bool applicationWasPaused;

    private void Awake()
    {
        //ResetPlayerPrefs();
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
        remainingSecondsWithInfiniteLives = 0D;
        MaxLives = PlayerPrefs.HasKey(MAX_LIVES_SAVEKEY) ? PlayerPrefs.GetInt(MAX_LIVES_SAVEKEY) : DefaultMaxLives;

        if (PlayerPrefs.HasKey(INFINITE_LIVES_TIME_SAVEKEY) && PlayerPrefs.HasKey(INFINITE_LIVES_MINUTES_SAVEKEY))
        {
            infiniteLivesStartTime = new DateTime(long.Parse(PlayerPrefs.GetString(INFINITE_LIVES_TIME_SAVEKEY)));
            infiniteLivesMinutes = PlayerPrefs.GetInt(INFINITE_LIVES_MINUTES_SAVEKEY);
        }
        else
        {
            infiniteLivesStartTime = DateTime.MinValue;
            infiniteLivesMinutes = 0;
        }

        if (PlayerPrefs.HasKey(LIVES_SAVEKEY) && PlayerPrefs.HasKey(RECOVERY_TIME_SAVEKEY))
        {
            lives = PlayerPrefs.GetInt(LIVES_SAVEKEY);
            recoveryStartTime = new DateTime(long.Parse(PlayerPrefs.GetString(RECOVERY_TIME_SAVEKEY)));
        }
        else
        {
            lives = MaxLives;
            recoveryStartTime = DateTime.Now;
        }

        if (lives > MaxLives)
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
        if (HasInfiniteLives)
        {
            remainingSecondsWithInfiniteLives -= Time.deltaTime;
            if (remainingSecondsWithInfiniteLives < 0D)
            {
                remainingSecondsWithInfiniteLives = 0D;
                infiniteLivesMinutes = 0;
                infiniteLivesStartTime = new DateTime(0);
                NotifyLivesChanged();
            }
            NotifyRecoveryTimeChanged();
        }
        else if (!HasMaxLives)
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
        remainingSecondsWithInfiniteLives = CalculateRemainingInfiniteLivesTime().TotalSeconds;
        if (!HasInfiniteLives)
        {
            secondsToNextLife = CalculateLifeRecovery().TotalSeconds;
        }
        calculateSteps = true;
        NotifyAll();
    }

    public bool ConsumeLife()
    {
        if (HasInfiniteLives)
        {
            return true;
        }
        bool result;
        if (lives > 0)
        {
            result = true;

            // If lifes where full, starts counting time for recovery.
            if (HasMaxLives)
            {
                recoveryStartTime = DateTime.Now;
                ResetSecondsToNextLife();
            }
            lives--;
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
        if (!HasMaxLives && !HasInfiniteLives)
        {
            lives++;
            recoveryStartTime = DateTime.Now;
            SavePlayerPrefs();
            NotifyAll();
        }
    }

    public void FillLives()
    {
        if (!HasInfiniteLives)
        {
            lives = MaxLives;
            SetSecondsToNextLifeToZero();
            NotifyAll();
        }
    }

    public void AddLifeSlots(int quantity, bool fillLives = false)
    {
        if (HasMaxLives && !HasInfiniteLives)
        {
            recoveryStartTime = DateTime.Now;
            ResetSecondsToNextLife();
        }
        MaxLives += quantity;
        if (fillLives)
        {
            FillLives();
        }
        else
        {
            SavePlayerPrefs();
        }
        InitTimer();
    }

    public void GiveInifinite(int minutes)
    {
        if (minutes <= 0)
        {
            return;
        }
        if (!HasInfiniteLives)
        {
            FillLives();
            infiniteLivesStartTime = DateTime.Now;
        }
        infiniteLivesMinutes += minutes;
        remainingSecondsWithInfiniteLives += minutes * 60;
        NotifyAll();
    }


    private void SavePlayerPrefs()
    {
        PlayerPrefs.SetInt(MAX_LIVES_SAVEKEY, MaxLives);
        PlayerPrefs.SetInt(LIVES_SAVEKEY, lives);
        PlayerPrefs.SetString(RECOVERY_TIME_SAVEKEY, recoveryStartTime.Ticks.ToString());
        PlayerPrefs.SetString(INFINITE_LIVES_TIME_SAVEKEY, infiniteLivesStartTime.Ticks.ToString());
        PlayerPrefs.SetInt(INFINITE_LIVES_MINUTES_SAVEKEY, infiniteLivesMinutes);
        try
        {
            PlayerPrefs.Save();
        }
        catch (Exception e)
        {
            Debug.LogError("Could not save LivesManager preferences: " + e.Message);
        }
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteKey(MAX_LIVES_SAVEKEY);
        PlayerPrefs.DeleteKey(RECOVERY_TIME_SAVEKEY);
        PlayerPrefs.DeleteKey(LIVES_SAVEKEY);
        PlayerPrefs.DeleteKey(INFINITE_LIVES_TIME_SAVEKEY);
        PlayerPrefs.DeleteKey(INFINITE_LIVES_MINUTES_SAVEKEY);
        RetrievePlayerPrefs();
    }

    private TimeSpan CalculateRemainingInfiniteLivesTime()
    {
        DateTime now = DateTime.Now;
        TimeSpan elapsed = now - infiniteLivesStartTime;
        double minutesElapsed = elapsed.TotalMinutes;

        if (minutesElapsed < (double)infiniteLivesMinutes)
        {
            return TimeSpan.FromMinutes(infiniteLivesMinutes - minutesElapsed);
        }
        else
        {
            return TimeSpan.Zero;
        }
    }

    private TimeSpan CalculateLifeRecovery()
    {
        DateTime now = DateTime.Now;
        TimeSpan elapsed = now - recoveryStartTime;
        double minutesElapsed = elapsed.TotalMinutes;

        while ((!HasMaxLives) && (minutesElapsed >= MinutesToRecover))
        {
            lives++;
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
            if (!HasInfiniteLives && HasMaxLives && !string.IsNullOrEmpty(CustomFullLivesText))
            {
                return CustomFullLivesText;
            }
            TimeSpan timerToShow = TimeSpan.FromSeconds(HasInfiniteLives ? remainingSecondsWithInfiniteLives : secondsToNextLife);
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
