using Firebase.Auth;
using System;
using System.Collections;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
	public static PlayerClass PlayerClass
	{
		get => new()
		{
			Level = Level,
			Coins = Coins,
			Exp = Exp,
			SingleLevelCompleted = SingleLevelCompleted
		};
		set
		{
			Level = value.Level;
			Coins = value.Coins;
			Exp = value.Exp;
			_singleLevelCompleted = value.SingleLevelCompleted;
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

	public static string Name;
	public static int Level;
	public static int Exp;
	public static int Coins;
	public static int Energy = 5;
	public static int Rank;

	private void Awake()
	{
		if (FindObjectsOfType<PlayerData>().Length > 1)
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(this);
	//	StartCoroutine(WaitToUserLogin());
	}

	public static void SetName()
	{
		Name = AuthManager.User.DisplayName;
	}
	
	// private static IEnumerator WaitToUserLogin()
	// {
	// 	yield return new WaitUntil(() => AuthManager.User is not null);
	//
	// 	var user = AuthManager.User;
	// 	Name = user.DisplayName;
	// }
}
