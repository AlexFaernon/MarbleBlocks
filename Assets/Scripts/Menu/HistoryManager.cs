using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryManager : MonoBehaviour
{
	[SerializeField] private GameObject historyRecord;
	public static Dictionary<string, Tuple<int, bool>> History;
	private readonly Dictionary<RankString, Sprite> _rankIcons = new();

	private void Awake()
	{
		StartCoroutine(LoadHistory());
		foreach (RankString rankString in Enum.GetValues(typeof(RankString)))
		{
			_rankIcons[rankString] = Resources.Load<Sprite>($"Rank/small/{rankString}");
		}
	}

	private IEnumerator LoadHistory()
	{
		StartCoroutine(RealtimeDatabase.ExportHistory());
		yield return new WaitUntil(() => RealtimeDatabase.HistoryLoaded);

		foreach (var pair in History)
		{
			var record = Instantiate(historyRecord, transform).GetComponent<HistoryRecord>();
			var playerName = pair.Key;
			var isWin = pair.Value.Item2;
			var playerRank = LeaderboardManager.LeaderboardData[playerName].Item2;
			record.SetRecord(playerName, pair.Value.Item1, isWin, _rankIcons[PlayerData.GetRankString(playerRank)]);
		}
	}
}