using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardRecord : MonoBehaviour
{
    [SerializeField] private TMP_Text positionInLeaderboard;
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text levelsCompleted;
    [SerializeField] private TMP_Text trophy;

    private void Awake()
    {
        positionInLeaderboard.text = (transform.GetSiblingIndex() + 1).ToString();
    }

    public void SetPlayerInfo(string name, int levels, int trophy)
    {
        playerName.text = name;
        levelsCompleted.text = levels.ToString();
        this.trophy.text = trophy.ToString();
    }
}
