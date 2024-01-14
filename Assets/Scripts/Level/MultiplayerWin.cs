using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MultiplayerWin : MonoBehaviour
{
	[SerializeField] private HelpSwitch helpSwitch;
	[SerializeField] private TMP_Text coinsRewardLabel;
	[SerializeField] private RewardProgressBar progressBarExp;
	[SerializeField] private RewardProgressBar progressBarRank;
	private void Awake()
	{
		DailyQuestsManager.MultiplayerLevelCompleted++;
		AchievementManager.MultiplayerLevelCompleted++;
		var helpReward = helpSwitch.HelpLevel < 3 ? 5 : 0;
		var stepsReward = StepCounter.Count <= LevelSaveManager.LoadedLevel.OptimalTurns ? 5 : 0;
		var rankDelta = 5 + helpReward + stepsReward;
		var coinsReward = 10 + helpReward + stepsReward;
		CoinsManager.Coins += coinsReward;
		coinsRewardLabel.text = coinsReward.ToString();
		StartCoroutine(progressBarRank.GainRank(PlayerData.Rank, PlayerData.Rank + rankDelta));
		StartCoroutine(progressBarExp.GainExp(20));
		StartCoroutine(RealtimeDatabase.PushRank(AuthManager.User.DisplayName, rankDelta));
		StartCoroutine(RealtimeDatabase.PushRank(RealtimeDatabase.Opponent, -1));
		StartCoroutine(RealtimeDatabase.IncreaseLevelCount());
		PlayerData.LastOpponentName = null;
		RealtimeDatabase.PushToHistory(RealtimeDatabase.Opponent, StepCounter.Count, true);
	}
}
