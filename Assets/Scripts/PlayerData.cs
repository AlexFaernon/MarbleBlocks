using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public static class PlayerData
{
	public static PlayerClass PlayerClass
	{
		get => new()
		{
			Level = Level,
			Coins = Coins,
			Exp = Exp,
			SingleLevelCompleted = SingleLevelCompleted,
			AchievementsAndQuest = DailyQuestsManager.QuestDict
				.Concat(AchievementManager.AchievementDict)
				.ToDictionary(pair => pair.Key, pair => pair.Value),
			LastLogin = DateTimeOffset.FromUnixTimeMilliseconds((long)AuthManager.User.Metadata.LastSignInTimestamp).DateTime.ToString(),
			LastOpponentName = LastOpponentName,
			FreeOpponentSkip = FreeOpponentSkip
		};
		set
		{
			Level = value.Level;
			Coins = value.Coins;
			Exp = value.Exp;
			_singleLevelCompleted = value.SingleLevelCompleted;
			DailyQuestsManager.QuestDict = value.AchievementsAndQuest;
			AchievementManager.AchievementDict = value.AchievementsAndQuest;
			LastLogin = DateTime.Parse(value.LastLogin);
			_lastOpponentName = value.LastOpponentName;
			_freeOpponentSkip = value.FreeOpponentSkip;
		}
	}

	private static int _singleLevelCompleted;
	public static int SingleLevelCompleted
	{
		get => _singleLevelCompleted;
		set
		{
			_singleLevelCompleted = value; 
			RealtimeDatabase.PushUserData();
		}
	}

	public static string Name => AuthManager.User?.DisplayName;

	private static string _lastOpponentName;
	public static string LastOpponentName
	{
		get => _lastOpponentName;
		set
		{
			_lastOpponentName = value;
			RealtimeDatabase.PushUserData();
		}
	}

	private static bool _freeOpponentSkip = true;

	public static bool FreeOpponentSkip
	{
		get => _freeOpponentSkip;
		set
		{
			_freeOpponentSkip = value;
			RealtimeDatabase.PushUserData();
		}
	}

	public static int Level;
	public static int Exp;
	public static int Coins;
	public static int Energy = 5;
	public static DateTime LastLogin;
	public static int Rank;

	public static RankString GetRankString(int rank = -1)
	{
		if (rank == -1)
		{
			rank = Rank;
		}
		return rank switch
		{
			< 50 => RankString.Дерево,
			>= 50 and < 100 => RankString.Серый,
			>= 100 and < 250 => RankString.Бронза,
			>= 250 and < 500 => RankString.Серебро,
			>= 500 and < 800 => RankString.Золото,
			>= 800 => RankString.Бриллиант
		};
	}
	public static int GetNextRankThreshold(int currentRank)
	{
		return currentRank switch
		{
			< 50 => 50,
			>= 50 and < 100 => 100,
			>= 100 and < 250 => 250,
			>= 250 and < 500 => 500,
			>= 500 and < 800 => 800,
			>= 800 => 0
		};
	}
	
	public static void HandleRankChanged(object sender, ValueChangedEventArgs args)
	{
		if (args.DatabaseError != null)
		{
			Debug.LogError(args.DatabaseError.Message);
			return;
		}

		Rank = int.Parse(args.Snapshot.Value.ToString());
		Debug.Log($"rank updated for {Rank}");
	}
}

public enum RankString
{
	Дерево,
	Серый,
	Бронза,
	Серебро,
	Золото,
	Бриллиант
}
