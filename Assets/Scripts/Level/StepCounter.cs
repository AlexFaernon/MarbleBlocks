using TMPro;
using UnityEngine;

public class StepCounter : MonoBehaviour
{
    public static int Count;
    [SerializeField] private TMP_Text label;

    private void Awake()
    {
        Count = 0;
    }

    private void Update()
    {
        if (GameMode.CurrentGameMode == GameModeType.SinglePlayer && LevelSaveManager.LevelNumber == 0)
        {
            return;
        }
    
        var optimalSteps = LevelSaveManager.LoadedLevel.OptimalTurns;
        if (optimalSteps == 0)
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
}
