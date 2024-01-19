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
	[SerializeField] private Image rankIcon;
	[SerializeField] private Image nextRankIcon;

	private void Awake()
	{
		var nextRank = PlayerData.GetNextRankThreshold(PlayerData.Rank);
		var rankString = PlayerData.GetRankString();
		rankLabel.text = rankString.ToString();
		rankIcon.sprite = Resources.Load<Sprite>($"Rank/big/{rankString}");
		var nextRankString = nextRank == 0 ? RankString.Бриллиант : PlayerData.GetRankString(nextRank);
		nextRankIcon.sprite = Resources.Load<Sprite>($"Rank/small/{nextRankString}");
		
		rankProgress.text = $"{PlayerData.Rank}/{(nextRank > 0 ? nextRank : "-")}";
		progressBar.fillAmount =  nextRank > 0 ? (float)PlayerData.Rank / nextRank : 1;

		var utcNow = DateTime.UtcNow;
		var timeLeft = new DateTime(utcNow.Year, utcNow.Month, 1).AddMonths(1) - utcNow;
		timerLabel.text = $"{timeLeft.Days} д. {timeLeft.Hours} ч.";
	}
}
