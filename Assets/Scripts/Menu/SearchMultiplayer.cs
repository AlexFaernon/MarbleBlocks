using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SearchMultiplayer : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    private void Update()
    {
        _button.interactable = AchievementManager.LevelsCreated > 0;
    }

    private void OnClick()
    {
        GameMode.CurrentGameMode = GameModeType.MultiPlayer;
        SceneManager.LoadScene("Level");
    }
}