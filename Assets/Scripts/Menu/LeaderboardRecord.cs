using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LeaderboardRecord : MonoBehaviour
{
    [SerializeField] private TMP_Text positionInLeaderboard;
    [SerializeField] private TMP_Text playerNameLabel;
    [SerializeField] private TMP_Text levelsCompleted;
    [SerializeField] private TMP_Text rankLabel;
    [SerializeField] private Image rankIcon;
    [SerializeField] private Sprite currentPlayerSprite;

    private void Awake()
    {
        positionInLeaderboard.text = (transform.GetSiblingIndex() + 1).ToString();
    }

    public void SetPlayerInfo(string playerName, int levels, int rank, Sprite rankIconSprite)
    {
        playerNameLabel.text = playerName;
        if (playerName == PlayerData.Name)
        {
            GetComponent<Image>().sprite = currentPlayerSprite;
        }
        rankIcon.sprite = rankIconSprite;
        levelsCompleted.text = levels.ToString();
        rankLabel.text = rank.ToString();
    }
}
