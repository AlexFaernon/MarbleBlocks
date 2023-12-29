using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
	[SerializeField] private QuestAchivement createFirst;
	[SerializeField] private QuestAchivement complete3Rating;
	[SerializeField] private QuestAchivement complete3Story;
	private static int _multiplayerLevelCompleted;
	private static int _levelsCreated;
	private static bool _3RatingClaimed;
	private static bool _3StoryClaimed;
	private static bool _createFirstClaimed;

	public static int LevelsCreated
	{
		get => _levelsCreated;
		set
		{
			_levelsCreated = value;
			RealtimeDatabase.PushUserData();
		}
	}

	public static int MultiplayerLevelCompleted
	{
		get => _multiplayerLevelCompleted;
		set
		{
			_multiplayerLevelCompleted = value;
			RealtimeDatabase.PushUserData();
		}
	}

	public static Dictionary<string, Tuple<int, bool>> AchievementDict
	{
		get => new()
		{
			{nameof(createFirst), new Tuple<int, bool>(LevelsCreated, _createFirstClaimed)},
			{nameof(complete3Rating), new Tuple<int, bool>(MultiplayerLevelCompleted, _3RatingClaimed)},
			{nameof(complete3Story), new Tuple<int, bool>(0, _3StoryClaimed)}
		};
		set
		{
			(_levelsCreated, _createFirstClaimed) = value[nameof(createFirst)];
			(_multiplayerLevelCompleted, _3RatingClaimed) = value[nameof(complete3Rating)];
			(_, _3StoryClaimed) = value[nameof(complete3Story)];
		}
	}

	private void Awake()
	{
		complete3Rating.progress = MultiplayerLevelCompleted;
		complete3Story.progress = PlayerData.SingleLevelCompleted;
		createFirst.progress = LevelsCreated;
		
		complete3Rating.claimButton.onClick.AddListener(() => ClaimReward(ref _3RatingClaimed));
		complete3Story.claimButton.onClick.AddListener(() => ClaimReward(ref _3StoryClaimed));
		createFirst.claimButton.onClick.AddListener(() => ClaimReward(ref _createFirstClaimed));
	}

	private void Update()
	{
		complete3Rating.claimButton.interactable = !_3RatingClaimed;
		complete3Story.claimButton.interactable = !_3StoryClaimed;
		createFirst.claimButton.interactable = !_createFirstClaimed;
	}
	
	private void ClaimReward(ref bool claimField)
	{
		claimField = true;
		RealtimeDatabase.PushUserData();
	}
}
