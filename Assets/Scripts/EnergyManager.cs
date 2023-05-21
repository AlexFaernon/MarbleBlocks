using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    public static int _currentEnergy;
    public const int MaxEnergy = 5;
    private DateTime _lastEnergyRefill;

    public static int CurrentEnergy
    {
        get => _currentEnergy;
        set => _currentEnergy = value;
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private IEnumerator EnergyRefill()
    {
        
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            PlayerPrefs.SetInt(nameof(_currentEnergy), CurrentEnergy);
            PlayerPrefs.SetString("PauseTime", DateTime.Now.ToString(CultureInfo.InvariantCulture));
        }
        else
        {
            CurrentEnergy = PlayerPrefs.GetInt(nameof(_currentEnergy), -1);
            if (CurrentEnergy == -1)
            {
                CurrentEnergy = 0;
            }
            if (DateTime.TryParse(PlayerPrefs.GetString("PauseTime"), CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var lastPauseTime))
            {
                var now = DateTime.Now;
                var timePassed = now - lastPauseTime;
                Debug.Log($"last {lastPauseTime}, now {now}, passed {timePassed.TotalSeconds}");
            }
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(nameof(_currentEnergy), CurrentEnergy);
        PlayerPrefs.SetString("PauseTime", DateTime.Now.ToString(CultureInfo.InvariantCulture));
    }
}
