using System;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public static int Count;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Feesh") || col.CompareTag("Sonic") || col.CompareTag("Jumper"))
        {
            col.gameObject.SetActive(false);
            switch (GameMode.CurrentGameMode)
            {
                case GameModeType.SinglePlayer:
                    WinLoseManager.WinSingleplayer.SetActive(true);
                    break;
                case GameModeType.MultiPlayer:
                    WinLoseManager.WinMultiplayer.SetActive(true);
                    break;
                case GameModeType.LevelEditor:
                    WinLoseManager.WinSingleplayer.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    private void OnEnable()
    {
        Count++;
    }

    private void OnDisable()
    {
        Count--;
    }
}
