using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DailyQuestsManager : MonoBehaviour
{
	[SerializeField] private QuestAchivement play1;
	[SerializeField] private QuestAchivement play2;
	[SerializeField] private QuestAchivement useHelp;
	public static int DailyQuestRemained => Convert.ToInt32(!_claimed1) + Convert.ToInt32(!_claimed2) + Convert.ToInt32(!_claimed3);
	private static int _multiplayerLevelCompleted;
	private static int _helpUsed;
	private static bool _claimed1;
	private static bool _claimed2;
	private static bool _claimed3;

	public static int MultiplayerLevelCompleted
	{
		get => _multiplayerLevelCompleted;
		set
		{
			_multiplayerLevelCompleted = value;
			RealtimeDatabase.PushUserData();
		}
	}

	public static int HelpUsed
	{
		get => _helpUsed;
		set
		{
			_helpUsed = value;
			RealtimeDatabase.PushUserData();
		}
	}

	public static Dictionary<string, Tuple<int, bool>> QuestDict
	{
		get => new()
		{
			{nameof(play1), new Tuple<int, bool>(MultiplayerLevelCompleted, _claimed1)},
			{nameof(play2), new Tuple<int, bool>(MultiplayerLevelCompleted, _claimed2)},
			{nameof(useHelp), new Tuple<int, bool>(HelpUsed, _claimed3)}
		};
		set
		{
			(_multiplayerLevelCompleted, _claimed1) = value[nameof(play1)];
			(_multiplayerLevelCompleted, _claimed2) = value[nameof(play2)];
			(_helpUsed, _claimed3) = value[nameof(useHelp)];
		}
	}

	public static void CheckResetDailyQuest()
	{
		var currentLogin = DateTimeOffset.FromUnixTimeMilliseconds((long)AuthManager.User.Metadata.LastSignInTimestamp);
		if (currentLogin.Day <= PlayerData.LastLogin.Day && currentLogin.Month <= PlayerData.LastLogin.Month && currentLogin.Year <= PlayerData.LastLogin.Year) return;

		_claimed1 = false;
		_claimed2 = false;
		_claimed3 = false;
		MultiplayerLevelCompleted = 0;
		HelpUsed = 0;
	}
	
	private void Awake()
	{
		play1.progress = MultiplayerLevelCompleted;
		play2.progress = MultiplayerLevelCompleted;
		useHelp.progress = HelpUsed;
		
		play1.claimButton.onClick.AddListener(() => ClaimReward(ref _claimed1));
		play2.claimButton.onClick.AddListener(() => ClaimReward(ref _claimed2));
		useHelp.claimButton.onClick.AddListener(() => ClaimReward(ref _claimed3));
	}

	private void ClaimReward(ref bool claimField)
	{
		claimField = true;
		RealtimeDatabase.PushUserData();
	}

	private void Update()
	{
		play1.claimButton.interactable = !_claimed1;
		play2.claimButton.interactable = !_claimed2;
		useHelp.claimButton.interactable = !_claimed3;
	}
}
