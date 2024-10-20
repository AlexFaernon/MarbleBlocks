using System;
using TMPro;
using UnityEngine;

public class StepCounter : MonoBehaviour
{
    public static int Count;
    [SerializeField] private TMP_Text label;

    private void Awake()
    {
        if (GameMode.CurrentGameMode == GameModeType.SinglePlayer && LevelSaveManager.LevelNumber <= 3)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (GameMode.CurrentGameMode == GameModeType.SinglePlayer && LevelSaveManager.LevelNumber == 0)
        {
            Debug.Log("single 0");
            return;
        }
        
        var optimalSteps = LevelSaveManager.LoadedLevel.OptimalTurns;
        if (GameMode.CurrentGameMode == GameModeType.LevelEditor)
        {
            label.text = $"{Count}";
            return;
        }
        label.text = $"{Count}/{optimalSteps}";
        if (Count > optimalSteps)
        {
            label.color = new Color32(255, 85, 85, 255);
        }
    }

    private void OnDestroy()
    {
        Count = 0;
    }
}
