using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowRankRewardInLeaderboard : MonoBehaviour
{
    [SerializeField] private Sprite current;
    [SerializeField] private RankString rank;

    private void Awake()
    {
        if (rank == PlayerData.GetRankString())
        {
            GetComponent<Image>().sprite = current;
        }
    }
}
