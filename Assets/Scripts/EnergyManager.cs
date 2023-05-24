using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class EnergyManager : MonoBehaviour
{
    [SerializeField] private TMP_Text energy;
    [SerializeField] private TMP_Text timer;
    public const int MaxEnergy = 5;
    private const int EnergyRefillTime = 300;
    private bool _energyIsRefiling;
    private float _timeUntilRefill;

    public static int CurrentEnergy { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void SpendEnergy()
    {
        CurrentEnergy--;
    }

    private void Update()
    {
        energy.text = $"{CurrentEnergy}/{MaxEnergy}";
        timer.text  = $"{(int)_timeUntilRefill / 60}:{(int)_timeUntilRefill % 60}";
        if (CurrentEnergy < MaxEnergy)
        {
            if (_energyIsRefiling)
            {
                _timeUntilRefill -= Time.unscaledDeltaTime;
            }
            else
            {
                _energyIsRefiling = true;
                _timeUntilRefill  = EnergyRefillTime;
            }

            if (_timeUntilRefill <= 0)
            {
                CurrentEnergy++;
                _energyIsRefiling = false;
                _timeUntilRefill = 0;
            }
        }
    }
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            PlayerPrefs.SetInt(nameof(CurrentEnergy), CurrentEnergy);
            PlayerPrefs.SetString("PauseTime", DateTime.Now.ToString(CultureInfo.InvariantCulture));
            PlayerPrefs.SetFloat(nameof(_timeUntilRefill), _timeUntilRefill);
        }
        else
        {
            CurrentEnergy   = PlayerPrefs.GetInt(nameof(CurrentEnergy), MaxEnergy);
            _timeUntilRefill = PlayerPrefs.GetFloat(nameof(_timeUntilRefill), 0);

            if (DateTime.TryParse(PlayerPrefs.GetString("PauseTime"), CultureInfo.InvariantCulture, DateTimeStyles.None, out var lastPauseTime))
            {
                var secondsPassed = (float)(DateTime.Now - lastPauseTime).TotalSeconds;
                Debug.Log($"last {lastPauseTime}, passed {secondsPassed} seconds");
                if (_timeUntilRefill > 0)
                {
                    RefillEnergySincePause(secondsPassed);
                }
            }
        }
    }

    private void RefillEnergySincePause(float secondsPassed)
    {
        while (CurrentEnergy < MaxEnergy && secondsPassed > 0)
        {
            if (_timeUntilRefill <= secondsPassed)
            {
                secondsPassed   -= _timeUntilRefill;
                _timeUntilRefill =  EnergyRefillTime;
                CurrentEnergy++;
            }
            else
            {
                _timeUntilRefill -= secondsPassed;
                secondsPassed   =  0;
            }
        }

        if (CurrentEnergy < MaxEnergy)
        {
            _energyIsRefiling = true;
        }
        else
        {
            _timeUntilRefill = 0;
        }
    }
    
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(nameof(CurrentEnergy), CurrentEnergy);
        PlayerPrefs.SetString("PauseTime", DateTime.Now.ToString(CultureInfo.InvariantCulture));
        PlayerPrefs.SetFloat(nameof(_timeUntilRefill), _timeUntilRefill);
    }
}
