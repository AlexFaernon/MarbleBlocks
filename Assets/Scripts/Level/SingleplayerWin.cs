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
    [SerializeField] private HelpSwitch helpSwitch;
    private void OnEnable()
    {
        StartCoroutine(playerLevel.GainExp(20));
        var helpReward = helpSwitch.HelpLevel < 3 ? 5 : 0;
        var stepsReward = StepCounter.Count <= LevelSaveManager.LoadedLevel.OptimalTurns ? 5 : 0;
        var reward = LevelSaveManager.LevelNumber > 3 ? 10 + helpReward + stepsReward : 10;
        CoinsManager.Coins += reward;
        coinsReward.text = reward.ToString();
        PlayerData.SingleLevelCompleted = Math.Max(PlayerData.SingleLevelCompleted, LevelSaveManager.LevelNumber);
    }
}
