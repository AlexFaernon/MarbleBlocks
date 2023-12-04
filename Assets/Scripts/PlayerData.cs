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
			Energy = Energy
		};
		set
		{
			Level = value.Level;
			Coins = value.Coins;
			Energy = value.Energy;
		}
	}

	public static string Name;
	public static int Level;
	public static int Coins;
	public static int Energy;
	
	private PlayerClass _playerClass;

	private void Awake()
	{
		if (FindObjectsOfType<PlayerData>().Length > 1)
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(this);
		StartCoroutine(WaitToUserLogin());
	}

	private static IEnumerator WaitToUserLogin()
	{
		yield return new WaitUntil(() => AuthManager.User is not null);

		var user = AuthManager.User;
		Name = user.DisplayName;
	}
}
