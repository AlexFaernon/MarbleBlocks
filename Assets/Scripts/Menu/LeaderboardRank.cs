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
		var nextRank = PlayerData.GetNextRankThreshold(PlayerData.Rank);
		rankLabel.text = PlayerData.Rank switch
		{
			< 50 => "-",
			>= 50 and < 100 => "Серый",
			>= 100 and < 250 => "Бронза",
			>= 250 and < 500 => "Серебро",
			>= 500 and < 800 => "Золото",
			>= 800 => "Бриллиант"
		};
		rankProgress.text = $"{PlayerData.Rank}/{(nextRank > 0 ? nextRank : "-")}";
		progressBar.fillAmount =  nextRank > 0 ? (float)PlayerData.Rank / nextRank : 1;

		var utcNow = DateTime.UtcNow;
		var timeLeft = new DateTime(utcNow.Year, utcNow.Month, 1).AddMonths(1) - utcNow;
		timerLabel.text = $"{timeLeft.Days} д. {timeLeft.Hours} ч.";
	}
}
