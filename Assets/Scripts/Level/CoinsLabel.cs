using System;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class CoinsLabel : MonoBehaviour
{
	private static Random _random = new();
	private void Awake()
	{
		var coinsAdded = 0;
		if (!PlayerPrefs.HasKey($"level{LevelSaveManager.LevelNumber}"))
		{
			PlayerPrefs.SetInt($"level{LevelSaveManager.LevelNumber}", Convert.ToInt32(true));
			ExpLevelManager.Exp += 100;
			coinsAdded = _random.Next(1, 3);
			CoinsManager.Coins += coinsAdded;
		}
		
		GetComponent<TMP_Text>().text = $"+{coinsAdded}";
	}
}
