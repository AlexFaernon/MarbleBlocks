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
    private bool energyIsRefiling;
    private float timeUntilRefill;

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
        timer.text  = $"{(int)timeUntilRefill / 60}:{(int)timeUntilRefill % 60}";
        if (CurrentEnergy < MaxEnergy)
        {
            if (energyIsRefiling)
            {
                timeUntilRefill -= Time.unscaledDeltaTime;
            }
            else
            {
                energyIsRefiling = true;
                timeUntilRefill  = EnergyRefillTime;
            }

            if (timeUntilRefill <= 0)
            {
                CurrentEnergy++;
                energyIsRefiling = false;
                timeUntilRefill = 0;
            }
        }
    }
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            PlayerPrefs.SetInt(nameof(CurrentEnergy), CurrentEnergy);
            PlayerPrefs.SetString("PauseTime", DateTime.Now.ToString(CultureInfo.InvariantCulture));
            PlayerPrefs.SetFloat(nameof(timeUntilRefill), timeUntilRefill);
        }
        else
        {
            CurrentEnergy   = PlayerPrefs.GetInt(nameof(CurrentEnergy), MaxEnergy);
            timeUntilRefill = PlayerPrefs.GetFloat(nameof(timeUntilRefill), 0);

            if (DateTime.TryParse(PlayerPrefs.GetString("PauseTime"), CultureInfo.InvariantCulture, DateTimeStyles.None, out var lastPauseTime))
            {
                var secondsPassed = (float)(DateTime.Now - lastPauseTime).TotalSeconds;
                Debug.Log($"last {lastPauseTime}, passed {secondsPassed} seconds");
                if (timeUntilRefill > 0)
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
            if (timeUntilRefill <= secondsPassed)
            {
                secondsPassed   -= timeUntilRefill;
                timeUntilRefill =  EnergyRefillTime;
                CurrentEnergy++;
            }
            else
            {
                timeUntilRefill -= secondsPassed;
                secondsPassed   =  0;
            }
        }

        if (CurrentEnergy < MaxEnergy)
        {
            energyIsRefiling = true;
        }
        else
        {
            timeUntilRefill = 0;
        }
    }
    
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(nameof(CurrentEnergy), CurrentEnergy);
        PlayerPrefs.SetString("PauseTime", DateTime.Now.ToString(CultureInfo.InvariantCulture));
        PlayerPrefs.SetFloat(nameof(timeUntilRefill), timeUntilRefill);
    }
}
