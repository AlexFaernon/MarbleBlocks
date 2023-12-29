using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerWin : MonoBehaviour
{
	[SerializeField] private HelpSwitch helpSwitch;
	private void Awake()
	{
		DailyQuestsManager.MultiplayerLevelCompleted++;
		AchievementManager.MultiplayerLevelCompleted++;
		var helpReward = helpSwitch.HelpLevel < 3 ? 5 : 0;
		var stepsReward = StepCounter.Count <= LevelSaveManager.LoadedLevel.OptimalTurns ? 5 : 0;
		StartCoroutine(RealtimeDatabase.PushRank(AuthManager.User.DisplayName, 5 + helpReward + stepsReward));
		StartCoroutine(RealtimeDatabase.PushRank(RealtimeDatabase.Opponent, -1));
		StartCoroutine(RealtimeDatabase.IncreaseLevelCount());
		RealtimeDatabase.PushToHistory(RealtimeDatabase.Opponent, StepCounter.Count, true);
	}
}
