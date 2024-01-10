using System;
using System.Globalization;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    public static int MaxEnergy => 4 + ExpLevelManager.PlayerLevel;
    private const int EnergyRefillTime = 300;
    private static bool _energyIsRefiling;
    public static float TimeUntilRefill;
    public static int CurrentEnergy
    {
        get => PlayerData.Energy;
        set
        {
            PlayerData.Energy = Math.Clamp(value, 0, MaxEnergy);
            RealtimeDatabase.PushEnergy(EnergyTimestamp);
        }
    }

    public static EnergyTimestamp EnergyTimestamp
    {
        get => new()
        {
            Energy = CurrentEnergy,
            PushTime = DateTime.Now,
            TimeUntilRefill = _energyIsRefiling ? TimeUntilRefill : EnergyRefillTime
        };
        set => RefillEnergySincePause(value);
    }

    private void Awake()
    {
        if (FindObjectsOfType<EnergyManager>().Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
    }
    
    public static void SpendEnergy()
    {
        if (PlayerData.Name.ToUpper() == "TRW") return;
        
        CurrentEnergy--;
    }

    private void Update()
    {
        if (CurrentEnergy < MaxEnergy)
        {
            if (_energyIsRefiling)
            {
                TimeUntilRefill -= Time.unscaledDeltaTime;
            }
            else
            {
                _energyIsRefiling = true;
                TimeUntilRefill  = EnergyRefillTime;
            }

            if (TimeUntilRefill <= 0)
            {
                CurrentEnergy++;
                _energyIsRefiling = false;
                TimeUntilRefill = 0;
            }
        }
        else
        {
            TimeUntilRefill = 0;
        }
    }
    // private void OnApplicationPause(bool pauseStatus)
    // {
    //     if (pauseStatus)
    //     {
    //         PlayerPrefs.SetInt(nameof(CurrentEnergy), CurrentEnergy);
    //         PlayerPrefs.SetString("PauseTime", DateTime.Now.ToString(CultureInfo.InvariantCulture));
    //         PlayerPrefs.SetFloat(nameof(TimeUntilRefill), TimeUntilRefill);
    //     }
    //     else
    //     {
    //         CurrentEnergy   = PlayerPrefs.GetInt(nameof(CurrentEnergy), MaxEnergy);
    //         TimeUntilRefill = PlayerPrefs.GetFloat(nameof(TimeUntilRefill), 0);
    //
    //         if (DateTime.TryParse(PlayerPrefs.GetString("PauseTime"), CultureInfo.InvariantCulture, DateTimeStyles.None, out var lastPauseTime))
    //         {
    //             var secondsPassed = (float)(DateTime.Now - lastPauseTime).TotalSeconds;
    //             Debug.Log($"last {lastPauseTime}, passed {secondsPassed} seconds");
    //             if (TimeUntilRefill > 0)
    //             {
    //                 RefillEnergySincePause(secondsPassed);
    //             }
    //         }
    //     }
    // }

    private static void RefillEnergySincePause(EnergyTimestamp energyTimestamp)
    {
        CurrentEnergy = energyTimestamp.Energy;
        TimeUntilRefill = energyTimestamp.TimeUntilRefill;
        var secondsPassed = (float)(DateTime.Now - energyTimestamp.PushTime).TotalSeconds;
        
        while (CurrentEnergy < MaxEnergy && secondsPassed > 0)
        {
            if (TimeUntilRefill <= secondsPassed)
            {
                secondsPassed -= TimeUntilRefill;
                TimeUntilRefill = EnergyRefillTime;
                CurrentEnergy++;
            }
            else
            {
                TimeUntilRefill -= secondsPassed;
                secondsPassed = 0;
            }
        }

        if (CurrentEnergy < MaxEnergy)
        {
            _energyIsRefiling = true;
        }
        else
        {
            TimeUntilRefill = 0;
        }
    }
    
    // private void OnApplicationQuit()
    // {
    //     PlayerPrefs.SetInt(nameof(CurrentEnergy), CurrentEnergy);
    //     PlayerPrefs.SetString("PauseTime", DateTime.Now.ToString(CultureInfo.InvariantCulture));
    //     PlayerPrefs.SetFloat(nameof(TimeUntilRefill), TimeUntilRefill);
    // }
}
