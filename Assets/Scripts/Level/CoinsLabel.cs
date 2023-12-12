using System;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class CoinsLabel : MonoBehaviour //todo разгрести эти хуйню
{
	private static Random _random = new();
	private void Awake()
	{
		ExpLevelManager.Exp += 100;
		var coinsAdded = _random.Next(1, 3);
		CoinsManager.Coins += coinsAdded;
		PlayerData.SingleLevelCompleted = Math.Max(PlayerData.SingleLevelCompleted, LevelSaveManager.LevelNumber);
		Debug.Log(PlayerData.SingleLevelCompleted);
		
		GetComponent<TMP_Text>().text = $"+{coinsAdded}";
	}
}
