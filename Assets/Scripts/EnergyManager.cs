using System;
using System.Globalization;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    public static int MaxEnergy => 4 + ExpLevelManager.PlayerLevel;
    private const int EnergyRefillTime = 60;
    private bool _energyIsRefiling;
    public static float TimeUntilRefill;
    private static int _currentEnergy;

    public static int CurrentEnergy
    {
        get => _currentEnergy;
        set => _currentEnergy = Math.Clamp(value, 0, MaxEnergy);
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
        if (NameManager.PlayerName.ToUpper() == "TRW") return;
        
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
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Debug.Log("pause");
            PlayerPrefs.SetInt(nameof(CurrentEnergy), CurrentEnergy);
            PlayerPrefs.SetString("PauseTime", DateTime.Now.ToString(CultureInfo.InvariantCulture));
            PlayerPrefs.SetFloat(nameof(TimeUntilRefill), TimeUntilRefill);
        }
        else
        {
            Debug.Log("unpause");
            CurrentEnergy   = PlayerPrefs.GetInt(nameof(CurrentEnergy), MaxEnergy);
            TimeUntilRefill = PlayerPrefs.GetFloat(nameof(TimeUntilRefill), 0);

            if (DateTime.TryParse(PlayerPrefs.GetString("PauseTime"), CultureInfo.InvariantCulture, DateTimeStyles.None, out var lastPauseTime))
            {
                var secondsPassed = (float)(DateTime.Now - lastPauseTime).TotalSeconds;
                Debug.Log($"last {lastPauseTime}, passed {secondsPassed} seconds");
                if (TimeUntilRefill > 0)
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
            if (TimeUntilRefill <= secondsPassed)
            {
                secondsPassed   -= TimeUntilRefill;
                TimeUntilRefill =  EnergyRefillTime;
                CurrentEnergy++;
            }
            else
            {
                TimeUntilRefill -= secondsPassed;
                secondsPassed   =  0;
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
    
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(nameof(CurrentEnergy), CurrentEnergy);
        PlayerPrefs.SetString("PauseTime", DateTime.Now.ToString(CultureInfo.InvariantCulture));
        PlayerPrefs.SetFloat(nameof(TimeUntilRefill), TimeUntilRefill);
    }
}
