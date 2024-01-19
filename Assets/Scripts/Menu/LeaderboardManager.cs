using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private GameObject leaderboardRecordPrefab;
    public static Dictionary<string, Tuple<int, int>> LeaderboardData;
    private readonly Dictionary<RankString, Sprite> _rankIcons = new();

    private void Awake()
    {
        StartCoroutine(SetLeaderboard());
        foreach (RankString rankString in Enum.GetValues(typeof(RankString)))
        {
            _rankIcons[rankString] = Resources.Load<Sprite>($"Rank/small/{rankString}");
        }
    }

    private IEnumerator SetLeaderboard()
    {
        yield return new WaitUntil(() => RealtimeDatabase.LeaderboardLoaded);
        
        foreach (var leaderboardRecord in LeaderboardData.OrderByDescending(pair => pair.Value.Item2))
        {
            var record = Instantiate(leaderboardRecordPrefab, transform).GetComponent<LeaderboardRecord>();
            var levelsCompleted = leaderboardRecord.Value.Item1;
            var rank = leaderboardRecord.Value.Item2;
            record.SetPlayerInfo(leaderboardRecord.Key, levelsCompleted, rank, _rankIcons[PlayerData.GetRankString(rank)]);
        }
    }
}
