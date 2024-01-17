using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MultiplayerOpponentInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text opponentName;
    [SerializeField] private TMP_Text opponentLevel;
    [SerializeField] private TMP_Text optimalSteps;

    public void UpdateInfo()
    {
        opponentName.text = RealtimeDatabase.Opponent;
        opponentLevel.text = RealtimeDatabase.OpponentLevel.ToString();
        optimalSteps.text = LevelSaveManager.LoadedLevel.OptimalTurns.ToString();
    }
}
