using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DailyResetTimer : MonoBehaviour
{
    private TMP_Text _timerLabel;

    private void Awake()
    {
        _timerLabel = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        var timeLeft = DateTime.UtcNow.Date.AddDays(1) - DateTime.UtcNow;
        _timerLabel.text = $"{timeLeft.Hours} ч. {timeLeft.Minutes} мин.";
    }
}
