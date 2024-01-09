using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class SingleplayerWin : MonoBehaviour
{
    [SerializeField] private TMP_Text coinsReward;
    [SerializeField] private RewardProgressBar playerLevel;
    private static Random _random = new();
    private void Start()
    {
        StartCoroutine(playerLevel.GainExp(20));
        var coinsAdded = _random.Next(1, 3);
        CoinsManager.Coins += coinsAdded;
        coinsReward.text = coinsAdded.ToString();
        PlayerData.SingleLevelCompleted = Math.Max(PlayerData.SingleLevelCompleted, LevelSaveManager.LevelNumber);
    }
}
