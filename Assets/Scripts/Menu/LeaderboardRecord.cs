using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardRecord : MonoBehaviour
{
    [SerializeField] private TMP_Text positionInLeaderboard;
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text levelsCompleted;
    [SerializeField] private TMP_Text trophy;
    [SerializeField] private Sprite currentPlayerSprite;

    private void Awake()
    {
        positionInLeaderboard.text = (transform.GetSiblingIndex() + 1).ToString();
    }

    public void SetPlayerInfo(string name, int levels, int trophy)
    {
        playerName.text = name;
        if (name == PlayerData.Name)
        {
            GetComponent<Image>().sprite = currentPlayerSprite;
        }
        levelsCompleted.text = levels.ToString();
        this.trophy.text = trophy.ToString();
    }
}
