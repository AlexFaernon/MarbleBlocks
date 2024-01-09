using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardRank : MonoBehaviour
{
	[SerializeField] private TMP_Text rankLabel;
	[SerializeField] private TMP_Text timerLabel;
	[SerializeField] private TMP_Text rankProgress;
	[SerializeField] private Image progressBar;

	private void Awake()
	{
		int nextRank;
		switch (PlayerData.Rank)
		{
			case < 50:
				rankLabel.text = "-";
				nextRank = 50;
				break;
			case >= 50 and < 100:
				rankLabel.text = "Серый";
				nextRank = 100;
				break;
			case >= 100 and < 250:
				rankLabel.text = "Бронза";
				nextRank = 250;
				break;
			case >= 250 and < 500:
				rankLabel.text = "Серебро";
				nextRank = 500;
				break;
			case >= 500 and < 800:
				rankLabel.text = "Золото";
				nextRank = 800;
				break;
			case >= 800:
				rankLabel.text = "Бриллиант";
				nextRank = 0;
				break;
		}
		rankProgress.text = $"{PlayerData.Rank}/{(nextRank > 0 ? nextRank : "-")}";
		progressBar.fillAmount =  nextRank > 0 ? (float)PlayerData.Rank / nextRank : 1;

		var utcNow = DateTime.UtcNow;
		var timeLeft = new DateTime(utcNow.Year, utcNow.Month, 1).AddMonths(1) - utcNow;
		timerLabel.text = $"{timeLeft.Days} д. {timeLeft.Hours} ч.";
	}
}
