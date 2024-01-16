using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Editor : MonoBehaviour
{
    private Button _button;
    public void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() =>
            {
                GameMode.CurrentGameMode = GameModeType.LevelEditor;
                SceneManager.LoadScene("LevelEditor");
            });
    }

    private void Update()
    {
        _button.interactable = PlayerData.SingleLevelCompleted >= 4;
    }
}
