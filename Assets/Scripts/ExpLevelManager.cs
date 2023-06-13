using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpLevelManager : MonoBehaviour
{
    public const int MaxExp = 1000;
    private static int _playerLevel = 1;
    public static int PlayerLevel
    {
        get => _playerLevel;
        private set
        {
            _playerLevel = value;
            EnergyManager.CurrentEnergy++;
            PlayerPrefs.SetInt("PlayerLevel", _playerLevel);
        }
    }
    
    private static int _exp;
    public static int Exp
    {
        get => _exp;
        set
        {
            _exp = value;
            if (_exp >= MaxExp)
            {
                PlayerLevel++;
                _exp %= MaxExp;
            }
            
            PlayerPrefs.SetInt("Exp", _exp);
        }
    }

    private void Awake()
    {
        if (PlayerPrefs.HasKey("PlayerLevel"))
        {
            _playerLevel = PlayerPrefs.GetInt("PlayerLevel");
        }
        
        if (PlayerPrefs.HasKey("Exp"))
        {
            _exp = PlayerPrefs.GetInt("Exp");
        }
    }
}
