using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    private TMP_Text _label;
    private static int _coins = -1;
    public static int Coins
    {
        get => _coins;
        set
        {
            _coins = value;
            PlayerPrefs.SetInt("Coins", _coins);
        }
    }

    private void Awake()
    {
        _label = GetComponent<TMP_Text>();
        if (Coins != -1) return;
        
        if (PlayerPrefs.HasKey("Coins"))
        {
            _coins = PlayerPrefs.GetInt("Coins");
        }
        else
        {
            _coins = 0;
        }
    }

    private void Update()
    {
        _label.text = Coins.ToString();
    }
}
