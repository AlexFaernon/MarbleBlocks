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

    private void Awake()
    {
        //Debug.Log(JsonConvert.SerializeObject());
        StartCoroutine(SetLeaderboard());
    }

    private IEnumerator SetLeaderboard()
    {
        yield return new WaitUntil(() => RealtimeDatabase.LeaderboardLoaded);
        
        foreach (var leaderboardRecord in LeaderboardData.OrderBy(pair => pair.Value.Item2))
        {
            var record = Instantiate(leaderboardRecordPrefab, transform).GetComponent<LeaderboardRecord>();
            record.SetPlayerInfo(leaderboardRecord.Key, leaderboardRecord.Value.Item1, leaderboardRecord.Value.Item2);
        }
    }
}
